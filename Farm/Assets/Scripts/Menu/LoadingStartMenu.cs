using UnityEngine;
using System.Collections;

public class LoadingStartMenu : MonoBehaviour
{

    public static bool showGift;
    public static bool showFullBanner;
    bool goMenu;


    void Awake()
    {
        goMenu = false;
        DFB.FBInit();
        showGift = true;
        showFullBanner = false;
        
    }
    
    void Update()
    {
        if (!goMenu)
        {
            VariableSystem.Start();
            DataCache.GetAchievementCacheData();
            goMenu = true;
            LoadingScene.ShowLoadingScene("Menu");
            
        }
    }
}
