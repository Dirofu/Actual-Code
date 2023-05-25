using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class AttackEventProjectile : MonoBehaviour
{
    [SerializeField] private Transform _startAttackPoint;
    [SerializeField] private ProjectileTarget _targetEffectPrefab;
    [SerializeField] private EnemyAttackRange _attackRange;
    
    [Header("Pool Settings")]
    [SerializeField] private ProjectilePool _poolPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private Weapon _projectilePrefab;

    private ProjectilePool _pool;
    private Character _character;
    private ProjectileTarget _targetEffect;
    private Weapon _projectile;
    private PlayerSkillsData _skills;

    public float Damage => _character.Damage;

    private void Awake()
    {
        _skills = GetComponent<PlayerSkillsData>();
        _character = GetComponent<Character>();
        InitializePool();

        if (_targetEffectPrefab != null)
            _targetEffect = Instantiate(_targetEffectPrefab);
    }

    private void OnDestroy()
    {
        if (_projectile != null)
            _projectile.Triggered -= OnDisableTargetEffect;

        if (_pool != null)
            Destroy(_pool.gameObject);

        if (_targetEffect != null)
            Destroy(_targetEffect.gameObject);
    }

    public ProjectilePool GetPool() => _pool;

    public void Attack()
    {
        LaunchProjectile();

        if (_skills != null && _skills.DoubleAttackActive == true)
        {
            float secondProjectileTime = .1f;
            StartCoroutine(StartSecondProjectile(secondProjectileTime));
        }
    }

    private IEnumerator StartSecondProjectile(float secondProjectileTime)
    {
        yield return new WaitForSeconds(secondProjectileTime);
        LaunchProjectile();
        StopCoroutine(StartSecondProjectile(secondProjectileTime));
    }

    private void LaunchProjectile()
    {
        if (_pool == null)
            throw new ArgumentNullException(nameof(_pool));

        _projectile = _pool.GetFirstDisabledProjectileFromPool();
        _projectile.transform.position = _startAttackPoint.position;
        _projectile.transform.rotation = _character.transform.rotation;
        _projectile.gameObject.SetActive(true);

        if (_targetEffect != null && _attackRange != null)
        {
            _projectile.Triggered += OnDisableTargetEffect;
            _targetEffect.SetPosition(_attackRange.Player.transform.position);
        }
    }

    private void OnDisableTargetEffect()
    {
        _targetEffect.DisableParticle();

        if (_projectile != null)
            _projectile.Triggered -= OnDisableTargetEffect;
    }

    private void InitializePool()
    {
        _pool = Instantiate(_poolPrefab);
        _pool.InitializePool(_character, _poolSize, _projectilePrefab);
    }
}
