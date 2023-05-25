using UnityEngine;

[RequireComponent(typeof(Character))]
public class EnemyAttackEvent : MonoBehaviour
{               
    private Character _character;
    private EnemyAttackRange _range;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _range = GetComponentInChildren<EnemyAttackRange>();
    }

    public void DamagePlayer()
    {
        _range.Player.TakeDamage(_character.Damage);
    }
}
