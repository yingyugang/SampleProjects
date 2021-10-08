using UnityEngine;
using System.Collections;
using System.IO;

public class ClearCacheMediator : PopupContentMediator
{
	protected override void YesButtonOnClickHandler ()
	{
		base.YesButtonOnClickHandler ();
		if (PathConstant.CheckIfExistingVersionCSV ()) {
			FileManager.DeleteFile (PathConstant.CLIENT_SERVER_VERSION_CSV);
			FileManager.DeleteFile (PathConstant.CLIENT_CLIENT_VERSION_CSV);
		}
		if (FileManager.DirectoryExists (PathConstant.CLIENT_ASSETS_PATH))
		{
			string[] stringArray = FileManager.GetFiles (PathConstant.CLIENT_ASSETS_PATH, "*", SearchOption.TopDirectoryOnly);
			foreach (var item in stringArray)
			{
				FileManager.DeleteFile (item);
			}
		}
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Splash);
	}
}
