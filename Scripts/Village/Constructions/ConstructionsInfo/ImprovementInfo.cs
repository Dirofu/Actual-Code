using UnityEngine;
using System;

[Serializable]
public class ImprovementInfo
{
    [SerializeField] private ImprovementLocalizator _localizator;
    [SerializeField] private ImprovementCost _improvementCost;

    public string Lable => _localizator.Lable;
    public string Description => _localizator.Description;
    public ImprovementCost Cost => _improvementCost;
}
