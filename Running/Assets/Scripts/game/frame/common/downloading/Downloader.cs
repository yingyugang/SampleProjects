using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Downloader : MonoBehaviour
{
	public string downloaderName;
	private WWW www;
	public DownloadingFileData downloadingFileData;
	private const float INCORRECT_PROGRESS = 0.5f;

	public UnityAction<Downloader,DownloadingFileData,AssetBundle> downloaderComplete;
	public UnityAction<Downloader,DownloadingFileData,string> downloaderError;
	public UnityAction<float> downloaderProgress;
	private float reachedSize;
	private int retryCount;

	public void StartDownload (DownloadingFileData downloadingFileData)
	{
		reachedSize = 0;
		this.downloadingFileData = downloadingFileData;
		StartCoroutine (Download ());
	}

	public void ReDownload ()
	{
		StartCoroutine (Download ());
	}

	private IEnumerator Download ()
	{
		www = new WWW (PathConstant.GetPathFromDownloadingFileType (downloadingFileData.fileType, true) + downloadingFileData.fileName + "?" + Random.Range (0, int.MaxValue));
		yield return www;
		if (www.isDone && string.IsNullOrEmpty (www.error)) {
			FileManager.WriteAllBytes (PathConstant.GetPathFromDownloadingFileType (downloadingFileData.fileType, false) + downloadingFileData.fileName, www.bytes);
			downloaderComplete (this, downloadingFileData, downloadingFileData.isAssetBundle == 1 ? www.assetBundle : null);
		}
	}

	private void Update ()
	{
		if (www != null && !string.IsNullOrEmpty (www.error)) {
			Retry ();
		}
	}

	public float CurrentSize {
		get {
			if (string.IsNullOrEmpty (www.error) && www.progress != INCORRECT_PROGRESS) {
				float currentSize = downloadingFileData.fileSize * www.progress;
				if (currentSize > reachedSize) {
					reachedSize = currentSize;
				}
			}
			return reachedSize;
		}
	}

	private void OnDestroy ()
	{
		www.Dispose ();
		www = null;
	}

	private void Retry ()
	{
		www.Dispose ();
		www = null;
		ReDownload ();
	}
}