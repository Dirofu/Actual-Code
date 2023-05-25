using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField] private float _attackRange;

    private EnemyMovement _movement;
    private EnemyAnimation _animation;
    private Character _player;
    private Character _current;

    private IEnumerator _attackCoroutine;

    public bool IsCanAttack { get; private set; } = false;
    public Character Player => _player;

    private void Awake()
    {
        _current = GetComponentInParent<Character>();
        _movement = GetComponentInParent<EnemyMovement>();
        _animation = GetComponentInParent<EnemyAnimation>();
    }

    private void Update()
    {
        if (_attackCoroutine == null && _player != null)
        {
            StartAttackTargetIntoDistance();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character) &&
            character.IsPlayer == true)
        {
            _player = character;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Character character) &&
            character.IsPlayer == true)
        {
            _player = null;
        }
    }

    private void StartAttackTargetIntoDistance()
    {
        float distanceToTarget = Vector3.Distance(_current.transform.position, _player.transform.position);

        if (distanceToTarget < _attackRange)
        {
            StartCoroutine(_attackCoroutine = AttackPlayerInRadius());
        }
        else if (distanceToTarget > _attackRange && _attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    private IEnumerator AttackPlayerInRadius()
    {
        _movement.StopMoving();
        IsCanAttack = true;
        _animation.ChangeAttackAnimationState(IsCanAttack);

        while (_player != null && _current.IsDied == false && _player.IsDied == false)
        {
            _movement.transform.LookAt(_player.transform);
            yield return new WaitForEndOfFrame();
        }

        IsCanAttack = false;
        _animation.ChangeAttackAnimationState(IsCanAttack);
        _attackCoroutine = null;
        StopCoroutine(AttackPlayerInRadius());
    }
}