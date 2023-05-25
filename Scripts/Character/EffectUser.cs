using System;
using UnityEngine;

public class EffectUser : MonoBehaviour
{
    [SerializeField] private ParticleSystem _attackZone;
    [SerializeField] private ParticleSystem _death;
    [SerializeField] private ParticleSystem _rebirth;
    [SerializeField] private ParticleSystem _dash;
    
    private AudioSource _deathAudio;
    private AudioSource _dashAudio;

    private void Awake()
    {
        _deathAudio = _death.GetComponent<AudioSource>();

        if (_dash != null)
            _dashAudio = _dash.GetComponent<AudioSource>();
    }

    public void PlayDashEffect()
    {
        PlayEffect(_dash);
        _dashAudio.Play();

    }

    public void StopDashEffect()
    {
        StopEffect(_dash);
    }

    public void PlayDeathEffect()
    {
        PlayEffect(_death);
        _deathAudio.Play();
    }

    public void StopAttackZoneEffect()
    {
        StopEffect(_attackZone);
    }

    public void PlayRebirthEffect()
    {
        PlayEffect(_rebirth);
    }

    public void StopRebirthEffect()
    {
        StopEffect(_rebirth);
    }

    private void PlayEffect(ParticleSystem effect)
    {
        if (effect == null)
            throw new ArgumentNullException(nameof(effect));

        effect.Play();
    }

    private void StopEffect(ParticleSystem effect)
    {
        if (effect == null)
            return;

        effect.Stop();
    }
}
