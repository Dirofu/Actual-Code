using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private List<Weapon> _projectiles;

    public Weapon GetFirstDisabledProjectileFromPool()
    {
        foreach (var item in _projectiles)
        {
            if (item.gameObject.activeInHierarchy == false)
                return item;
        }
        return null;
    }

    public void DisableAllProjectiles()
    {
        foreach (var item in _projectiles)
            item.gameObject.SetActive(false);
    }

    public void InitializePool(Character character, int size, Weapon projectilePrefab)
    {
        for (int i = 0; i < size; i++)
        {
            Weapon projectile = Instantiate(projectilePrefab, gameObject.transform);
            projectile.Initialize(character);
            _projectiles.Add(projectile);
        }
    }
}
