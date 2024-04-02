using UnityEngine;

public enum BorderSide
{
    Left,
    Right
}

public static class ScreenBoundaries
{
    public static bool CheckIfOutOfBounds(Vector3 detectPoint)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(detectPoint);

        bool isVisible = screenPoint.x >= 0 && screenPoint.x <= 1 && 
                         screenPoint.y >= 0 && screenPoint.y <= 1;
        return isVisible;
    }

    public static float CheckScreenPointX(Vector3 detectPoint)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(detectPoint);
        return screenPoint.x;
    }

    public static float GetBorderPositionX(BorderSide side)
    {
        float borderPointX = side == BorderSide.Left ? 0 : 1;
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(new Vector3(borderPointX, 0, 0));
        return worldPoint.x; 
    }
}
