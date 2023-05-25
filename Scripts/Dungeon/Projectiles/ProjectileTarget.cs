using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ProjectileTarget : MonoBehaviour
{
    private ParticleSystem _targetParticle;

    private Vector3 _offset = new Vector3(0, .05f, 0);

    private void Awake()
    {
        _targetParticle = GetComponent<ParticleSystem>();
        DisableParticle();
    }

    public void DisableParticle() => _targetParticle.Stop();

    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = new Vector3(position.x, position.y + _offset.y, position.z);
        _targetParticle.Play();
    }
}
