using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{

    private void Start()
    {
        EventCenter.OnRestart.AddListener(ResetCamerw);
    }

    private void OnDestroy()
    {
        EventCenter.OnRestart.RemoveListener(ResetCamerw);
    }

    private void ResetCamerw()
    {
        SetCameraPosition(Vector3.zero);
    }

    public void SetCameraPosition(Vector3 newPostiion)
    {
        Camera.main.transform.position = newPostiion;
    }
}
