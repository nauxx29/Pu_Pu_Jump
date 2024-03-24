using UnityEngine;

public class BoundaryValue : MonoBehaviour
{
    public static float LeftX { get; private set; }
    public static float RightX { get; private set; }

    private void Start()
    {
        LeftX = ScreenBoundaries.GetBorderPositionX(BorderSide.Left);
        RightX = ScreenBoundaries.GetBorderPositionX(BorderSide.Right);
        Debug.Log($"L = {LeftX}, R = {RightX}");
    }
}
