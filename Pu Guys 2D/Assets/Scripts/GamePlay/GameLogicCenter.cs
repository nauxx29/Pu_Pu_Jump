using Firebase.Analytics;
using UnityEngine;

public class GameLogicCenter : MonoBehaviour
{
    private void Awake()
    {
        Vibration.Init();
        BoundaryValue.Init();
        if (PlayerPrefs.GetInt(SaveKey.FIRST_PLAY) == 0)
        {
            PlayerPrefs.SetInt(SaveKey.FIRST_PLAY, 1);
            PlayerPrefs.SetInt(SaveKey.VIBRATION, 1);
            PlayerPrefs.SetInt(SaveKey.MUSIC, 1);
            PlayerPrefs.SetInt(SaveKey.BEST_SCORE, 0);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        // Need to Init After BoundaryValue.Init()
        StairManager.Instance.Init();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            FirebaseAnalytics.LogEvent("app_out");
            PlayerPrefs.Save();
        }
        else
        {
            FirebaseAnalytics.LogEvent("app_in");
        }
    }

    private void OnApplicationQuit()
    {
        FirebaseAnalytics.LogEvent("app_quit");
        PlayerPrefs.Save();
    }
}


