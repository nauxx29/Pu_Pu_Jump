using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UiManager : MonoSingleton<UiManager> 
{
    public int NowScore { get; private set; }
    private int bestScoreRecord;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Button _reviveButton;
    [SerializeField] private GameObject _gameoverPanel;
    [SerializeField] private GameObject _rvNotReadyPopup;
    [SerializeField] private GameObject _bestScore;
    [SerializeField] private TMP_Text _bestScoreText;

    private void Start()
    {
        void UpdateScoreText()
        {
            NowScore = 0;
            UpdateScore(NowScore);
            bestScoreRecord = PlayerPrefs.GetInt(SaveKey.BEST_SCORE);
            _bestScoreText.text = bestScoreRecord.ToString();
        }

#if UNITY_EDITOR
        _reviveButton.interactable = true;
#else
        _reviveButton.interactable = false;
#endif

        UpdateScoreText();
        _reviveButton.gameObject.SetActive(AdsManager.Instance.IsRvInit);
    }

    public void UpdateScore(int score, Action callback = null)
    {
        NowScore += score;
        _scoreText.text = NowScore.ToString();
        callback?.Invoke();
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
        ResetScore();
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

    public void ToggleHighestScore(bool toggle)
    {
        _bestScore.SetActive(toggle);
    }

    public void UpdateBsetScore(int score)
    {
        _bestScoreText.text = score.ToString();
    }

    private void ResetScore()
    {
        if (NowScore > bestScoreRecord)
        {
            UpdateBsetScore(NowScore);
            bestScoreRecord = NowScore;
            PlayerPrefs.SetInt(SaveKey.BEST_SCORE, NowScore);
            PlayerPrefs.Save();
        }
        ToggleHighestScore(true);
        NowScore = 0;
        UpdateScore(NowScore);
    }
}
