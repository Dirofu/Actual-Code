using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerAttackRange : MonoBehaviour
{
    [SerializeField] private GameObject _choosedEffect;
    [SerializeField] private List<Character> _enemies;

    private SphereCollider _collider;
    private PlayerSkillsData _skills;
    private PlayerMovement _movement;
    private PlayerAnimation _animation;
    private Character _character;
    private Character _currentEnemy;

    private float _standartColliderRange;

    private Vector3 _effectOffset = new Vector3(0, 0.05f, 0);
    private int _currentRangeLevel = -1;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _skills = GetComponentInParent<PlayerSkillsData>();
        _character = GetComponentInParent<Character>();
        _movement = GetComponentInParent<PlayerMovement>();
        _animation = GetComponentInParent<PlayerAnimation>();

        _standartColliderRange = _collider.radius;
    }

    private void Update()
    {
        StartAttackEnemy();
        ApplyLevelAttackRange();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character) &&
            character.IsDied == false &&
            CheckContainsEnemyInList(character) == false)
        {
            SubcribeOnDeath(character);
            _enemies.Add(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            UnSubcribeOnDeath(character);
            DeleteEnemyFromList(character);
        }
    }

    private void StartAttackEnemy()
    {
        if (_movement.IsMoving == false && _character.IsDied == false)
        {
            if (_currentEnemy != null)
                StartCoroutine(AttackEnemyInRadius());
            else
                SetNearestEnemy();
        }
    }

    private void ApplyLevelAttackRange()
    {
        if (_currentRangeLevel != _skills.AttackRangeLevel)
        {
            _currentRangeLevel = _skills.AttackRangeLevel;
            _collider.radius = _standartColliderRange + _currentRangeLevel;
        }
    }

    private void SubcribeOnDeath(Character character) => character.Died += ChooseNewEnemy;
    private void UnSubcribeOnDeath(Character character) => character.Died -= ChooseNewEnemy;

    private void ChooseNewEnemy(Character character)
    {
        DeleteEnemyFromList(character);

        if (_enemies.Count > 0)
            SetNearestEnemy();
    }

    private void SetNearestEnemy()
    {
        float minDistance = float.MaxValue;

        foreach (var item in _enemies)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                _currentEnemy = item;
            }
        }
    }

    private bool CheckContainsEnemyInList(Character enemy) => _enemies.Contains(enemy);

    private void DeleteEnemyFromList(Character enemy)
    {
        _currentEnemy = null;
        _enemies.Remove(enemy);
    }

    private IEnumerator AttackEnemyInRadius()
    {
        _animation.ChangeAttackAnimationState(true);
        _choosedEffect.SetActive(true);

        while (_movement.IsMoving == false && _currentEnemy != null)
        {
            _choosedEffect.transform.position = _currentEnemy.transform.position + _effectOffset;
            _movement.transform.LookAt(_currentEnemy.transform);
            yield return new WaitForEndOfFrame();
        }

        _currentEnemy = null;
        _choosedEffect.SetActive(false);
        _animation.ChangeAttackAnimationState(false);
        StopCoroutine(AttackEnemyInRadius());
    }
}