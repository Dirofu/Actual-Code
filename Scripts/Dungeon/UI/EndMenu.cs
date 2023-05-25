using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EndMenu : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _coinsText;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.ignoreListenerVolume = true;
        _source.ignoreListenerPause = true;
    }

    private void OnEnable()
    {
        _source.Play();
    }

    public void SetCoins(int coins)
    {
        _coinsText.text = coins.ToString();
    }
}
