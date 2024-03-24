using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicScripts : MonoBehaviour
{
    private void Awake()
    {
        Vibration.Init();

        if (PlayerPrefs.GetInt(SaveKey.FIRST_PLAY) == 0)
        {
            PlayerPrefs.SetInt(SaveKey.FIRST_PLAY, 1);
            PlayerPrefs.SetInt(SaveKey.VIBRATION, 1);
            PlayerPrefs.SetInt(SaveKey.MUSIC, 1);
            PlayerPrefs.Save();
        }
    }

    public void OnClickedRestart()
    {
        EventCenter.OnRestart.Invoke();
        StairManager.Instance.Reset();
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
            PlayerPrefs.Save();
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}


