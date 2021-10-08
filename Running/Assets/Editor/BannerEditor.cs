using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ZucksAdNetworkBanner))]
public class BannerEditor : Editor {

	public override void OnInspectorGUI()
	{
		var ZucksAdNetworkBanner = target as ZucksAdNetworkBanner;

		ZucksAdNetworkBanner.Banner =
			EditorGUILayout.RectField ("Banner", ZucksAdNetworkBanner.Banner);
	}
}
