using System.Collections.Generic;
using UnityEngine;

public class EnemySettuper : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private CharacterMovement _movement;
    [SerializeField] private List<Outfit> _levelOutfits;

    private int _level = 0;

    public Character Character => _character;

    public void SetMovementTarget(Character target)
    {
        if (_movement != null)
            _movement.SetTargetMoving(target);
        else
            Debug.LogError("Movement is not set");
    }

    public void SetEnemyData(int enemyLevel)
    {
        SetLevelOutfitState(false);
        _level = enemyLevel;
        SetLevelOutfitState(true);
    }

    private void SetLevelOutfitState(bool state)
    {
        Outfit outfit = _levelOutfits[_level];
        outfit.SetOutfitState(state);
        _character.SetUpCharacterData(outfit.Health, outfit.Damage, _level + 1);
    }
}

[System.Serializable]
public class Outfit
{
    [SerializeField] protected GameObject Head;
    [SerializeField] protected GameObject Torse;
    [SerializeField] protected GameObject Weapon;
    [SerializeField] protected GameObject Horse;
    [SerializeField] private int _health;
    [SerializeField] private int _damage;

    public int Health => _health;
    public int Damage => _damage;

    public void SetOutfitState(bool state)
    {
        Head.SetActive(state);
        Torse.SetActive(state);
        Weapon.SetActive(state);
        
        if (Horse != null)
            Horse.SetActive(state);
    }
}