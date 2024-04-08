using UnityEngine;

public class FpsLimiter : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1f / 60f;
    }

}
