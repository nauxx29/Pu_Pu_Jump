using UnityEngine;

public class CameraFollow : MonoBehaviour
{ 
    [SerializeField] private Transform target;
    private float lastY;

    private void Start()
    {
        EventCenter.OnRestart.AddListener(Reset);
        lastY = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        if (target.position.y > transform.position.y)
        {
            Vector3 newPosition = new Vector3(transform.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, GameConst.CAMERA_SMOOTH_SPEED);
        }

        if (transform.position.y > lastY + GameConst.SPAWN_INTERVAL_Y)
        {
            StairManager.Instance.SpawnStair();
            lastY = transform.position.y;
        }
    }

    private void OnDestroy()
    {
        EventCenter.OnRestart.RemoveListener(Reset);
    }

    private void Reset()
    {
        transform.position = Vector3.zero;
        lastY = transform.position.y;
    }

}

