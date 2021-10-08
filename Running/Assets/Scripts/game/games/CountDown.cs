using UnityEngine;
using System.Collections;

public class CountDown : MonoBehaviour
{
    public delegate void OnEndCountDown();
    public event OnEndCountDown onEndCountDownCallBack;
    public Animator m_Animator;

    public static CountDown s_Instance;

    private void Awake()
    {
        s_Instance = this;
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void SetVisible()
    {
        Time.timeScale = 1;
        transform.parent.gameObject.SetActive(false);
        if (onEndCountDownCallBack != null)
        {
            onEndCountDownCallBack();
        }
    }
}
