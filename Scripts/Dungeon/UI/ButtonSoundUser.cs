using UnityEngine;

public class ButtonSoundUser : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    public void ClickButton()
    {
        _source.Play();
    }
}