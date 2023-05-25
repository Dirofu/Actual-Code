using UnityEngine;

[System.Serializable]
public class UpgradeImprovementInfo : ImprovementInfo
{
    [SerializeField] private GameObject _model;

    public GameObject Model => _model;
}
