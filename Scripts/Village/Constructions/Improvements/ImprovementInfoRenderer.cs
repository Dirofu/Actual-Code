using NoCodingEasyLocalization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImprovementInfoRenderer : MonoBehaviour, ILocalizer
{
    [SerializeField] private TMP_Text _lableText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private RectTransform _costContainer;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TMP_Text _buyButtonText;
    [SerializeField] private Button _showRequiredLevelButton;
    [SerializeField] private ResourcesStorage _resourcesStorage;
    [SerializeField] private ResourceRenderer _resourceRendererTemplate;

    [SerializeField] private LocalizeField _requiredLevelTextLoc;

    private Color _defaultDescriptionColor;
    private List<ResourceRenderer> _resourceRenderers = new();
    private ImprovementButton _currentImprovement;

    private void Awake()
    {
        _defaultDescriptionColor = _descriptionText.color;
        _buyButton.onClick.AddListener(OnBuyButtonClick);
        _showRequiredLevelButton.onClick.AddListener(OnRequiredLevelInfoButtonClick);

        foreach (var resource in _resourcesStorage.Resources)
        {
            var renderer = Instantiate(_resourceRendererTemplate, _costContainer);
            _resourceRenderers.Add(renderer);
            renderer.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _buyButton.onClick.RemoveListener(OnBuyButtonClick);
        _showRequiredLevelButton.onClick.RemoveListener(OnRequiredLevelInfoButtonClick);
    }

    public void RenderImprovement(ImprovementButton improvement)
    {
        if (_currentImprovement != null)
            _currentImprovement.SetHighlighted(false);

        _currentImprovement = improvement;
        improvement.SetHighlighted(true);
        _lableText.text = improvement.Info.Lable;
        _descriptionText.text = improvement.Info.Description;
        _descriptionText.color = _defaultDescriptionColor;
        RenderRequiredResources(improvement.Info.Cost);
        _buyButton.gameObject.SetActive(improvement.IsLevelSatisfied == false || improvement.CanAcquire);
        _buyButton.interactable = improvement.CanAcquire;
        _showRequiredLevelButton.gameObject.SetActive(improvement.IsLevelSatisfied == false);
        gameObject.SetActive(true);
    }

    public void PlayNotEnoughResourcesAnimation(List<ResourceType> resourceTypes)
    {
        foreach (var resourceRenderer in _resourceRenderers)
            foreach (var resourceType in resourceTypes)
                if (resourceRenderer.ResourceType == resourceType)
                    resourceRenderer.PlayRedAnimation();
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }

    private void RenderRequiredResources(ImprovementCost cost)
    {
        if (cost.RequiredResources.Count < _resourceRenderers.Count)
            ClearRequiredResources();

        var requiredResources = cost.RequiredResources;

        for (int i = 0; i < requiredResources.Count; i++)
        {
            _resourceRenderers[i].Render(requiredResources[i].ResourceType, requiredResources[i].Amount);
            _resourceRenderers[i].gameObject.SetActive(true);
        }
    }

    private void ClearRequiredResources()
    {
        foreach (var renderer in _resourceRenderers)
            renderer.gameObject.SetActive(false);
    }

    private void OnBuyButtonClick()
    {
        bool result = _currentImprovement.TryImprove();

        if (result)
            RenderImprovement(_currentImprovement);
    }

    private void OnRequiredLevelInfoButtonClick()
    {
        if (_descriptionText.text.Equals(_currentImprovement.Info.Description))
        {
            ReloadLocalizationField();
        }
        else
        {
            _descriptionText.text = _currentImprovement.Info.Description;
            _descriptionText.color = _defaultDescriptionColor;
        }
    }

    public void ReloadLocalizationField()
    {
        _descriptionText.text = _requiredLevelTextLoc.GetLocalizeText(this) +
                +_currentImprovement.RequiredConstructionLevel;
        _descriptionText.color = Color.red;
    }
}
