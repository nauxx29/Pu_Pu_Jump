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

    private bool isOpen;
    private bool isLoadScene => PlayerManager.Instance == null;

    private void Start()
    {
        isOpen = false;
    }

/*    // should Not trigger
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
*/
    public void OnClickSideMenu()
    {
        isOpen = !isOpen;
        _panel.raycastTarget = isLoadScene ? false : isOpen;
        _sideMenuRenderer.sprite = isOpen ? _closeSprite : _threeLineSprite;
        _vibrateButton.gameObject.SetActive(isOpen);
        _musicButton.gameObject.SetActive(isOpen);
        _exitButton.SetActive(isOpen);

        if (!isOpen)
        {
            PlayerPrefs.Save();
        }
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
        PlayerPrefs.Save();
        Application.Quit();
    }

    private void OnChageSettingValue(string key, SideMenuButton button)
    {
        switch (key)
        {
            case SaveKey.MUSIC:
                PlayerRunTimeSettingData.SetMusic(!PlayerRunTimeSettingData.MusicSetting);
                PlayerPrefs.SetInt(key, BoolToInt(PlayerRunTimeSettingData.MusicSetting));
                button.UpdateColor(PlayerRunTimeSettingData.MusicSetting);
                break;
            case SaveKey.VIBRATION:
                PlayerRunTimeSettingData.SetVibrate(!PlayerRunTimeSettingData.VibrationSetting);
                PlayerPrefs.SetInt(key, BoolToInt(PlayerRunTimeSettingData.VibrationSetting));
                button.UpdateColor(PlayerRunTimeSettingData.VibrationSetting);
                break;
            default:
                Debug.LogError("Change Setting Value is not SaveKey value");
                break;
        }
    }

    private int BoolToInt(bool boolValue)
    {
        int value = boolValue ? 1 : 0;
        return value;
    }
}

