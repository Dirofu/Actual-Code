using UnityEngine;

public class LevelLeaver : MonoBehaviour
{
    private LevelSpawner _spawner;

    private void OnEnable()
    {
        _spawner = FindObjectOfType<LevelSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character) && character.IsPlayer == true)
            _spawner.SpawnNextDungeonLevel();
    }
}
