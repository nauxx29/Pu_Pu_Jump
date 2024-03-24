using UnityEngine;

public class Stair : MonoBehaviour
{
    protected static Stair lastStair;

    public StairType Type => _type;
    [SerializeField] private StairType _type;

    public Animator StairAnimator => _animator;
    [SerializeField] private Animator _animator;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f && gameObject != null)
        {
            switch (_type)
            {
                case StairType.Brick:
                    break;
                case StairType.Wood:
                    _animator.Play(GameConst.Wood.WOOD_CLIP);
                    break;
                case StairType.Straw:
                    _animator.Play(GameConst.Straw.STRAW_CLIP);
                    break;
            }

            if (lastStair != this)
            {
                lastStair = this;
                StairManager.Instance.UpdateScore(1);
            }
        }
    }

    public void StrawAnimationCallback()
    {
        StairManager.Instance.UpdateStrawStairPositionRecord(transform.position);
        StairManager.Instance.ReturnToPool(_type, this);
    }
}

