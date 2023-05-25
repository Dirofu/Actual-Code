using Agava.YandexGames;
using System.Collections;
using UnityEngine;

public class InitializationSDK : MonoBehaviour
{
    [SerializeField] private SaveSystem _save;

    private LevelLoader _loader;

    private const string TutorialCompleteStatus = "TutorialCompleteStatus";

    private void Awake()
    {
#if !UNITY_EDITOR
        YandexGamesSdk.CallbackLogging = true; 
#endif
        _loader = GetComponent<LevelLoader>();
    }

    private IEnumerator Start()
    {
#if !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize();

        if (PlayerAccount.IsAuthorized == true)
            PlayerAccount.RequestPersonalProfileDataPermission();
#endif
        yield return new WaitForEndOfFrame();
        DontDestroyOnLoad(Instantiate(_save));
        TryLoadTutorialScene();
    }

    private void TryLoadTutorialScene()
    {
        int completeStatus = PlayerPrefs.GetInt(TutorialCompleteStatus);

        if (completeStatus != 1)
            _loader.LoadNextLevel();
        else
            _loader.LoadLevel(1);
    }
}