using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum EditValueType
{
    Speed = 0,
    Jump = 1
}

public class Debugger : MonoBehaviour
{
    [SerializeField] private GameObject _debugPanel;
    [SerializeField] private TMP_Text _fpsText;
    [SerializeField] private TMP_InputField _speed;
    [SerializeField] private TMP_InputField _jump;
    [SerializeField] private Toggle _joystickToggle;

    private float updateTimer = .2f;
    private float fpsCount;
    private bool isDebugPanelOpen => _debugPanel.activeSelf;

    private void Start()
    {
        if (!Debug.isDebugBuild)
        {
            Destroy(_debugPanel);
            Destroy(_fpsText.gameObject);
            Destroy(gameObject);
            return;
        }

        _joystickToggle.isOn = PlayerManager.Instance.IsUsingJoyStick;
    }

    private void Update()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0f)
        {
            fpsCount = 1f / Time.unscaledDeltaTime;
            _fpsText.text = "FPS : " + Mathf.Round(fpsCount);
            updateTimer = 0.2f;
        }
    }

    public void OnClickedDebugBtn()
    {
        _debugPanel.SetActive(!isDebugPanelOpen);
        Time.timeScale = isDebugPanelOpen ? 0 : 1;
    }

    public void OnEditEnd(int typeNum)
    {
        int parseValue;
        switch (typeNum)
        {
            case (int)EditValueType.Speed:
                if (int.TryParse(_speed.text, out parseValue))
                {
                    PlayerManager.Instance.DebugSetSpeed(parseValue);
                }
                break;

            case (int)EditValueType.Jump:
                if (int.TryParse(_jump.text, out parseValue))
                {
                    PlayerManager.Instance.DebugSetJump(parseValue);
                }
                break;

            default:
                Debug.LogError("OnEditEnd() perameter not in EditValueType");
                break;
        }
    }

    public void OnClearSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnNoGhost()
    {
        GhostManager.Instance.OnNoGhost();
    }

    public void OnRestart()
    {
        EventCenter.OnRestart.Invoke();
    }

    public void OnJoystick(Toggle toggle)
    {
        PlayerManager.Instance.ToggleJoystick(toggle.isOn);
    }
}
