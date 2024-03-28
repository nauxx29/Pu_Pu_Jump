using Firebase.Analytics;
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
    [SerializeField] private AudioSource _bgMusic;

    private bool isOpen;
    private bool isLoadScene => PlayerManager.Instance == null;

    public static bool VibrationSetting { get; private set; }
    public static bool MusicSetting { get; private set; }

    private void Awake()
    {
        VibrationSetting = PlayerPrefs.GetInt(SaveKey.VIBRATION) == 1;
        MusicSetting = PlayerPrefs.GetInt(SaveKey.MUSIC) == 1;
    }

    private void Start()
    {
        isOpen = false;
    }

    // should Not trigger
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void OnClickSideMenu()
    {
        isOpen = !isOpen;
        FirebaseAnalytics.LogEvent("SideMenu", new Parameter("isOpen", isOpen.ToString()));

        _panel.raycastTarget = isLoadScene ? false : isOpen;
        _sideMenuRenderer.sprite = isOpen ? _closeSprite : _threeLineSprite;
        _vibrateButton.gameObject.SetActive(isOpen);
        _musicButton.gameObject.SetActive(isOpen);
        _exitButton.SetActive(isOpen);
    }

    public void OnClickMusic()
    {
        OnChageSettingValue(SaveKey.MUSIC, _musicButton);
        EventCenter.OnMusicChange.Invoke();
    }

    public void OnClickVibration()
    {
        OnChageSettingValue(SaveKey.VIBRATION, _vibrateButton);
    }

    public void OnClickQuit()
    {
        FirebaseAnalytics.LogEvent("QuitBtn");
        PlayerPrefs.Save();
        Application.Quit();
    }

    private void OnChageSettingValue(string key, SideMenuButton button)
    {
        bool targetSetting;
        switch (key)
        {
            case SaveKey.MUSIC:
                MusicSetting = !MusicSetting;
                targetSetting = MusicSetting;
                break;
            case SaveKey.VIBRATION:
                VibrationSetting = !VibrationSetting;
                targetSetting = VibrationSetting;
                break;
            default:
                targetSetting = false;
                Debug.LogError("Change Setting Value is not SaveKey value");
                break;
        }

        int boolToIntValue = targetSetting == true ? 1 : 0;
        PlayerPrefs.SetInt(key, boolToIntValue);
        button.UpdateColor(targetSetting);
    }
}
