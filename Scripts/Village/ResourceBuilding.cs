using UnityEngine;

public class ResourceBuilding : Construction
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private int _growth;
    [SerializeField] private int _improvedGrowth;
    [SerializeField] private ResourcesStorage _storage;
    [SerializeField] private ExtractingAmountAnimator _animator;

    protected override void Awake()
    {
        base.Awake();

        Upgraded += ExtractResources;

        if (IsBuilt)
            ExtractResources(this);
    }

    private void OnDestroy()
    {
        Upgraded -= ExtractResources;
    }

    public void Extract(int amount)
    {
        _storage.Add(_resourceType, amount);
        _animator.PlayExtract(_resourceType, amount);
    }

    private void ExtractResources(Construction _)
    {
        if (CurrentLevelIndex > 1)
            Extract(_improvedGrowth);
        else
            Extract(_growth);
    }
}
