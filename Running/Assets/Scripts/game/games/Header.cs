using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public enum LifeType
{
    Number,
    Time
}

public class Header : MonoBehaviour
{
    public delegate void OnPause();

    public event OnPause onPause;

    public UnityAction callbackEndCountDown;
    [HideInInspector]
    public bool isPause = false;
    public Text txtTitle;
    public Text txtScore;
    public Text txtLife;
    public Image imgLife;
    public Image imgLifeTimer;
    public Image imgNokori;

	[HideInInspector]
	public PopupContentMediator popupCountDown;


    private static Header m_Instance;
    public static Header Instance
    {
        get
        {
            return m_Instance;
        }
    }

    private int m_Life;
    private int m_MaxLife;
    private LifeType m_LifeType;
    private bool m_NeedCountTime = false;
    private float m_LifeTimer;

	public bool isRunOnAwake = true;

	public Button btnPause;

    public float GetLifeTime()
    {
        return m_LifeTimer;
    }

    public void SetLifeTime(float time)
    {
        m_LifeTimer = time;
    }

    void Awake()
    {
        m_Instance = this;
        SetShowNokori(false);
		if (ComponentConstant.POPUP_LOADER != null && isRunOnAwake)
        {
            popupCountDown = ComponentConstant.POPUP_LOADER.Popup(PopupEnum.GameCountDown);

        }
    }

    public void SetShowNokori(bool isShow = false)
    {
        imgNokori.gameObject.SetActive(isShow);
        GridLayoutGroup grid = imgLifeTimer.GetComponent<GridLayoutGroup>();
        grid.padding.top = isShow ? 0 : 20;
    }

    // Pause game button callback
    public void BtnPause()
    {
        if (!GameCountDownMediator.didEndCountDown)
        {
            return;
        }

        if (ComponentConstant.SOUND_MANAGER != null)
        {
            ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
        }
        if (onPause != null)
        {
            onPause();

        }

        //if (ComponentConstant.POPUP_LOADER != null)
        //{
        //    ComponentConstant.POPUP_LOADER.Popup(PopupEnum.GamePause);
        //}
    }

    public void SetOnPauseCallback(OnPause callback)
    {
        onPause += callback;
    }

    public void RemoveOnPauseCallback(OnPause callback)
    {
        onPause -= callback;
    }

    // Set title
    public void SetTitle(string str)
    {
        txtTitle.text = str;
    }

    // Set title
    public void SetScore(string str)
    {
        txtScore.text = str;
    }

    // Set life
    // if type is Time, value is seconds
    public void SetLife(LifeType type, int value)
    {
        m_LifeType = type;
        m_MaxLife = value;
        UpdateLife(value);
        if (type == LifeType.Time)
            StartTimer();
    }

    public void UpdateLife(int value)
    {
        m_Life = value;
        txtLife.text = value.ToString();
    }

    public void StartTimer()
    {
        m_NeedCountTime = true;
        m_LifeTimer = m_Life;
    }

    public void PauseTimer()
    {
        m_NeedCountTime = false;
    }

    public void ResumeTimer()
    {
        m_NeedCountTime = true;
    }

    public void UpdateTime()
    {
        if (!GameCountDownMediator.didEndCountDown)
        {
            return;
        }

        if (!m_NeedCountTime)
            return;

        if (m_LifeTimer < 0)
            return;


        m_LifeTimer -= Time.deltaTime;
        float percent = m_LifeTimer / m_MaxLife;
        imgLifeTimer.fillAmount = percent;
        m_Life = (int)m_LifeTimer;
        UpdateLife(m_Life);
    }

    void Update()
    {
        if (isPause)
        {
            return;
        }
        UpdateTime();
    }

	//hoantt
	public void ShowPopupCountDown(){
		if (ComponentConstant.POPUP_LOADER != null)
		{
			popupCountDown = ComponentConstant.POPUP_LOADER.Popup(PopupEnum.GameCountDown);

		}
	}

	public void PauseBtnInteractive(bool isEnable)
	{
		btnPause.interactable = isEnable;
	}
}
