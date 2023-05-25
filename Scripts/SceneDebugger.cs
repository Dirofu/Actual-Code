using UnityEngine;

public class SceneDebugger : MonoBehaviour
{
    [SerializeField] private SaveSystem _savePrefab;

    private SaveSystem _save;

    private void Awake()
    {
        if (_save == null)
            DontDestroyOnLoad(_save = Instantiate(_savePrefab));
    }
}
