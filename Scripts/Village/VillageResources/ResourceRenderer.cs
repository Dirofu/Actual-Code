using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ResourceRenderer : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] protected TMP_Text AmountText;

    private const string RedAnimation = "Red";
    private const string GreenAnimation = "Green";
    private Animator _animator;

    public ResourceType ResourceType { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public virtual void Render(IReadOnlyResource resource)
    {
        ResourceType = resource.Type;
        _image.sprite = resource.Sprite;
        AmountText.text = resource.Amount.ToString();
    }

    public virtual void Render(ResourceType type, int value)
    {
        ResourceType = type;
        _image.sprite = ResourcesStorage.GetSprite(type);
        AmountText.text = value.ToString();
    }

    public void PlayRedAnimation()
    {
        _animator.Play(RedAnimation);
    }

    public void PlayGreenAnimation()
    {
        _animator.Play(GreenAnimation);
    }
}
