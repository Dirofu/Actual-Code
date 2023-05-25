using UnityEngine;

public class SoundFocusSetter : MonoBehaviour
{
    private void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    private void Silence(bool silence)
    {
        AudioListener.pause = silence;
        AudioListener.volume = silence ? 0 : 1;

        float scale = silence ? 0f : 1f;
    }
}