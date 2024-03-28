using UnityEngine;

public class BgAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    private void Awake()
    {
        EventCenter.OnMusicChange.AddListener(AudioSetting);
    }

    private void OnDestroy()
    {
        EventCenter.OnMusicChange.RemoveListener(AudioSetting);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        AudioSetting();
    }

    private void AudioSetting()
    {
        _audio.volume = SideMenuUi.MusicSetting ? GameConst.Volume.BG_AS_ORIGINAL_VOULME : 0f;
    }

}
