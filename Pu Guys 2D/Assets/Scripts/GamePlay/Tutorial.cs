using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static bool IsTutorialDone => isTutorialDone;
    private static bool isTutorialDone;

    private void Start()
    {
        EventCenter.OnRestart.AddListener(Reset);
    }

    private void OnDestroy()
    {
        EventCenter.OnRestart.RemoveListener(Reset);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTutorialDone = true;
        }
    }

    private void Reset()
    {
        isTutorialDone = false;
    }
}
