using UnityEngine;

[RequireComponent(typeof(SoundSettuper))]
public class GamePauseSettuper : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    private SoundSettuper _sound;

    private void Awake()
    {
        _sound = GetComponent<SoundSettuper>();
        UnPause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void UnPause()
    {
        if (_pauseMenu.activeInHierarchy == false)
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
#if !UNITY_EDITOR
            _sound.SaveSettings();
#endif
        }
    }
}
