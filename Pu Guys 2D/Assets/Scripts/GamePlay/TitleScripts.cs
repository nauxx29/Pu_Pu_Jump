using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScripts : MonoBehaviour
{
    public void StartButton()
    {
        FirebaseManager.Instance.FirebaseLog("start");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
#if DEBUG
        Debugger.Instance.OnChangeScene();
#endif
    }
}
