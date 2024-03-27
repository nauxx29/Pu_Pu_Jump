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
        _panel.raycastTarget = isLoadScene ? false : isOpen;
        _sideMenuRenderer.sprite = isOpen ? _closeSprite : _threeLineSprite;
        _vibrateButton.gameObject.SetActive(isOpen);
        _musicButton.gameObject.SetActive(isOpen);
        _exitButton.SetActive(isOpen);
    }

    public void OnClickMusic()
    {
        OnChageSettingValue(SaveKey.MUSIC, MusicSetting);
        _musicButton.UpdateColor(MusicSetting);
        if (MusicSetting)
        {
            AudioManager.Instance.Play();
        }
        else
        {
            AudioManager.Instance.Stop();
        }
        
    }

    public void OnClickVibration()
    {
        OnChageSettingValue(SaveKey.VIBRATION, VibrationSetting);
        _vibrateButton.UpdateColor(VibrationSetting);
    }

    public void OnClickQuit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    private void OnChageSettingValue(string key, bool targetSeting)
    {
        targetSeting = !targetSeting;
        int boolToIntValue = targetSeting == true ? 1 : 0;
        PlayerPrefs.SetInt(key, boolToIntValue);
    }
}
