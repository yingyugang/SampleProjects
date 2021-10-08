using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
public class CheckPackage : MonoBehaviour {
	static string[] listPackageOff=new string[]{"net.da.super","mr.sai.stuff","com.cih.game_cih","cn.mc.sq","org.aqua.gg","cc.cz.madkite.freedom","cc.madkite.freedom"};
	static string[] listPackageOnline=new string[50];
	static string url = "http://apipackage.yome.vn/api/get-package/v1?version_code=1";
	static string text;
	//static List<Package> listPackageOnline;
	static bool check_online;


	void Start () {
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));

		}



	static AndroidJavaObject getUnityPlayerObject ()
	{
		AndroidJavaClass parentClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activityObject = parentClass.GetStatic<AndroidJavaObject> ("currentActivity");
		
		return activityObject;
	}
	
	static AndroidJavaClass prepareLog ()
	{
		return new AndroidJavaClass ("com.splaygame.checkpackage.Check");
	}
	
	public static bool check(){

		int number = 0;
		string[] xx=prepareLog ().CallStatic<string[]> ("check",getUnityPlayerObject ());


		for(int i=0;i<listPackageOff.Length;i++){
			for(int j=0;j<xx.Length;j++){
				if(listPackageOff[i]==xx[j])
					number+=1;
			}
		}

		if (check_online) {

			for(int i=0;i<listPackageOnline.Length;i++){
				for(int j=0;j<xx.Length;j++){
					if(listPackageOnline[i]==xx[j])
						number+=1;
				}
			}


				}

		if (number > 0)
			return true;
		else
			return false;
	
		
	}

	  


	 IEnumerator WaitForRequest(WWW www)
	{
		
		yield return www;
		
		if (www.error == null)
		{
			
			text = www.text;	
			PackageInfo dt=JsonConvert.DeserializeObject<PackageInfo>(text);			
			check_online=dt.success;
			if(dt.success){
			for(int i=0;i<dt.package_list.Count;i++){
			listPackageOnline[i]= dt.package_list[i].package;

				}


			}



			
		}

		
	}
}
