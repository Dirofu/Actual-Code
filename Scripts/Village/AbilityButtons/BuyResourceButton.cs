using UnityEngine;

public class BuyResourceButton : ImprovementButton
{
    [SerializeField] private int _amount;
    [SerializeField] private ImprovementInfo _info;
    public override ImprovementInfo Info => _info;

    protected override void Improve()
    {
        var resourceBuilding = Construction as ResourceBuilding;

        if (resourceBuilding == null)
            throw new System.InvalidOperationException();

        resourceBuilding.Extract(_amount);
    }
}
