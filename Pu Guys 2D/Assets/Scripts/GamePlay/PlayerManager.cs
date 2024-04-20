using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public readonly Vector3 START_POSITITON = new Vector3(0, 0, 0);
    public readonly Vector3 REVIVE_ERROR = new Vector3(0, 2, 0);

    public float LastRecordY {  get; private set; }
    public bool IsAlive { get; private set; }
    public bool IsUsingJoyStick { get; private set; }
    public bool AlreadyRevived { get; private set; }
    public bool IsOnTheGround { get; private set; }

    private float moveHorizontal;
    private bool isJump = false;

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _jumpAudioClip;

    [Header("MovementDetect")]
    [SerializeField] private InputActionReference _moveActionToUse;

    [Header("Game Mechanic")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f; 

    protected override void Awake()
    {
        base.Awake();
        EventCenter.OnRestart.AddListener(Reset);
        EventCenter.OnMusicChange.AddListener(AudioSetting);
    }

    private void OnDestroy()
    {
        EventCenter.OnRestart.RemoveListener(Reset);
        EventCenter.OnMusicChange.RemoveListener(AudioSetting);
    }

    private void Start()
    {

#if UNITY_EDITOR
        IsUsingJoyStick = false;
#else
        IsUsingJoyStick = true;
#endif
        LastRecordY = 0;
        IsAlive = true;
        AudioSetting();
        SetPuAlive(true);
    }

    private void Update()
    {
        if (!IsAlive)
        {
            return;
        }

        if (IsUsingJoyStick)
        {
            moveHorizontal = PlayerTouchMovement.Instance.MovementAmount.x;
            // jump => button detect
        }
        else
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            isJump = Input.GetKey(KeyCode.Space);
        }
            
    }

    // Update for physic system inside Unity per frame
    private void FixedUpdate()
    {
        if (!IsAlive)
        {
            return;
        }

        OnCheckMoving();
        OnCheckJump();
        OnCheckBoundary();
        OnCheckCameraPosition();
    }

    private void OnCheckCameraPosition()
    {
        bool shouldUpdateY = transform.position.y > LastRecordY;
        if (!shouldUpdateY)
        {
            return;
        }

        LastRecordY = transform.position.y;
        CameraManager.Instance.SetCameraPosition(new Vector3(Camera.main.transform.position.x, LastRecordY, 0));

    }

    #region Movement

    public void OnJumpBtnClicked()
    {
        isJump = true;
    }

    private void OnCheckMoving()
    {
        if (moveHorizontal > 0.2f || moveHorizontal < - 0.2f)
        {
            // AddForce has applied Time.Deltatime as default in its ForceMode.
            // so no need to apply it again
            // Please be aware that AddForce() utilizes the object's current velocity to modify the force.
            // This can result in inconsistent movement (( sometime object may move very quickly ))

            Vector2 velocityH = _rb2D.velocity;
            velocityH.x = moveSpeed * moveHorizontal;
            _rb2D.velocity = velocityH;

            // flip 
            if (moveHorizontal > 0.05f)
            {
                transform.localScale = Vector3.one;

            }
            else if (moveHorizontal < 0.05f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);

            }
            AnimationWalk(true);
        }
        else
        {           
            AnimationWalk(false);
        }
    }

    private void OnCheckJump()
    {
        void JumpForce()
        {
            Vector2 velocityV = _rb2D.velocity;
            velocityV.y = jumpForce;
            _rb2D.velocity = velocityV;
        }

        void JumpVfx()
        {
            _audioSource.PlayOneShot(_jumpAudioClip);
        }

        // condition fail
        if (isJump != true || !IsOnTheGround)
        {
            return;
        }

        isJump = false;
        IsOnTheGround = false;
        AnimationWalk(false);
        JumpForce();
        JumpVfx();
        
    }

    private void OnCheckBoundary()
    {
        // Jump out from one side then transfer to another side
        if (transform.position.x > BoundaryValue.RightX + 0.2f)
        {
            transform.position = new Vector2(BoundaryValue.LeftX - 0.2f, transform.position.y);
        }
        else if (transform.position.x < BoundaryValue.LeftX - 0.2f)
        {
            transform.position = new Vector2(BoundaryValue.RightX + 0.2f, transform.position.y);
        }
    }

    #endregion

    private void AnimationWalk(bool isWaliking)
    {
        _animator.SetBool("isWalking", isWaliking);
    }

    // is weird to use PlayerManger to call GameOver()
    public void GameOver()
    {
        SetPuAlive(false);
        UiManager.Instance.TogglePanel(true);
    }

    private void SetPuAlive(bool isPuAlive)
    {
        IsAlive = isPuAlive;
        if (isPuAlive)
        {
            _rb2D.WakeUp();
        }
        else
        {
            _rb2D.Sleep();
        }
    }

    public void Reset()
    {
        AlreadyRevived = false;
        SetPuAlive(true);
        transform.position = START_POSITITON;
        LastRecordY = START_POSITITON.y;
    }

    public void RevivePlayer()
    {
        AlreadyRevived = true;
        Vector3 revivePosition = Stair.LastStair.position + REVIVE_ERROR;
        transform.position = revivePosition;
        SetPuAlive(true);
    }

    private void AudioSetting()
    {
        _audioSource.volume = PlayerRunTimeSettingData.MusicSetting ? GameConst.Volume.PU_AS_ORIGINAL_VOULME : 0f;
    }

    public void SetOnGround(bool isOn)
    {
        IsOnTheGround = isOn;
    }

    #region DEBUG

    public void DebugSetSpeed(int speed)
    {
        moveSpeed = speed;
    }

    public void DebugSetJump(int jump)
    {
        jumpForce = jump;
    }

    public void DebugSetRiggid(float mass, float linear, float angular, float gravity)
    {
        _rb2D.mass = mass;
        _rb2D.drag = linear;
        _rb2D.angularDrag = angular;
        _rb2D.gravityScale = gravity;
    }

    public void ToggleJoystick(bool isOn)
    {
        IsUsingJoyStick = isOn;
    }

    #endregion
}
