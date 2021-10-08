using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.iOS;

public class SystemConstant
{
	private static string A_DEVICE_ID = "a_device_id";
	public const string CLIENT_VERSION = "2.0.0";
	private static string hashCode;
	private static string uuid;
	private static string userAgent;

	public static string HashCodeOfVersionFile {
		get {
			if (FileManager.Exists (PathConstant.CLIENT_CLIENT_VERSION_CSV)) {
				hashCode = FileManager.GetFileHash (PathConstant.CLIENT_CLIENT_VERSION_CSV);
				return hashCode;
			}
			return null;
		}
	}

	public static string UUID {
		get {
			if (FileManager.Exists (PathConstant.UUID)) {
				uuid = FileManager.ReadString (PathConstant.UUID);
				return uuid;
			} else {
				return CreateUUID ();
			}
		}
	}

	public static string DeviceID {
		get {
			string deviceid = string.Empty;
			if (FileManager.Exists (PathConstant.DEVICEID)) {
				deviceid = FileManager.ReadString (PathConstant.DEVICEID);
				if (string.IsNullOrEmpty (deviceid)) {
					deviceid = Alternative_DeviceID;
					SetDeviceID (deviceid);
				} else {
					Alternative_DeviceID = deviceid;
				}
			} else {
				deviceid = Alternative_DeviceID;
				SetDeviceID (deviceid);
			}
			return deviceid;
		}
		set {
			if (!string.IsNullOrEmpty (value)) {
				Alternative_DeviceID = value;
				SetDeviceID (value);
			}
		}
	}

	private static void SetDeviceID	(string deviceid)
	{
		FileManager.WriteString (PathConstant.DEVICEID, deviceid);
	}

	public static void ClearDeviceID ()
	{
		PlayerPrefs.SetString (A_DEVICE_ID, string.Empty);
		FileManager.WriteString (PathConstant.DEVICEID, string.Empty);
	}

	public static string Alternative_DeviceID {
		get { 
			return PlayerPrefs.GetString (A_DEVICE_ID, string.Empty);
		}
		private set { 
			PlayerPrefs.SetString (A_DEVICE_ID, value);
		}
	}

	private static string CreateUUID ()
	{
		uuid = Guid.NewGuid ().ToString ();
		FileManager.WriteString (PathConstant.UUID, uuid);
		return uuid;
	}

	public static string UserAgent {
		get {
			if (userAgent != null) {
				return userAgent;
			}
			Dictionary<string, string> systemInformation = SystemInformation;
			foreach (string key in systemInformation.Keys) {
				userAgent += key + ":" + systemInformation [key] + "||";
			}
			return userAgent;
		}
	}

	private static Dictionary<string, string> SystemInformation {
		get {
			Dictionary<string, string> systemInformation = new Dictionary<string, string> ();
			systemInformation.Add ("operatingSystem", SystemInfo.operatingSystem);
//			systemInformation.Add ("deviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
			systemInformation.Add ("deviceModel", SystemInfo.deviceModel);
			systemInformation.Add ("deviceName", SystemInfo.deviceName);
			systemInformation.Add ("graphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
			systemInformation.Add ("processorType", SystemInfo.processorType);
			systemInformation.Add ("graphicsMemorySize", SystemInfo.graphicsMemorySize.ToString ());
			systemInformation.Add ("systemMemorySize", SystemInfo.systemMemorySize.ToString ());

			#if UNITY_IOS
			systemInformation.Add ("iPhone.generation", Device.generation.ToString ());
			#endif

			return systemInformation;
		}
	}
}