using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PopUpButton : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _extractingSprite;
    [SerializeField] private Sprite _buildingSprite;

    private Button _button;
    private ButtonType _currentButton;

    public event Action<ButtonType> Clicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void Show(ButtonType buttonType)
    {
        _currentButton = buttonType;
        SetSprite(buttonType);
        gameObject.SetActive(true);
    }

    public void Hide(ButtonType buttonType)
    {
        if (_currentButton == buttonType)
            gameObject.SetActive(false);
    }

    private void OnButtonClick()
    {
        Clicked?.Invoke(_currentButton);
    }

    private void SetSprite(ButtonType buttonType)
    {
        _image.sprite = buttonType switch
        {
            ButtonType.Extracting => _extractingSprite,
            ButtonType.Building => _buildingSprite,
            _ => throw new NotImplementedException(),
        };
    }

    public enum ButtonType
    {
        Extracting,
        Building
    }
}
