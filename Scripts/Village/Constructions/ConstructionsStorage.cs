using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionsStorage : MonoBehaviour
{
    [SerializeField] private List<Construction> _constructions;
    [SerializeField] private PopUpButton _constructionButton;
    [SerializeField] private ConstructionMenu _constructionMenu;

    private Construction _currentConstruction;

    private void Awake()
    {
        _constructionButton.Clicked += OnConstructionButtonClick;

        foreach (var construction in _constructions)
        {
            construction.TriggerStay += OnConstructionTriggerStay;
            construction.TriggerExit += OnConstructionTriggerExit;
        }
    }

    private void OnDestroy()
    {
        _constructionButton.Clicked -= OnConstructionButtonClick;

        foreach (var construction in _constructions)
        {
            construction.TriggerStay -= OnConstructionTriggerStay;
            construction.TriggerExit -= OnConstructionTriggerExit;
        }
    }

    private void OnConstructionButtonClick(PopUpButton.ButtonType buttonType)
    {
        if (buttonType == PopUpButton.ButtonType.Building)
        {
            _constructionMenu.Open(_currentConstruction);
            _constructionButton.Hide(PopUpButton.ButtonType.Building);
        }
    }

    private void OnConstructionTriggerStay(InteractableObject interactableObject)
    {
        if (_constructionMenu.gameObject.activeSelf)
        {
            _constructionButton.Hide(PopUpButton.ButtonType.Building);
        }
        else
        {
            if (_currentConstruction != null && _currentConstruction.IsBuilding)
            {
                _constructionButton.Hide(PopUpButton.ButtonType.Building);
            }
            else
            {
                _constructionButton.Show(PopUpButton.ButtonType.Building);
                _currentConstruction = interactableObject as Construction;

                if (_currentConstruction == null)
                    throw new System.ArgumentException();
            }
        }
    }

    private void OnConstructionTriggerExit(InteractableObject _)
    {
        _constructionButton.Hide(PopUpButton.ButtonType.Building);
        _constructionMenu.Close();
    }
}
