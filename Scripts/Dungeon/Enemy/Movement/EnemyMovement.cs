using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] 
[RequireComponent(typeof(EnemyAnimation))] 
public class EnemyMovement : CharacterMovement
{
    private NavMeshAgent _navMesh;
    private EnemyAnimation _animation;
    private EnemyAttackRange _attack;

    protected override void Awake()
    {
        base.Awake();
        _navMesh = GetComponent<NavMeshAgent>();
        _animation = GetComponent<EnemyAnimation>();
        _attack = GetComponentInChildren<EnemyAttackRange>();

        _navMesh.speed = _speed;
    }

    protected override void StartMoving()
    {
        base.StartMoving();
        StartCoroutine(WaitToStartFollowing());
    }

    public override void StopMoving()
    {
        base.StopMoving();

        _navMesh.SetDestination(transform.position);
    }

    protected override void StopMovingToCharactedDied(Character current = null)
    {
        base.StopMovingToCharactedDied(current);
        _navMesh.enabled = false;
    }

    private IEnumerator WaitToStartFollowing()
    {
        while (_attack.IsCanAttack == true)
        {
            yield return new WaitForEndOfFrame(); ;
        }

        StartCoroutine(FollowingToTarget());
        StopCoroutine(WaitToStartFollowing());
    }

    private IEnumerator FollowingToTarget()
    {
        Vector3 lastPlayerPosition = transform.position;
        _animation.ChangeRunAnimationState(IsMoving);

        while (IsMoving == true)
        {
            if (lastPlayerPosition != _target.transform.position)
            {
                lastPlayerPosition = _target.transform.position;
                _navMesh.SetDestination(lastPlayerPosition);
            }
            
            yield return new WaitForEndOfFrame();
        }

        _animation.ChangeRunAnimationState(IsMoving);
        StopCoroutine(FollowingToTarget());
    }
}
