using UnityEngine;

public class ResourcesPanel : MonoBehaviour
{
    [SerializeField] private ResourcesStorage _storage;
    [SerializeField] private VillageResourceRenderer _rendererTemplate;
    [SerializeField] private RectTransform _container;

    private void Awake()
    {
        foreach (var resource in _storage.Resources)
        {
            var renderer = Instantiate(_rendererTemplate, _container);
            renderer.Render(resource);
        }
    }
}
