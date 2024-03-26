using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public readonly Vector3 START_POSITITON = new Vector3(0, 0, 0);
    public readonly Vector3 REVIVE_ERROR = new Vector3(0, 2, 0);

    public float LaetRecordY {  get; private set; }
    public bool IsAlive { get; private set; }
    public bool IsUsingJoyStick { get; private set; }
    public bool AlreadyRevived { get; private set; }

    private float moveHorizontal;
    private bool isOnTheGround = true;
    private bool isJump = false;
    private Vector3 lastPlatform;
    

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
    }

    private void OnDestroy()
    {
        EventCenter.OnRestart.RemoveListener(Reset);
    }

    private void Start()
    {

#if UNITY_EDITOR
        IsUsingJoyStick = false;
#else
        IsUsingJoyStick = true;
#endif
        LaetRecordY = 0;
        IsAlive = true;

        SetPuAlive(true);
    }

    private void Update()
    {
        if (!IsAlive)
        {
            return;
        }

        if (!IsUsingJoyStick)
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            isJump = Input.GetKey(KeyCode.Space);
        }
        else
        {
            moveHorizontal = PlayerTouchMovement.Instance.MovementAmount.x;
            // jump => button detect
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
        //OnCheckFall();
        OnCheckPlayerPosition();
    }

    private void OnCheckPlayerPosition()
    {
        bool shouldUpdateY = transform.position.y > LaetRecordY;
        if (!shouldUpdateY)
        {
            return;
        }

        LaetRecordY = transform.position.y;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, LaetRecordY, 0);

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
        if (isJump != true || !isOnTheGround)
        {
            return;
        }

        isJump = false;
        isOnTheGround = false;
        AnimationWalk(false);
        JumpForce();
        JumpVfx();
    }

    private void OnCheckBoundary()
    {
        // Jump out from one side then transfer to another side
        if (transform.position.x > BoundaryValue.RightX)
        {
            transform.position = new Vector2(BoundaryValue.LeftX, transform.position.y);
        }
        else if (transform.position.x < BoundaryValue.LeftX)
        {
            transform.position = new Vector2(BoundaryValue.RightX, transform.position.y);
        }
    }

/*    private void OnCheckFall()
    {
        // Drop down Gameover <<Not Finish yet>>
        if (transform.position.y < _deadLine.position.y)
        {
            GameOver();
        }
    }*/

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

    private void Reset()
    {
        AlreadyRevived = false;
        SetPuAlive(true);
        transform.position = START_POSITITON;
    }

    public void RevivePlayer()
    {
        AlreadyRevived = true;
        Vector3 revivePosition = lastPlatform + REVIVE_ERROR;
        transform.position = revivePosition;
        SetPuAlive(true);
    }

    public void DebugSetSpeed(int speed)
    {
        moveSpeed = speed;
    }

    public void DebugSetJump(int jump)
    {
        jumpForce = jump;
    }

    public void ToggleJoystick(bool isOn)
    {
        IsUsingJoyStick = isOn;
    }

    #region Collision
    // if hiiting(collision) sth
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Check pu is on the top of the stair not trigger it from the below
        if (collision.gameObject.tag == "Platform" && collision.relativeVelocity.y > 0f)
        {
            isOnTheGround = true;
            lastPlatform = collision.transform.position;
        }

        else if (collision.gameObject.tag == "Ghost" && IsAlive)
        {
            GameOver();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isOnTheGround = false;
        }
    }
    #endregion
}
