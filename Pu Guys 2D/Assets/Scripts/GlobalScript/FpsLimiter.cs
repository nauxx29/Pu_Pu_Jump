using UnityEditor;
using UnityEngine;

public class FpsLimiter : MonoBehaviour
{
    [SerializeField] private int TargetFrameRate = 60;

    void Awake()
    {
        PlayerSettings.Android.optimizedFramePacing = true;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)TargetFrameRate;

    }

}
