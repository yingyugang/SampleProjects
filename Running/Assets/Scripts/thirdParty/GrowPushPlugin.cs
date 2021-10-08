using UnityEngine;
using System.Collections;

#if UNITY_IPHONE
using NotificationServices = UnityEngine.iOS.NotificationServices;
#endif
public class GrowPushPlugin : MonoBehaviour
{

    void Awake()
    {
        GrowthPush.GetInstance().Initialize("PxJY4L7n3aevP6qq", "pwUtDoJigPojKZYsZUjI24eQcr045oFm", Debug.isDebugBuild ? GrowthPush.Environment.Development : GrowthPush.Environment.Production);
        GrowthPush.GetInstance().RequestDeviceToken("271177066788");
        #if !PRODUCT
        GrowthPush.GetInstance().SetTag("isDevelop");
        #endif
        DontDestroyOnLoad(this.gameObject);
        GrowthPush.GetInstance().ClearBadge();
    }
    // Use this for initialization
    void Start()
    {
        string devicetoken = GrowthPush.GetInstance().GetDeviceToken();
        Debug.Log(devicetoken);
    }

    bool tokenSent = false;
    // Update is called once per frame
    void Update()
    {
        #if UNITY_IPHONE
        if (!tokenSent)
        {
            byte[] token = NotificationServices.deviceToken;
            if (token != null)
            {
                GrowthPush.GetInstance().SetDeviceToken(System.BitConverter.ToString(token).Replace("-", "").ToLower());
                tokenSent = true;
            }
        }
        #endif
    }

    void OnApplicationPause(bool pauseStatus)
    {
        GrowthPush.GetInstance().ClearBadge();
    }
}
