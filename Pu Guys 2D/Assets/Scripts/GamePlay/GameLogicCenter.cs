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
            FirebaseAnalytics.LogEvent("App_Out");
            PlayerPrefs.Save();
        }
        else
        {
            FirebaseAnalytics.LogEvent("App_In");
        }
    }

    private void OnApplicationQuit()
    {
        FirebaseAnalytics.LogEvent("App_Quit");
        PlayerPrefs.Save();
    }
}


