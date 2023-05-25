using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ExtractingAmountAnimator : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image; 

    private const string ExtractAnimation = "Extract";
    private const string ExtraText = "+";
    private Animator _animator;

    public float Duration => _animator.GetCurrentAnimatorStateInfo(0).length;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayExtract(ResourceType resourceType, int amount)
    {
        _text.text = ExtraText + amount.ToString();
        _image.sprite = ResourcesStorage.GetSprite(resourceType);
        _animator.Play(ExtractAnimation);
    }
}
