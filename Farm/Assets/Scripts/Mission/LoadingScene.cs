using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    UIProgressBar progressBar;
    UILabel lbPercent;
    AsyncOperation async;
    Transform main;
    Transform black;
    public bool show;
    string scene = "";
    float time_delay;

    void Awake()
    {
        time_delay = 0;
        show = false;
        main = transform.Find("Main");
        black = transform.Find("Black");
        //DontDestroyOnLoad(this.gameObject);
        progressBar = main.Find("ProgressBar").GetComponent<UIProgressBar>();
        lbPercent = main.Find("ProgressBar").Find("Label").GetComponent<UILabel>();
        //if (!startGame)
        //{
        //    main.localScale = new Vector3(0, 0, 0);
        //    black.localScale = new Vector3(0, 0, 0);
        //    main.GetComponent<UITexture>().enabled = false;
        //    black.GetComponent<UITexture>().enabled = false;
        //}
    }

    void Update()
    {
        if (show && async != null)
        {
            progressBar.value = async.progress;
            int per = (int)(async.progress * 100);
            lbPercent.text = per + "%";
            if (per >= 100)
            {
                time_delay += 0.01f;
                if (time_delay >= 0.2f)
                {
                    time_delay = 0;
                    HideLoading();
                }
                //Debug.Log("time_delay......." + time_delay + " Time.deltaTime " + Time.deltaTime);
                //Neu ve mission van dang o trang thai pause thi loading se ko bao gio bi an di
                if (Time.deltaTime == 0)
                {
                    if ("Mission".Equals(Application.loadedLevelName))
                    {
                        Time.timeScale = 1;
                    }
                }
            }
            //Debug.Log("loading......." + per + "%");
        }
        if (scene != "")
        {
            time_delay += 0.01f;
            if (time_delay > 0.2f)
            {
                time_delay = 0;
                async = Application.LoadLevelAsync(scene);
                scene = "";
            }
        }
        //Debug.Log("CHAY UPDATE");
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            HideLoading();
        }
    }

    public void HideLoading()
    {
        CommonObjectScript.isViewPoppup = false;
        show = false;
        main.localScale = new Vector3(0, 0, 0);
        black.localScale = new Vector3(0, 0, 0);
        main.GetComponent<UITexture>().enabled = false;
        black.GetComponent<UITexture>().enabled = false;
        progressBar.value = 0;
        lbPercent.text = "0%";
        //Debug.Log("---------------HIDE--------------------");
    }

    void ShowLoading(string scene, bool inGame)
    {
        time_delay = 0;
        if (async != null)
        {
            async = null;
        }
        show = true;
        if (inGame)
        {
            black.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            main.localScale = new Vector3(1, 1, 1);
        }
        main.GetComponent<UITexture>().enabled = true;
        black.GetComponent<UITexture>().enabled = true;
        if (string.IsNullOrEmpty(scene))
        {
            Debug.LogError("Level scene is empty!");
        }
        else
        {
            this.scene = scene;
        }
        CommonObjectScript.isViewPoppup = true;
        //Debug.Log("---------------SHOW--------------------");
    }

    public static void ShowLoadingScene(string scene, bool inGame = false)
    {
        GameObject.Find("LoadingScene").GetComponent<LoadingScene>().ShowLoading(scene, inGame);
    }

    public static void HideLoadingScene()
    {
        LoadingScene load = GameObject.Find("LoadingScene").GetComponent<LoadingScene>();
        if (load.show)
        {
            load.HideLoading();
        }
    }
}
