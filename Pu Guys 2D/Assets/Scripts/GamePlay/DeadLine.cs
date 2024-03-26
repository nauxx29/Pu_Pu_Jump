using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("DeadLine collision trigger");
        // Check if the colliding object should be destroyed
        if (collision.CompareTag("Platform"))
        {
            var stair = collision.gameObject.GetComponent<Stair>();
            if (stair != null)
            {
                StairManager.Instance.ReturnToPool(stair.Type, stair);
            }
        }
        else if (collision.CompareTag("Player"))
        {
            Debug.Log("Collide player should sleep");
            PlayerManager.Instance.GameOver();
        }
    }
}
