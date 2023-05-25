using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class CharacterMovement : MonoBehaviour
{
    [SerializeField] protected float _speed;

    protected Character _current;
    protected Character _target;

    public bool IsMoving { get; protected set; } = true;

    protected virtual void Awake()
    {
        _current = GetComponent<Character>();
    }

    private void OnEnable()
    {
        _current.Died += StopMovingToCharactedDied;
    }

    private void OnDisable()
    {
        _current.Died -= StopMovingToCharactedDied;
    }

    public virtual void SetTargetMoving(Character character)
    {
        _target = character;
        StartMoving();
    }

    protected virtual void StartMoving()
    {
        if (_current.IsDied == true)
            return;

        IsMoving = true;
    }

    public virtual void StopMoving()
    {
        IsMoving = false;
    }

    protected virtual void StopMovingToCharactedDied(Character current = null)
    {
        StopMoving();
    }
}