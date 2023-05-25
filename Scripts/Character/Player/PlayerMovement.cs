using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EffectUser))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(PlayerSkillsData))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _jumpLength;
    [SerializeField] private float _maxDistanceDashDelta = 10f;

    private Rigidbody _rigidbody;
    private EffectUser _effect;
    private PlayerInput _input;
    private Character _character;
    private PlayerSkillsData _skills;
    private Coroutine _dashMotion;

    private bool _isCanMove = true;
    public bool IsMoving => _input.Direction.magnitude != 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
        _character = GetComponent<Character>();
        _skills = GetComponent<PlayerSkillsData>();
        _effect = GetComponent<EffectUser>();
    }

    private void OnEnable()
    {
        _character.Died += StopMoving;
        _input.OnJump += OnDash;
    }

    private void OnDisable()
    {
        _character.Died -= StopMoving;
        _input.OnJump -= OnDash;
    }

    private void FixedUpdate()
    {
        if (IsMoving == true && _dashMotion == null && _isCanMove == true && _character.IsDied == false)
            Move(_input.Direction);
    }

    public void StartMoving()
    {
        _isCanMove = true;
    }

    public void StopMoving(Character character = null)
    {
        _isCanMove = false;
    }

    public void StopDashing()
    {
        if (_dashMotion != null)
        {
            StopCoroutine(_dashMotion);
            _effect.StopDashEffect();
            _dashMotion = null;
        }
    }

    private void OnDash()
    {
        if (_isCanMove == true && _dashMotion == null && _character.IsDied == false)
            _dashMotion = StartCoroutine(Dash());
    }

    private void Move(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(
            direction.x * (_speed + _skills.MoveSpeedLevel), 
            _rigidbody.velocity.y, 
            direction.y * (_speed + _skills.MoveSpeedLevel));

        _rigidbody.velocity = moveDirection;
        Rotation(moveDirection);
    }

    private void Rotation(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private IEnumerator Dash()
    {
        var waitForEndOfFrame = new WaitForEndOfFrame();
        Vector3 target = _rigidbody.position + transform.forward * _jumpLength;
        float timeDash = .2f;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 1))
            timeDash = 0;

        if (timeDash > 0)
            _effect.PlayDashEffect();

        while (timeDash > 0 && _rigidbody.position != target)
        {
            _rigidbody.position = Vector3.Lerp(_rigidbody.position, target, _maxDistanceDashDelta * Time.deltaTime);
            timeDash -= Time.deltaTime;
            yield return waitForEndOfFrame;
        }

        _effect.StopDashEffect();
        _dashMotion = null;
        StopCoroutine(Dash());
    }
}