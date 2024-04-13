using Firebase.Extensions;
using UnityEngine;
using Firebase;
using Firebase.Crashlytics;
using Firebase.Analytics;

public class FirebaseManager : MonoSingleton<FirebaseManager>
{
    public bool IsInit { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        IsInit = false;
    }

    public void FirebaseInit()
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
                IsInit = true;
#if DEBUG
                Debug.Log("##### Firebase Init OK #####");
#endif
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void FirebaseLog(string eventName)
    {
        if (!IsInit)
        {
            return;
        }

        FirebaseAnalytics.LogEvent(eventName);
    }
}
