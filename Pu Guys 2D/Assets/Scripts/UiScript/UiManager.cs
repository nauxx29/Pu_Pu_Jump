using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoSingleton<UiManager> 
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highScore;
    [SerializeField] private Button _reviveButton;
    [SerializeField] private GameObject _gameoverPanel;
    [SerializeField] private GameObject _rvNotReadyPopup;

    private void Start()
    {
#if UNITY_EDITOR
        _reviveButton.interactable = true;
#else
        _reviveButton.interactable = false;
#endif
        int initScore = 0;
        UpdateScore(initScore);

        PlayerRunTimeSettingData.SetVibrate(PlayerPrefs.GetInt(SaveKey.VIBRATION) == 1);
        PlayerRunTimeSettingData.SetMusic(PlayerPrefs.GetInt(SaveKey.MUSIC) == 1);
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void TogglePanel(bool sholdPanelActive)
    {
        _reviveButton.gameObject.SetActive(!PlayerManager.Instance.AlreadyRevived);
        _gameoverPanel.SetActive(sholdPanelActive);
    }

    public void OnClickedRestart()
    {
        EventCenter.OnRestart.Invoke();
        StairManager.Instance.Restart();
        TogglePanel(false);
    }

    public void OnClickedRevive()
    {
        void OnRvReward()
        {
            StairManager.Instance.OnReviveResetStrawStair();
            PlayerManager.Instance.RevivePlayer();
            GhostManager.Instance.OnRevive();
            TogglePanel(false);
        }

        if (PlayerManager.Instance.AlreadyRevived)
        {
            Debug.LogError("Already Revive but still press revive button");
            return;
        }
#if UNITY_EDITOR
        OnRvReward();
#elif UNITY_ANDROID
        if (AdsHelper.Instance == null)
        {
            Debug.LogError("AdsHelper.Instance == null");
            return;
        }

        AdsHelper.Instance.OnTryShowRv(RvPlacement.GAME_OVER, OnRvReward, OnReviveRvFail);
#endif
    }

    void OnReviveRvFail()
    {
        _rvNotReadyPopup.SetActive(true);
        _reviveButton.gameObject.SetActive(false);
    }

    public void SetReviveBtn(bool setOn)
    {
        _reviveButton.interactable = setOn;
    }

    public void OnClickRvFailPopup()
    {
        _rvNotReadyPopup.SetActive(false);
    }

    public void OnRvToggleMusic(bool isPlayingRv)
    {
        PlayerRunTimeSettingData.SetMusic(!isPlayingRv);
        EventCenter.OnMusicChange.Invoke();
    }
}

public static class PlayerRunTimeSettingData
{
    public static bool VibrationSetting { get; private set; }
    public static bool MusicSetting { get; private set; }

    public static void SetVibrate(bool setting)
    {
        VibrationSetting = setting;
    }

    public static void SetMusic(bool setting)
    {
        MusicSetting = setting;
    }
}