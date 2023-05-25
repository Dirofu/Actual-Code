using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CavalryAttack))]
public class CavalryMovement : CharacterMovement
{
    #region Fields
    private Rigidbody _rigidbody;
    private CavalryAttack _attack;
    private Vector3 _direction;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
        _attack = GetComponent<CavalryAttack>();
    }

    private void Update()
    {
        if (_rigidbody.velocity != _direction && IsMoving == true)
            _rigidbody.velocity = _direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Character character) && character.IsPlayer == true)
        {
            _attack.Attack(character);
        }
        else if (collision.gameObject.TryGetComponent(out ProjectileArrow arrow) == false)
        {
            _direction = Vector3.Reflect(_direction, collision.contacts[0].normal);

            Vector3 lookDirection = new Vector3(transform.position.x + _direction.x, transform.position.y, transform.position.z + _direction.z);
            transform.LookAt(lookDirection);
        }
    }

    protected override void StartMoving()
    {
        base.StartMoving();

        transform.LookAt(_target.transform);
        _direction = transform.forward * _speed;
    }

    public override void StopMoving()
    {
        base.StopMoving();
        _rigidbody.velocity = Vector3.zero;
    }
}
