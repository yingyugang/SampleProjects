using UnityEngine;
using DG.Tweening;
using UnityEngine.CrashLog;

public class GameInitialization : MonoBehaviour
{
	public PopupLoader popupLoader;
	public APIManager apiManager;
	public DebugManager debugManager;
	public SceneCombineManager sceneCombineManager;
	public AssetBundleResourcesGetter assetBundleResourcesGetter;
	public SoundManager soundManager;
	public VolumeManager volumeManager;
	public Purchaser purchaser;
	public ScreenManager screenManager;
	public GachaPlayer gachaPlayer;
	public ShopInitializer shopInitializer;

	public void Init ()
	{
		Application.targetFrameRate = 45;
		Input.multiTouchEnabled = false;
		DOTween.Init ();

		ComponentConstant.COROUTINE = gameObject;
		ComponentConstant.POPUP_LOADER = popupLoader;
		ComponentConstant.API_MANAGER = apiManager;
		ComponentConstant.DEBUG_MANAGER = debugManager;
		ComponentConstant.SCENE_COMBINE_MANAGER = sceneCombineManager;
		ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER = assetBundleResourcesGetter;
		ComponentConstant.SOUND_MANAGER = soundManager;
		ComponentConstant.VOLUME_MANAGER = volumeManager;
		ComponentConstant.PURCHASER = purchaser;
		ComponentConstant.SCREEN_MANAGER = screenManager;
		ComponentConstant.GACHA_PLAYER = gachaPlayer;
		ComponentConstant.SHOP_INITIALIZER = shopInitializer;
		DontDestroyOnLoad (gameObject);
		CrashReporting.Init ("3a828460-04f7-4a24-9c3a-028d3d715b4f");
        //Init Ads
        #if !UNITY_EDITOR
        AdsManager.AdsInit();
        #endif
	}
}
