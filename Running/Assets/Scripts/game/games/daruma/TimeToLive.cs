using UnityEngine;

public class TimeToLive : MonoBehaviour {

    public  float   timeToLive          = 5.0f;
    private float   m_RealTime;
    public bool     isDestroy           = false;
    public bool     isHideOutOfCamera   = false;  

    void OnEnable()
    {
        m_RealTime = timeToLive;
    }

    bool IsOutsideCamera()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.y > Screen.height || screenPos.y < 0 || screenPos.x < 0 || screenPos.x > Screen.width)
        {
            return true;
        }
        return false;
    }

    void Update ()
    {
        if (isHideOutOfCamera && IsOutsideCamera())
        {
            if (isDestroy)
            {
                Destroy(gameObject);
                return;
            }
            gameObject.SetActive(false);
        }

        m_RealTime -= Time.deltaTime;
        if (m_RealTime <= 0)
        {
            m_RealTime = timeToLive;

            if (isDestroy)
            {
                Destroy(gameObject);
                return;
            }
            gameObject.SetActive(false);
        }
	}
}
