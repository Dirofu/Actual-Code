using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesExtractor : MonoBehaviour
{
    [SerializeField] private PopUpButton _extractingButton;
    [SerializeField] private ResourcesStorage _resourcesStorage;

    private List<ExtractionableResource> _extractionableResources = new();
    private ExtractionableResource _currentResource;

    private void Awake()
    {
        _extractingButton.Clicked += OnButtonClick;
    }

    private void OnDestroy()
    {
        _extractingButton.Clicked -= OnButtonClick;
    }

    public void AddResource(ExtractionableResource extractionableResource)
    {
        _extractionableResources.Add(extractionableResource);
        extractionableResource.TriggerStay += OnResourceTriggerEnter;
        extractionableResource.TriggerExit += OnResourceTriggerExit;
    }

    private void OnResourceTriggerEnter(InteractableObject interactableObject)
    {
        _currentResource = interactableObject as ExtractionableResource;

        if (_currentResource == null)
            throw new ArgumentException();

        _extractingButton.Show(PopUpButton.ButtonType.Extracting);
    }

    private void OnResourceTriggerExit(InteractableObject _)
    {
        _extractingButton.Hide(PopUpButton.ButtonType.Extracting);
    }

    private void OnButtonClick(PopUpButton.ButtonType buttonType)
    {
        if (buttonType == PopUpButton.ButtonType.Extracting)
        {
            _extractingButton.Hide(PopUpButton.ButtonType.Extracting);
            _currentResource.Extract(_resourcesStorage);
        }
    }
}
