using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScripts : MonoBehaviour
{
    public void startButton()
    {
        FirebaseAnalytics.LogEvent("start");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
#if DEBUG
        Debugger.Instance.OnChangeScene();
#endif
    }
}
