using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _slider;
    [SerializeField] private Image _sliderDown;

    [SerializeField] private Color _playerColor;
    [SerializeField] private Color _enemyColor;

    private Character _targetCharacter;
    private Camera _camera;
    private Transform _target;
    private float _health;

    private float _sliderDownSpeed = 0.5f;
    private const float _sliderOffset = 0.05f;

    private void OnEnable()
    {
        if (_targetCharacter != null)
            _targetCharacter.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _targetCharacter.HealthChanged -= OnHealthChanged;
    }

    public void Initialize(Transform target, float maxHealth, bool isPlayer, Character character)
    {
        _target = target;
        _health = maxHealth;
        _text.text = maxHealth.ToString();
        _targetCharacter = character;

        if (isPlayer == true)
            _slider.color = _playerColor;
        else
            _slider.color = _enemyColor;

        _camera = FindObjectOfType<Camera>();
        OnEnable();
        OnHealthChanged(maxHealth);
    }

    private void Update()
    {
        transform.position = _camera.WorldToScreenPoint(_target.position);
    }

    private void OnHealthChanged(float health)
    {
        _text.text = ((int)health).ToString();
        float procent = health / _health;
        _slider.fillAmount = procent;
        StartCoroutine(MovingSliderDown());
    }

    private IEnumerator MovingSliderDown()
    {
        yield return new WaitForSeconds(_sliderDownSpeed);

        while (_sliderDown.fillAmount > _slider.fillAmount - _sliderOffset)
        {
            _sliderDown.fillAmount -= _sliderDownSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(MovingSliderDown());
    }
}
