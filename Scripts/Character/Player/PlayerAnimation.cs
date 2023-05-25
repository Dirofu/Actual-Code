using UnityEngine;

[RequireComponent(typeof(PlayerSkillsData))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private PlayerSkillsData _skills;
    private PlayerInput _input;
    private Animator _animator;

    private void Awake()
    {
        _skills = GetComponent<PlayerSkillsData>();
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetAttackSpeed(_skills.AttackSpeedLevel);
    }

    private void Update()
    {
        ChangeRunAnimationState();
    }

    public void ChangeAttackAnimationState(bool state) => _animator.SetBool(Constantes.IsAttack, state);
    private void ChangeRunAnimationState()
    {
        if (_input.Direction.magnitude != 0)
            _animator.SetBool(Constantes.IsRun, true);
        else
            _animator.SetBool(Constantes.IsRun, false);

    }

    private void SetAttackSpeed(int speed) => _animator.SetFloat(Constantes.AttackSpeedLevel, speed);
}
