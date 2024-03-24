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
        UpdateColor();
    }

    public void UpdateColor()
    {
        bool SettingControl = setting == SideMenuSetting.Vibrate ? SideMenuUi.VibrationSetting : SideMenuUi.MusicSetting;
        _image.color = SettingControl ? _turnOnColor : _turnOffColor;
    }

}
