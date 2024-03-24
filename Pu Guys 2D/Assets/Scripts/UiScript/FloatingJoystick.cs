using UnityEngine;

public class FloatingJoystick : MonoBehaviour
{
    public RectTransform RectTransform => rectTransform;
    [SerializeField] private RectTransform rectTransform;

    public RectTransform Knob => knob;
    [SerializeField] private RectTransform knob;
}