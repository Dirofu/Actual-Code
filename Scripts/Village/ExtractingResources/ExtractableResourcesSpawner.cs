using System.Collections.Generic;
using UnityEngine;

public class ExtractableResourcesSpawner : MonoBehaviour
{
    [SerializeField] private ResourcesExtractor _resourcesExtractor;
    [SerializeField] private List<ResourceToSpawn> _resourcesToSpawn;
    [SerializeField] private List<Transform> _spawnPoints;

    private void Awake()
    {
        foreach (var resource in _resourcesToSpawn)
        {
            for (int i = 0; i < resource.Amount; i++)
                Spawn(resource.Template);
        }
    }

    private void Spawn(ExtractionableResource resourceTemplate)
    {
        int spawnPointIndex = Random.Range(0, _spawnPoints.Count - 1);
        var resource = Instantiate(resourceTemplate, _spawnPoints[spawnPointIndex].position, Quaternion.identity);
        _spawnPoints.RemoveAt(spawnPointIndex);
        _resourcesExtractor.AddResource(resource);
        resource.Init();
    }

    [System.Serializable]
    private class ResourceToSpawn
    {
        public ExtractionableResource Template;
        public int Amount;
    }
}
