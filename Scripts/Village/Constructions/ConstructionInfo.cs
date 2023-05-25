using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Construction", menuName = "Village/Construction", order = 6)]
public class ConstructionInfo : ScriptableObject
{
    [SerializeField] private List<UpgradeImprovementInfo> _upgradeLevels;
    [SerializeField] private List<ImprovementButton> _improvementButtons;

    public IReadOnlyList<UpgradeImprovementInfo> Levels => _upgradeLevels;
    public IReadOnlyList<ImprovementButton> ImprovementButtons => _improvementButtons;
}
