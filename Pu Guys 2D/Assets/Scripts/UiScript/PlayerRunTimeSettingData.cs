using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunTimeSettingData : MonoBehaviour
{
    public static bool VibrationSetting { get; private set; }
    public static bool MusicSetting { get; private set; }

    private void Awake()
    {
        SetVibrate(PlayerPrefs.GetInt(SaveKey.VIBRATION) == 1);
        SetMusic(PlayerPrefs.GetInt(SaveKey.MUSIC) == 1);
    }

    public static void SetVibrate(bool setting)
    {
        VibrationSetting = setting;
    }

    public static void SetMusic(bool setting)
    {
        MusicSetting = setting;
    }
}
