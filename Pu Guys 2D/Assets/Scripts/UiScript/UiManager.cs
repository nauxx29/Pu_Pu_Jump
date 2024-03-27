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


}
