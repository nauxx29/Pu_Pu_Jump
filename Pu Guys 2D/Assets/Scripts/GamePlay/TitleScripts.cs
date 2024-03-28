using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScripts : MonoBehaviour
{
    public void startButton()
    {
        FirebaseAnalytics.LogEvent("Start");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
        if (Debug.isDebugBuild)
        {
            Debugger.Instance.OnChangeScene();
        }
    }
}
