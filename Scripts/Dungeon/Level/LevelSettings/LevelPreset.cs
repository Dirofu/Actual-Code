using System.Collections.Generic;
using UnityEngine;

public class LevelPreset : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnpoint;
    [SerializeField] private List<EnemySpawnpoint> _spawnpoints;
    [SerializeField] private GateOpener _gate;

    public Transform PlayerSpawnpoint => _playerSpawnpoint;

    public Vector3 GetRandomSpawnpointPosition()
    {
        int id = Random.Range(0, _spawnpoints.Count);
        return _spawnpoints[id].transform.position;
    }

    public void OpenGate() => _gate.Open();
}
