using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettuper : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _slider;

    [Header("Buttons")]
    [SerializeField] private GameObject _musicEnable;
    [SerializeField] private GameObject _musicDisable;
    [SerializeField] private GameObject _effectsEnable;
    [SerializeField] private GameObject _effectsDisable;

    private SaveSystem _saveSystem;

    private const string MasterVolume = "Master";
    private const string MusicVolume = "Music";
    private const string EffectsVolume = "Effects";

    private const float _maxVolume = 5f;
    private const float _minVolume = -80f;

    public bool MusicEnable { get; private set; }
    public bool EffectsEnable { get; private set; }
    public float Volume { get; private set; }

    private void Awake()
    {
        _slider.onValueChanged.AddListener(OnVolumeChanged);
    }   

    public void SetSave(SaveSystem save)
    {
        _saveSystem = save;
        LoadSettings(_saveSystem.SaveData);
    }

    public void SaveSettings() => _saveSystem.Save();

    public void OnVolumeChanged(float value)
    {
        Volume = value;
        _mixer.SetFloat(MasterVolume, Volume);
    }

    public void SetMusicStatus(bool state)
    {
        float value = state ? _maxVolume : _minVolume;
        _mixer.SetFloat(MusicVolume, value);
        MusicEnable = state;
        _musicEnable.SetActive(state);
        _musicDisable.SetActive(!state);
    }

    public void SetEffectsStatus(bool state)
    {
        float value = state ? _maxVolume : _minVolume;
        _mixer.SetFloat(EffectsVolume, value);
        EffectsEnable = state;
        _effectsEnable.SetActive(state);
        _effectsDisable.SetActive(!state);
    }

    public void LoadSettings(SaveData data)
    {
        OnVolumeChanged(data.VolumeValue);
        SetMusicStatus(data.MusicEnable);
        SetEffectsStatus(data.EffectsEnable);
        _slider.value = data.VolumeValue;
    }
}
