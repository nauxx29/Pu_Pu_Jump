using UnityEngine;

public class GhostManager : MonoSingleton<GhostManager>
{
    private const float RESET_DISTANCE = 10f;

    [SerializeField] private Transform _target;
    [SerializeField] private float _slowChase = 1f;
    [SerializeField] private float _fastChase = 4f;
    [SerializeField] private float _fastChaseDistance = 8f;
    [SerializeField] private AudioSource _audioSource;

    private Vector3 originalPosition;

    protected override void Awake()
    {
        base.Awake();
        EventCenter.OnMusicChange.AddListener(AudioSetting);
        EventCenter.OnRestart.AddListener(OnReset);
    }

    private void OnDestroy()
    {
        EventCenter.OnRestart.RemoveListener(OnReset);
        EventCenter.OnMusicChange.RemoveListener(AudioSetting);
    }

    private void Start()
    {
        originalPosition = transform.position;
        AudioSetting();
    }

    private void AudioSetting()
    {
        _audioSource.volume = PlayerRunTimeSettingData.MusicSetting ? GameConst.Volume.GHOST_AS_ORIGINAL_VOULME : 0f;
    }

    // Update is called once per frame
    void Update()
    {
        void FacingTarget()
        {
            if ( _target.position.x - transform.position.x >= 0)
            {
                if (_target.position.x - transform.position.x >= 0)
                {
                    transform.localScale = Vector3.one;
                }
                else
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
        }

        if (!PlayerManager.Instance.IsAlive || !Tutorial.IsTutorialDone)
        {
            return;
        }

        float slowStep = _slowChase * Time.deltaTime;
        float fastStep = _fastChase * Time.deltaTime;
        float nowDistance = Vector2.Distance(_target.position, transform.position);

        // facing Pu
        FacingTarget();

        if (nowDistance < _fastChaseDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, slowStep);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, fastStep);
        }
    }

    private void OnReset()
    {
        transform.position = originalPosition;
    }

    public void OnRevive()
    {
        transform.position = new Vector3(_target.position.x - RESET_DISTANCE, _target.position.y - RESET_DISTANCE, transform.position.z);
    }

    public void OnNoGhost()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerManager.Instance.IsAlive)
        {
            PlayerManager.Instance.GameOver();
        }
    }
}
