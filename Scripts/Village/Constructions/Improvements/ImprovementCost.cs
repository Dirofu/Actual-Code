using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImprovementCost
{
    [SerializeField] private List<Cost> _requiredResources;

    public IReadOnlyList<Cost> RequiredResources=> _requiredResources;

    [Serializable]
    public class Cost
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private int _amount;

        public ResourceType ResourceType => _resourceType;  
        public int Amount => _amount;
    }
}
