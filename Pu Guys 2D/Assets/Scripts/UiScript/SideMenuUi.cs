using UnityEngine;
using UnityEngine.UI;

public class SideMenuUi : MonoBehaviour
{
    [SerializeField] private Image _sideMenuRenderer;
    [SerializeField] private Sprite _closeSprite;
    [SerializeField] private Sprite _threeLineSprite;
    [SerializeField] private SideMenuButton _vibrateButton;
    [SerializeField] private SideMenuButton _musicButton;
    [SerializeField] private GameObject _exitButton;
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _panel;

    public static bool VibrationSetting => vibrateSetting;
    public static bool MusicSetting => musicSetting;
    private static bool vibrateSetting;
    private static bool musicSetting;
    private bool isOpen;

    private void Start()
    {
        isOpen = false;
        vibrateSetting = PlayerPrefs.GetInt(SaveKey.VIBRATION) == 1;
        musicSetting = PlayerPrefs.GetInt(SaveKey.MUSIC) == 1;
    }

    // should Not trigger
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void OnClickSideMenu()
    {
        isOpen = !isOpen;
        _panel.raycastTarget = isOpen;
        _sideMenuRenderer.sprite = isOpen ? _closeSprite : _threeLineSprite;
        _vibrateButton.gameObject.SetActive(isOpen);
        _musicButton.gameObject.SetActive(isOpen);
        _exitButton.SetActive(isOpen);
    }

    public void OnClickMusic()
    {
        int boolValue = musicSetting ? 1 : 0;
        PlayerPrefs.SetInt(SaveKey.MUSIC, boolValue);
        musicSetting = !musicSetting;
        _musicButton.UpdateColor();
    }

    public void OnClickVibration()
    {
        int boolValue = vibrateSetting ? 1 : 0;
        PlayerPrefs.SetInt(SaveKey.VIBRATION, boolValue);
        vibrateSetting = !vibrateSetting;
        _vibrateButton.UpdateColor();
    }

    public void OnClickQuit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
