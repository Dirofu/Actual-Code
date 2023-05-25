using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileArrow : Weapon
{
    #region Fields
    [SerializeField] private float _speed;
    [SerializeField] private RangeExplosion _explosionPrefab;

    private float _standartRebounceCount;
    private float _currentRebounceCount;

    private Rigidbody _rigidbody;
    private PlayerSkillsData _skills;
    private RangeExplosion _explosion;

    private Vector3 _direction;
    #endregion

    public Character Character => CurrentCharacter;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (_skills != null && 
            (_currentRebounceCount != _standartRebounceCount || 
            _standartRebounceCount != _skills.RebounceProjectileCount))
        {
            _standartRebounceCount = _skills.RebounceProjectileCount;
            _currentRebounceCount = _standartRebounceCount;
        }

        _direction = transform.forward * _speed;
    }

    private void Update()
    {
        if (_rigidbody.velocity != _direction)
            _rigidbody.velocity = _direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision);
    }

    public override void Initialize(Character character)
    {
        base.Initialize(character);

        if (character.IsPlayer == true)
        {
            _skills = character.GetComponent<PlayerSkillsData>();
        }
    }

    private void HandleHit(Collision collision)
    {
        if (collision.gameObject.layer != IgnoreColliderLayer)
        {
            if (collision.gameObject.TryGetComponent(out Character character))
            {
                if (IsPlayerShot == character.IsPlayer)
                    return;

                character.TakeDamage(CurrentCharacter.Damage);

                UseExplosionEffect();

                gameObject.SetActive(false);
            }
            else
            {
                if (_currentRebounceCount > 0)
                {
                    _currentRebounceCount--;
                    _direction = Vector3.Reflect(_direction, collision.contacts[0].normal);

                    Vector3 lookDirection = new Vector3(transform.position.x + _direction.x, transform.position.y, transform.position.z + _direction.z);
                    transform.LookAt(lookDirection);
                }
                else
                {
                    UseExplosionEffect();
                    gameObject.SetActive(false);
                }
            }

            Triggered?.Invoke();
        }
    }

    private void UseExplosionEffect()
    {
        if (_explosionPrefab != null && 
            _skills != null &&
            _skills.ExploseArrow == true)
        {
            _explosion = Instantiate(_explosionPrefab);
            _explosion.Initialize(_skills);
            _explosion.gameObject.SetActive(true);
            _explosion.transform.position = transform.position;
        }
    }
}