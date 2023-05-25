using Agava.YandexGames;
using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private GameObject _jumpButton;
    [SerializeField] private MenuOpener _menuOpener;
    [SerializeField] private InputType _inputType;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    public Vector2 Direction { get; private set; }
    public Vector2 LastDirection { get; private set; }
    public InputType InputType => _inputType;

    public UnityAction OnJump;

    private void Awake()
    {
        ChooseInputType();
    }

    private void Update()
    {
        GetInputAxis();
        GetPCJumpButton();

        if (_menuOpener != null)
            GetPauseInput();
        else
            throw new ArgumentNullException(nameof(_menuOpener));
    }

    private void GetInputAxis()
    {
        if (_inputType == InputType.PC)
            Direction = new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical)).normalized;
        else if (_inputType == InputType.Mobile)
            Direction = new Vector2(_joystick.Horizontal, _joystick.Vertical);

        if (Direction.magnitude > 0)
            LastDirection = Direction;
    }

    public void GetMobileJumpButton()
    {
        if (_inputType == InputType.Mobile)
            OnJump?.Invoke();
    }

    private void GetPCJumpButton()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _inputType == InputType.PC)
            OnJump?.Invoke();
    }

    private void GetPauseInput()
    {
        if (_inputType == InputType.PC && Input.GetKeyDown(KeyCode.Escape))
        {
            if (_menuOpener.PauseStatus == false)
                _menuOpener.OpenPauseMenu();
            else
                _menuOpener.ClosePauseMenu();
        }
    }

    private void ChooseInputType()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (Device.Type == Agava.YandexGames.DeviceType.Mobile)
        {
            _inputType = InputType.Mobile;
            _joystick.gameObject.SetActive(true);
            _jumpButton.SetActive(true);
        }
        else
        {
            _inputType = InputType.PC;
            _joystick.gameObject.SetActive(false);
            _jumpButton.SetActive(false);
        }
#endif
    }
}

public enum InputType
{
    PC,
    Mobile
}