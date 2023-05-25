using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerSkillsData : MonoBehaviour
{
    #region Fields
    [Range(1, 3)]
    [SerializeField] private int _healthLevel;
    [Range(1, 3)]
    [SerializeField] private int _damageLevel;
    [Range(1, 3)]
    [SerializeField] private int _moveSpeed;
    [Range(0, 3)]
    [SerializeField] private int _attackRangeLevel;
    [Range(1, 3)]
    [SerializeField] private int _attackSpeed;
    [Range(0, 3)]
    [SerializeField] private int _rebounceProjectileCount;
    [SerializeField] private bool _doubleAttack;
    [SerializeField] private bool _throughArrow;
    [SerializeField] private bool _exploseArrow;
    [Range(1, 3)]
    [SerializeField] private int _exploseArrowDamageLevel;
    [Range(1, 3)]
    [SerializeField] private int _exploseArrowRangeLevel;

    private SaveSystem _saveSystem;

    private int[] _health = new int[3] { 700, 1500, 2000 };
    private int[] _damage = new int[3] { 80, 100, 120 };
    private int[] _exploseArrowDamage = new int[3] { 50, 60, 70 };
    #endregion

    #region Constantes
    private const string _attackSpeedAnimation = "AttackSpeed";
    #endregion

    #region Properties 
    public int HealthLevel => _healthLevel;
    public int Health => _health[_healthLevel - 1];
    public int DamageLevel => _damageLevel;
    public int Damage => _damage[_damageLevel - 1];
    public int MoveSpeedLevel => _moveSpeed;
    public int AttackRangeLevel => _attackRangeLevel;
    public int RebounceProjectileCount => _rebounceProjectileCount;
    public int AttackSpeedLevel => _attackSpeed;
    public bool DoubleAttackActive => _doubleAttack;
    public bool ThroughArrow => _throughArrow;
    public bool ExploseArrow => _exploseArrow;
    public int ExploseArrowDamage => _exploseArrowDamage[_exploseArrowDamageLevel - 1];
    public int ExploseArrowDamageLevel => _exploseArrowDamageLevel;
    public int ExploseArrowRange => _exploseArrowRangeLevel;
    public int MaxSkillLevel { get; private set; } = 3;
    #endregion

    public void SetSave(SaveSystem save)
    {
        _saveSystem = save;
        SetSkills(_saveSystem.SaveData);
    }

    public void IncreaseSkillLevelOfHealth() => IncreaseSkill(ref _healthLevel);
    public void IncreaseSkillLevelOfDamage() => IncreaseSkill(ref _damageLevel);
    public void IncreaseSkillLevelOfMoveSpeed() => IncreaseSkill(ref _moveSpeed);
    public void IncreaseSkillLevelOfAttackRange() => IncreaseSkill(ref _attackRangeLevel);
    public void IncreaseSkillLevelOfAttackSpeed() => IncreaseSkill(ref _attackSpeed);
    public void IncreaseSkillLevelOfRebounceCount() => IncreaseSkill(ref _rebounceProjectileCount);
    public void IncreaseSkillLevelOfDoubleAttack() => IncreaseSkill(ref _doubleAttack);
    public void IncreaseSkillLevelOfThroughArrow() => IncreaseSkill(ref _throughArrow);
    public void IncreaseSkillLevelOfExploseArrow() => IncreaseSkill(ref _exploseArrow);
    public void IncreaseSkillLevelOfExploseArrowDamage() => IncreaseSkill(ref _exploseArrowDamageLevel);
    public void IncreaseSkillLevelOfExploseArrowRange() => IncreaseSkill(ref _exploseArrowRangeLevel);

    public void ResetSkills()
    {
        _healthLevel = 1;
        _damageLevel = 1;
        _moveSpeed = 1;
        _attackRangeLevel = 1;
        _attackSpeed = 1;
        _rebounceProjectileCount = 0;
        _doubleAttack = false;
        _throughArrow = false;
        _exploseArrow = false;
        _exploseArrowDamageLevel = 1;
        _exploseArrowRangeLevel = 1;

        _saveSystem.Save();
    }

    private void IncreaseSkill(ref int skillLevel)
    {
        if (skillLevel > MaxSkillLevel)
            return;

        skillLevel++;
        _saveSystem.Save();
    }

    private void IncreaseSkill(ref bool skill)
    {
        if (skill == true)
            return;

        skill = true;
        _saveSystem.Save();
    }

    private void SetSkills(SaveData data)
    {
        _healthLevel = data.HealthLevel;
        _damageLevel = data.DamageLevel;
        _moveSpeed = data.MoveSpeedLevel;
        _attackRangeLevel = data.AttackRangeLevel;
        _attackSpeed = data.AttackSpeedLevel;
        _rebounceProjectileCount = data.RebounceProjectileLevel;
        _doubleAttack = data.DoubleAttack;
        _throughArrow = data.ThroughArrow;
        _exploseArrow = data.ExploseArrow;
        _exploseArrowDamageLevel = data.ExploseArrowDamageLevel;
        _exploseArrowRangeLevel = data.ExploseArrowRangeLevel;
    }
}