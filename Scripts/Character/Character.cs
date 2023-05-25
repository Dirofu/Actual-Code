using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(EffectUser))]
public class Character : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _damage;
    [SerializeField] private CharacterType _characterType;
    
    [Header("UI Settings")]
    [SerializeField] private Transform _healthBarContainer;
    [SerializeField] private HealthBar _healthBarPrefab;
    [SerializeField] private Transform _healthBarPosition;

    private EffectUser _effect;
    private Collider _collider;
    private HealthBar _healthBar;
    private Animator _animator;
    private PlayerSkillsData _skills;
    private float _maxHealth;

    private bool _immortality = false;
    private float _secondsImmortality = 3f;
    private float _secondsToHide = 2.5f;

    public CharacterType Type => _characterType;
    public bool IsPlayer => _characterType == CharacterType.Player;
    public bool IsDied { get; private set; } = false;
    public float Damage => _damage;
    public int Level { get; private set; }

    #region Constantes
    private const string _dead = "Dead";
    private const string _rebirth = "Rebirth";
    #endregion

    public UnityAction<float> HealthChanged;
    public UnityAction<Character> Died;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _effect = GetComponent<EffectUser>();
        _skills = GetComponent<PlayerSkillsData>();

        if (_healthBarContainer == null && _healthBarPrefab != null)
            _healthBarContainer = FindObjectOfType<HealthbarContainer>().transform;

        SetUpCharacterData(_currentHealth, _damage);
    }

    public void RebirthCharacter()
    {
        IsDied = false;
        _currentHealth = _maxHealth;
        _animator.SetTrigger(_rebirth);
        StartCoroutine(RebirthImmortality());
        SetUpHealthBar();
    }

    public void SetUpCharacterData(float health, float damage, int level = 1)
    {
        if (IsPlayer == false)
        {
            _currentHealth = health;
            _damage = damage;
            Level = level;
        }
        else
        {
            _currentHealth = _skills.Health;
            _damage = _skills.Damage;
        }

        SetUpHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0 || _immortality == true)
            return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }

        if (_healthBar != null)
            HealthChanged?.Invoke(_currentHealth);
    }

    private void SetUpHealthBar()
    {
        if (_healthBarPrefab == null)
            return;

        _maxHealth = _currentHealth;

        if (_healthBar == null)
            _healthBar = Instantiate(
                _healthBarPrefab,
                _healthBarPosition.position,
                Quaternion.identity,
                _healthBarContainer);

        _healthBar.Initialize(_healthBarPosition, _maxHealth, IsPlayer, this);
    }

    private void Die()
    {
        if (IsDied == true)
            return;

        _effect.PlayDeathEffect();
        _animator.SetTrigger(_dead);
        IsDied = true;
        Died?.Invoke(this);

        if (IsPlayer == false)
        {
            _effect.StopAttackZoneEffect();
            _collider.enabled = false;
            StartCoroutine(WaitToHideCharacter());
        }
    }

    private IEnumerator WaitToHideCharacter()
    {
        float currentSeconds = _secondsToHide;

        Destroy(_healthBar.gameObject);

        yield return new WaitForSeconds(_secondsToHide);

        while (currentSeconds > 0)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, transform.position - Vector3.up, Time.deltaTime);
            currentSeconds -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
        StopCoroutine(WaitToHideCharacter());
    }

    private IEnumerator RebirthImmortality()
    {
        _effect.PlayRebirthEffect();
        _immortality = true;
        yield return new WaitForSeconds(_secondsImmortality);
        _immortality = false;
        _effect.StopRebirthEffect();
        StopCoroutine(RebirthImmortality());
    }
}

public enum CharacterType
{
    Player,
    Swordman,
    Scout,
    Mage,
    Cavalry
}