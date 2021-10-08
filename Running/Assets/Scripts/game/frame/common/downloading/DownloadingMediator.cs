using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class DownloadingMediator : SceneInitializer
{
	public Downloading downloading;
	public DownloadingManager downloadingManager;
	public CSVParser csvParser;
	public DownloadingFileFilter downloadingFileFilter;
	public DownloadingFileDataQueueCreator downloadingFileDataQueueCreator;
	public AssetCSVReader assetCSVReader;
	public UnityAction complete;

	private void Awake ()
	{
		ComponentConstant.SCENE_COMBINE_MANAGER.unityAction = (LongLoadingMediator longLoadingMediator) => {
			ComponentConstant.SCENE_COMBINE_MANAGER.unityAction = null;
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.downloading_local.ToString (), (List<Texture2D> list) => {
				downloading.rogerScrollGrid.initComplete = () => {
					downloading.container.SetActive (true);
					Download ();
				};
				var sortedList = from items in list
				                 orderby int.Parse(items.name)
				                 select items;
				list = sortedList.ToList ();
				downloading.rogerScrollGrid.Init (TextureToSpriteConverter.ConvertToSpriteList (list));
				InitScene ();
				longLoadingMediator.Hide ();
			}, false, true));
		};
	}

	override protected IEnumerator CheckSceneResources ()
	{
		yield return null;
		InitScene ();
	}

	public void InitScene ()
	{
		if (PathConstant.CheckIfExistingVersionCSV ()) {
			FileManager.DeleteFile (PathConstant.CLIENT_SERVER_VERSION_CSV);
			FileManager.DeleteFile (PathConstant.CLIENT_CLIENT_VERSION_CSV);
		}

		downloading.Clear ();
		ShowOrHideDownloading (true);
		ShowOrHideArrows (false);
	}

	private void Download ()
	{
		DownloadCSV ();
	}

	private void ShowOrHideArrows (bool isShow)
	{
		downloading.rogerScrollGrid.ArrowLeft.gameObject.SetActive (isShow);
		downloading.rogerScrollGrid.ArrowRight.gameObject.SetActive (isShow);
	}

	private void ShowOrHideDownloading (bool isShow)
	{
		downloading.gameObject.SetActive (isShow);
	}

	private void DownloadAssets ()
	{
		csvParser.Parse ();
		List<VersionCSVStructure> filteredVersionCSVStructureList = downloadingFileFilter.Filter (VersionCSV.versionCSV);
		if (filteredVersionCSVStructureList.Count == 0) {
			downloading.UpdateProgress (1);
			Invoke ("DownloadCompleteHandler", 1);
			return;
		}
		downloadingManager.progress = (float f) => {
			downloading.UpdateProgress (f);
		};
		downloadingManager.complete = DownloadCompleteHandler;
		Queue<DownloadingFileData> downloadingFileDataQueue = downloadingFileDataQueueCreator.CreateForDownloadingAssets (filteredVersionCSVStructureList);
		downloadingManager.StartDownload (downloadingFileDataQueue, true, downloadingFileDataQueueCreator.totalSize);
	}

	private void DownloadCSV ()
	{
		downloadingManager.complete = DownloadAssets;
		downloadingManager.StartDownload (downloadingFileDataQueueCreator.CreateForDownloadingCSV ());
	}

	private void DownloadCompleteHandler ()
	{
		ParseCSV ();
	}

	private void FinishDownloading ()
	{
		downloadingManager.complete = null;
		downloadingManager.progress = null;
		downloading.container.SetActive (false);
		ShowOrHideDownloading (false);
		FileManager.CopyFile (PathConstant.CLIENT_SERVER_VERSION_CSV, PathConstant.CLIENT_CLIENT_VERSION_CSV);
		if (complete != null) {
			complete ();
		}
		GameConstant.hasDownloaded = true;
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Main);
	}

	private void ParseCSV ()
	{
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle (AssetBundleName.csv.ToString (), (AssetBundle assetBundle) => {
			#if !READ_LOCAL_CSV
			assetCSVReader.Read (assetBundle);
			#endif
			FinishDownloading ();
		}));
	}

	private void OnDestroy ()
	{
		downloadingManager.complete = null;
		downloadingManager.progress = null;
	}
}
