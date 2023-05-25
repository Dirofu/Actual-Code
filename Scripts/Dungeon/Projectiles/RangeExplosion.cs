using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeExplosion : MonoBehaviour
{
    [SerializeField] private SphereCollider _collider;

    private PlayerSkillsData _skills;
    private List<Character> _characters = new List<Character>();

    private float _timeToDestroy = 5f;

    private void Start()
    {
        _collider.radius = _skills.ExploseArrowRange;
        StartCoroutine(WaitToAttack());
    }

    private void Update()
    {
        if (_timeToDestroy <= 0)
            Destroy(gameObject);

        _timeToDestroy -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            if (_skills != null &&
                _skills.ExploseArrow == true &&
                _characters.Contains(character) == false &&
                character.IsPlayer == false)
            {
                _characters.Add(character);
            }
        }
    }

    public void Initialize(PlayerSkillsData skills)
    {
        _skills = skills;
    }

    private void DamageAllCharacters()
    {
        foreach (var item in _characters)
            item.TakeDamage(_skills.ExploseArrowDamage);
    }

    private IEnumerator WaitToAttack()
    {
        yield return new WaitForSeconds(.1f);
        _collider.enabled = false;
        DamageAllCharacters();
        StopCoroutine(WaitToAttack());
    }
}
