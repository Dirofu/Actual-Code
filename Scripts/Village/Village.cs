using UnityEngine;

public class Village : MonoBehaviour
{
    [SerializeField] private ResourcesStorage _resourcesStorage;

    private static Village _instance;

    public static PlayerSkillsData SkillsData { get; private set; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        SkillsData = FindObjectOfType<PlayerSkillsData>();

        foreach (var resource in _resourcesStorage.Resources)
            _resourcesStorage.Add(resource.Type, 100);
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        foreach (var resource in _resourcesStorage.Resources)
            if(resource.Type==resourceType)
                return resource.Amount;

        throw new System.NotImplementedException();
    }
}
