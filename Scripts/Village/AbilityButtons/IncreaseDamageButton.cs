using UnityEngine;

public class IncreaseDamageButton : AbilityImprovementButton
{
    [SerializeField] private int _additionalDamage;

    protected override void Improve()
    {
        base.Improve();
        Village.SkillsData.IncreaseSkillLevelOfDamage();
    }
}
