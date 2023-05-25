using Agava.YandexGames;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private AdActivator _ad;
    private AsyncOperation _level;

    private const string CurrentLevelName = "CurrentLevelName";
    private const string StartLoading = "StartLoading";
    private const string CurrentLevelID = "CurrentLevelID";

    public int SceneCount => SceneManager.sceneCountInBuildSettings - 1;
    public int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

    private void Awake()
    {
        _ad = GetComponent<AdActivator>();
        PlayerPrefs.SetString(CurrentLevelName, SceneManager.GetActiveScene().name);
    }

    private IEnumerator AsyncLoadLevel()
    {
        _level.allowSceneActivation = false;

        while (_level.isDone == false)
        {
            if (_level.progress >= 0.9f && _ad.AdIsEnable == false)
                _level.allowSceneActivation = true;

            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(AsyncLoadLevel());
    }

    private IEnumerator TryStartLoading()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        while (_ad != null && _ad.AdIsEnable == true)
            yield return new WaitForEndOfFrame();
#endif
        yield return new WaitForEndOfFrame();
        StartCoroutine(AsyncLoadLevel());
        StopCoroutine(TryStartLoading());
    }

    private void OpenLoadScreen()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (_ad != null)
            _ad.ShowInterstitialAd();
#endif
        StartCoroutine(TryStartLoading());
    }

    public void LoadLevel(int levelID)
    {
        _level = SceneManager.LoadSceneAsync(levelID);
        OpenLoadScreen();
    }

    public void LoadLevel(string levelName)
    {
        _level = SceneManager.LoadSceneAsync(levelName);
        OpenLoadScreen();
    }

    public void LoadNextLevel()
    { 
        int id = SceneCount > CurrentSceneIndex ? CurrentSceneIndex + 1 : 0;
        int currentLevel = PlayerPrefs.GetInt(CurrentLevelID) + 1;
        PlayerPrefs.SetInt(CurrentLevelID, currentLevel);
        LoadLevel(id);
    }

    public void RestartLevel() => LoadLevel(CurrentSceneIndex);

    public void BackToMenu()
    {
        LoadLevel(1);
    }
}