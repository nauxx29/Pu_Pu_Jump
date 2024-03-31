using UnityEngine;
using UnityEngine.UI;

enum SideMenuSetting
{
    Vibrate,
    Music
}

public class SideMenuButton : MonoBehaviour
{
    [SerializeField] private SideMenuSetting setting;
    [SerializeField] private Image _image;
    [SerializeField] private Color _turnOnColor;
    [SerializeField] private Color _turnOffColor;
    

    private void OnEnable()
    {
        bool isON = setting == SideMenuSetting.Vibrate ? PlayerRunTimeSettingData.VibrationSetting : PlayerRunTimeSettingData.MusicSetting;
        UpdateColor(isON);
    }

    public void UpdateColor(bool isOn)
    {
        _image.color = isOn ? _turnOnColor : _turnOffColor;
    }

}
