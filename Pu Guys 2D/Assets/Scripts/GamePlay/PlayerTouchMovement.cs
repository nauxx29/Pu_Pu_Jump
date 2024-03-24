using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;


public class PlayerTouchMovement : MonoSingleton<PlayerTouchMovement>
{
    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoystick joystick;

    private Finger movementFinger;
    public Vector2 MovementAmount => movementAmout;
    private Vector2 movementAmout;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
    }

    private void HandleFingerDown(Finger touchFinger)
    {
        if (movementAmout == Vector2.zero && touchFinger.screenPosition.x <= Screen.width / 2f) 
        {
            movementFinger = touchFinger;
            movementAmout = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.RectTransform.anchoredPosition = touchFinger.screenPosition; //ClampStartPosition(touchFinger.screenPosition);
        }
    }


    private void HandleFingerUp(Finger upFinger)
    {
        if (upFinger == movementFinger)
        {
            movementFinger = null;
            joystick.Knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
            movementAmout = Vector2.zero;
        }
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2f;
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, joystick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - joystick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joystick.RectTransform.anchoredPosition;
            }

            joystick.Knob.anchoredPosition = knobPosition;
            movementAmout = knobPosition / maxMovement;
        }
    }
}
