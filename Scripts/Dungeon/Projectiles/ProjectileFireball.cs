using System.Collections;
using UnityEngine;

public class ProjectileFireball : Weapon
{
    [SerializeField] private float _speed;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _missleEffect;
    [SerializeField] private ParticleSystem _torchEffect;
    [SerializeField] private ParticleSystem _explosionEffect;

    [SerializeField] private Transform _target;

    private PlayerInput _player;

    private bool _isExplode = false;
    private float _timeBeforeReturnToPool = .1f;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerInput>();
    }

    private void OnEnable()
    {
        SetStandartState();
        SetTargetPosition(_player.transform.position);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != IgnoreColliderLayer)
        {
            if (other.TryGetComponent(out Character character))
            {
                if (character.IsPlayer == false)
                    return;

                character.TakeDamage(CurrentCharacter.Damage);
            }
            
            PlayDestroyAnimation();
        }
    }

    public void SetTargetPosition(Vector3 target)
    {
        StartCoroutine(FlyToTargetPosition(target));
    }

    private void SetStandartState()
    {
        _torchEffect.Play();
        _explosionEffect.Stop();
        _isExplode = false;
    }

    private void PlayDestroyAnimation()
    {
        Triggered?.Invoke();
        _torchEffect.Stop();

        if (_isExplode == false)
            Instantiate(_explosionEffect, transform.position, transform.rotation);

        _isExplode = true;
        StartCoroutine(WaitReturnToPool());
    }

    private IEnumerator FlyToTargetPosition(Vector3 target)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        PlayDestroyAnimation();
        StopCoroutine(WaitReturnToPool());
    }

    private IEnumerator WaitReturnToPool()
    {
        yield return new WaitForSeconds(_timeBeforeReturnToPool);

        gameObject.SetActive(false);
        StopCoroutine(WaitReturnToPool());
    }
}
