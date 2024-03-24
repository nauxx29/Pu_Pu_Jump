using UnityEngine;

public static class GameConst
{
    public const float SPAWN_INTERVAL_Y = 1.2f;
    public const float SPAWN_MARGIN_Y = 0.1f;

    public const float CAMERA_SMOOTH_SPEED = 0.3f;
    public const float SCREEN_LEFT = -2.5f;
    public const float SCREEN_RIGHT = 2.5f;
    public const int INIT_STAIR_AMOUNT = 20;

    public class Wood
    {
        public const string WOOD_CLIP = "WoodClip";
        public static readonly Vector3 WOOD_SCALE = new(0.7f, 0.7f, 0.7f);
    }

    public class Straw
    {
        public const string STRAW_CLIP = "StrawClip";
        public static readonly Vector3 STRAW_SCALE = new(0.64f, 0.64f, 0.64f);
        public static readonly Color STRAW_COLOR = new Color(255, 255, 255, 255);
    }

    public class AnimationName
    {
        public const string SIDEMENU_FADE_IN = "SideMenuFadeIn";
        public const string SIDEMENU_CLOSE = "SideMenuClose";
    }

}



public static class SaveKey
{
    public const string FIRST_PLAY = "firstPlay";
    public const string VIBRATION = "vibration";
    public const string MUSIC = "music";
}

