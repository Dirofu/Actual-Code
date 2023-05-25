using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ChangeAttackAnimationState(bool state) => _animator.SetBool(Constantes.IsAttack, state);
    public void ChangeRunAnimationState(bool state) => _animator.SetBool(Constantes.IsRun, state);
}