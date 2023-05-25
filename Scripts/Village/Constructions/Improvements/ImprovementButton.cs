using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ImprovementButton : MonoBehaviour
{
    [SerializeField] private Image _highlightedFrame;
    [SerializeField] private int _requiredConstructionLevel = 0;

    private ResourcesStorage _resourcesStorage;
    private ImprovementInfoRenderer _renderer;
    private bool _isInitialized;

    public virtual bool CanAcquire => IsLevelSatisfied;
    public bool IsLevelSatisfied => Construction.CurrentLevelIndex >= _requiredConstructionLevel;
    public int RequiredConstructionLevel => _requiredConstructionLevel;
    public abstract ImprovementInfo Info { get; }
    protected Button Button { get; private set; }
    protected Construction Construction { get; private set; }

    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnButtonClick);
    }

    private void OnEnable()
    {
        SetHighlighted(false);
    }

    private void OnDestroy()
    {
        Button.onClick.RemoveListener(OnButtonClick);
    }

    public void Init(Construction construction, ResourcesStorage resourcesStorage, ImprovementInfoRenderer renderer)
    {
        Construction = construction ?? throw new ArgumentNullException();
        _resourcesStorage = resourcesStorage ?? throw new ArgumentNullException();
        _renderer = renderer ?? throw new ArgumentNullException();
        ChildInit();
        _isInitialized = true;
    }

    public bool TryImprove()
    {
        if (_isInitialized == false)
            throw new InvalidOperationException("Object is not initialized");

        var improvementCost = Info.Cost;
        var notEnoughResourcesTypes = GetNotEnoughResources(improvementCost, _resourcesStorage);

        if (notEnoughResourcesTypes.Count == 0)
        {
            TransferResources(improvementCost);
            Improve();
            return true;
        }
        else
        {
            _renderer.PlayNotEnoughResourcesAnimation(notEnoughResourcesTypes);
            return false;
        }
    }

    public void SetHighlighted(bool value)
    {
        _highlightedFrame.gameObject.SetActive(value);
    }

    protected abstract void Improve();
    protected virtual void ChildInit() { }

    private List<ResourceType> GetNotEnoughResources(ImprovementCost cost, ResourcesStorage storage)
    {
        List<ResourceType> notEnoughResourcesTypes = new();

        foreach (var requiredResource in cost.RequiredResources)
            if (storage.CanGive(requiredResource.ResourceType, requiredResource.Amount) == false)
                notEnoughResourcesTypes.Add(requiredResource.ResourceType);

        return notEnoughResourcesTypes;
    }

    private void TransferResources(ImprovementCost buildCost)
    {
        foreach (var requiredResource in buildCost.RequiredResources)
            _resourcesStorage.Give(requiredResource.ResourceType, requiredResource.Amount);
    }

    private void OnButtonClick()
    {
        _renderer.RenderImprovement(this);
    }
}
