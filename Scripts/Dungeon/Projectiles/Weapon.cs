using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    protected Character CurrentCharacter;
    protected bool IsPlayerShot;

    protected int IgnoreColliderLayer = 6;

    public UnityAction Triggered;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != IgnoreColliderLayer)
        {
            if (other.TryGetComponent(out Character character))
                character.TakeDamage(CurrentCharacter.Damage);

            Triggered?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public virtual void Initialize(Character character)
    {
        CurrentCharacter = character;
        IsPlayerShot = CurrentCharacter.IsPlayer;
        gameObject.SetActive(false);
    }
}
