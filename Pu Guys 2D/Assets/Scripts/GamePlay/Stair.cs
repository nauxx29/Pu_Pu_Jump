using UnityEngine;

public class Stair : MonoBehaviour
{
    public static Transform LastStair { get; private set; }
    public bool IsScored{ get; private set; }  

    public StairType Type => _type;
    [SerializeField] private StairType _type;

    public Animator StairAnimator => _animator;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        IsScored = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PlayerManager.Instance.IsOnTheGround)
        {
            return;
        }

        // need to be at leaset 0.1 due to they will have detect error if set to 0
        if (collision.relativeVelocity.y <= 0.1f && gameObject != null && collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.SetOnGround(true);

            switch (_type)
            {
                case StairType.Brick:
                case StairType.Tutorial:
                    break;
                case StairType.Wood:
                    _animator.Play(GameConst.Wood.WOOD_CLIP);
                    break;
                case StairType.Straw:
                    _animator.Play(GameConst.Straw.STRAW_CLIP);
                    break;
            }

            if (LastStair == this || _type == StairType.Tutorial)
            {
                return;
            }

            LastStair = gameObject.transform;
            StairManager.Instance.UpdateScoreAndVibrate(1, this);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.SetOnGround(false);
        }
    }

    public void StrawAnimationCallback()
    {
        StairManager.Instance.UpdateStrawStairPositionRecord(transform.position);
        StairManager.Instance.ReturnToPool(_type, this);
    }

    public void SetStairScored()
    {
        IsScored = true;
    }
}

