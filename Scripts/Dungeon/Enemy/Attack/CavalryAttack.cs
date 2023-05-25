using System.Collections;
using UnityEngine;

public class CavalryAttack : MonoBehaviour
{
    [SerializeField] private float _timeBetweenAttack;

    private Character _character;
    private Character _target;
    private float _currentTime;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Character character) && character.IsPlayer == true)
        {
            _target = null;
            StopCoroutine(WaitToDamageTarget());
        }
    }

    public void Attack(Character target)
    {
        _target = target;
        StartCoroutine(WaitToDamageTarget());
    }

    private IEnumerator WaitToDamageTarget()
    {
        while (_target != null && _character.IsDied == false)
        {
            while (_currentTime > 0)
            {
                yield return new WaitForEndOfFrame();
                _currentTime -= Time.deltaTime;
            }

            _target.TakeDamage(_character.Damage);
            _currentTime = _timeBetweenAttack;
        }
        StopCoroutine(WaitToDamageTarget());
    }
}
