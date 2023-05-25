using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuAnimation))]
public class ConstructionMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _lableText;
    [SerializeField] private Button _desctiptionButton;
    [SerializeField] private RectTransform _buttonsContainer;
    [SerializeField] private ResourcesStorage _resourcesStorage;
    [SerializeField] private ImprovementInfoRenderer _improvementInfoRenderer;

    private MenuAnimation _animation;

    private Construction _construction;
    private List<ImprovementButton> _improvementButtons = new();
    private bool _isDesctiprionActive;

    private void Awake()
    {
        _animation = GetComponent<MenuAnimation>();
        _desctiptionButton.onClick.AddListener(OnInfoButtonClick);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _desctiptionButton.onClick.RemoveListener(OnInfoButtonClick);
    }

    private void OnEnable()
    {
        _improvementInfoRenderer.Clear();
    }

    public void Open(Construction construction)
    {
        _construction = construction;
        construction.UpgradeStarted += OnConstructionUpgraded;
        RenderMenu(construction);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        if (gameObject.activeSelf)
        {
            _construction.UpgradeStarted -= OnConstructionUpgraded;
            _animation.Close();
        }
    }

    private void RenderMenu(Construction construction)
    {
        _isDesctiprionActive = false;
        _lableText.text = construction.CurrentLevel.Lable;
        ClearButtons();

        foreach (var buttonTemplate in construction.Info.ImprovementButtons)
        {
            var improvementButton = Instantiate(buttonTemplate, _buttonsContainer);
            improvementButton.Init(construction, _resourcesStorage, _improvementInfoRenderer);
            _improvementButtons.Add(improvementButton);
        }
    }

    private void OnConstructionUpgraded(Construction _)
    {
        Close();
    }

    private void ClearButtons()
    {
        while (_improvementButtons.Count > 0)
        {
            Destroy(_improvementButtons[0].gameObject);
            _improvementButtons.RemoveAt(0);
        }
    }

    private void OnInfoButtonClick()
    {
        if (_isDesctiprionActive)
            _lableText.text = _construction.CurrentLevel.Lable;
        else
            _lableText.text = _construction.CurrentLevel.Description;

        _isDesctiprionActive = _isDesctiprionActive == false;
    }
}