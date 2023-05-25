using UnityEngine;

public class CharacterAudioSystem : MonoBehaviour
{
    [SerializeField] private AudioClip[] _attackSound;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponentInChildren<AudioSource>();
    }

    public void PlayRandomAttackSound() => _source.PlayOneShot(_attackSound[Random.Range(0, _attackSound.Length)]);
}