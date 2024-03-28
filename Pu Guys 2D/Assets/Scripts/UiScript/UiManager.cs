using UnityEngine;
using TMPro;

public class UiManager : MonoSingleton<UiManager> 
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highScore;
    [SerializeField] private GameObject _reviveButton;
    [SerializeField] private GameObject _gameoverPanel;


    private void Start()
    {
        int initScore = 0;
        UpdateScore(initScore);
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void TogglePanel(bool sholdPanelActive)
    {
        _reviveButton.SetActive(!PlayerManager.Instance.AlreadyRevived);
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
        if (PlayerManager.Instance.AlreadyRevived)
        {
            return;
        }
        // Order matter
        StairManager.Instance.OnReviveResetStrawStair();
        PlayerManager.Instance.RevivePlayer();
        GhostManager.Instance.OnRevive();
        TogglePanel(false);
    }

}
