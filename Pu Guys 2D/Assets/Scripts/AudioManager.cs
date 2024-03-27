using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioSource _audio;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        bool souldPLayMusic = SideMenuUi.MusicSetting;

        if (souldPLayMusic)
        {
            _audio.Stop();
        }
    }

    public void Stop()
    {
        _audio.Stop();
    }

    public void Play()
    {
        _audio.Play();
    }
}
