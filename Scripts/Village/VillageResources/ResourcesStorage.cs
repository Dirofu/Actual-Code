using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    [SerializeField] private List<Resource> _resources;

    private static ResourcesStorage _instance;
    private SaveSystem _saveSystem;

    public IReadOnlyList<IReadOnlyResource> Resources => _resources;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void SetSave(SaveSystem saveSystem)
    {
        _saveSystem = saveSystem;
        SetResources(saveSystem);
    }

    public static Sprite GetSprite(ResourceType resourceType)
    {
        return _instance.GetResource(resourceType).Sprite;
    }

    public bool CanGive(ResourceType resourceType, int amount)
    {
        return GetResource(resourceType).CanGive(amount);
    }

    public void Give(ResourceType resourceType, int amount)
    {
        GetResource(resourceType).Give(amount);
    }

    public void Add(ResourceType resourceType, int amount)
    {
        GetResource(resourceType).Add(amount);
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        return GetResource(resourceType).Amount;
    }

    private Resource GetResource(ResourceType resourceType)
    {
        foreach (var resource in _resources)
            if (resource.Type == resourceType)
                return resource;

        throw new NotImplementedException();
    }

    private void SetResources(SaveSystem saves)
    {
        GetResource(ResourceType.Gold).SetAmount(saves.SaveData.GoldCount);
        GetResource(ResourceType.Wood).SetAmount(saves.SaveData.WoodCount);
        GetResource(ResourceType.Stone).SetAmount(saves.SaveData.RockCount);
    }
}