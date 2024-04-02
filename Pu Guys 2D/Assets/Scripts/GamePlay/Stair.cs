using UnityEngine;

public class Stair : MonoBehaviour
{
    public static Transform lastStair;

    public StairType Type => _type;
    [SerializeField] private StairType _type;

    public Animator StairAnimator => _animator;
    [SerializeField] private Animator _animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f && gameObject != null && collision.gameObject.CompareTag("Player"))
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

            if (lastStair == this || _type == StairType.Tutorial)
            {
                return;
            }

            lastStair = gameObject.transform;
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
}

