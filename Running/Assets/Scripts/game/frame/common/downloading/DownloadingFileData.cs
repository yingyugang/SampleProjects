using UnityEngine;
using System.Collections;

public class DownloadingFileData
{
	public string fileName;
	public DownloadingFileTypeEnum fileType;
	public int fileSize;
	public int isAssetBundle;
	public int isCSV;
	public string hashCode;

	public DownloadingFileData (string fileName, DownloadingFileTypeEnum fileType, int fileSize, int isAssetBundle, int isCSV, string hashCode)
	{
		this.fileName = fileName;
		this.fileType = fileType;
		this.fileSize = fileSize;
		this.isAssetBundle = isAssetBundle;
		this.isCSV = isCSV;
		this.hashCode = hashCode;
	}
}
