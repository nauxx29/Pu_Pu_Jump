//using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void OnClickedRestart()
    {
        EventCenter.OnRestart.Invoke();
        StairManager.Instance.Restart();
        UiManager.Instance.TogglePanel(false);
    }

    public void OnClickedRevive()
    {
        if (PlayerManager.Instance.AlreadyRevived)
        {
            return;
        }
        // Order matter
        StairManager.Instance.OnReviveResetStrawStair();
        PlayerManager.Instance.RevivePlayer();
        GhostManager.Instance.OnRevive();
        UiManager.Instance.TogglePanel(false);
    }

    private void Update()
    {
        CheckQuitBtn();
    }

    private void CheckQuitBtn()
    {
        if (SceneManager.GetActiveScene() != null && Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //FirebaseAnalytics.LogEvent("App_Out");
            PlayerPrefs.Save();
        }
        else
        {
            //FirebaseAnalytics.LogEvent("App_In");
        }
    }

    private void OnApplicationQuit()
    {
        //FirebaseAnalytics.LogEvent("App_Quit");
        PlayerPrefs.Save();
    }
}


