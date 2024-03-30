using Firebase.Extensions;
using UnityEngine;
using Firebase;
using Firebase.Crashlytics;
using System.CodeDom;

public class AdsManager : MonoSingleton<AdsManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        void FirebaseInit()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                    Debug.Log("Firebase Init OK");
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        void IronSourceInit()
        {
            IronSource.Agent.init("1e06a87e5", IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL);
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        }

        FirebaseInit();
        IronSourceInit();
        
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
        IronSource.Agent.validateIntegration();
    }
    
    private void SdkInitializationCompletedEvent() 
    {
        Debug.Log("IronSource Init OK");
    }
}
