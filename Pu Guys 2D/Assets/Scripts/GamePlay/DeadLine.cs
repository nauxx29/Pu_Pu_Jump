using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object should be destroyed
        if (collision.CompareTag("Platform"))
        {
            var stair = collision.gameObject.GetComponent<Stair>();
            if (stair != null)
            {
                StairManager.Instance.ReturnToPool(stair.Type, stair);
            }
        }
    }
}
