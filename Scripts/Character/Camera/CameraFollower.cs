using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;

    [SerializeField] private Vector2 _minLimit;
    [SerializeField] private Vector2 _maxLimit;

    private void Update()
    {
        Vector3 targetPosition = _target.position + _offset;

        float positionX = GetClampedFloatValue(targetPosition.x, _minLimit.x, _maxLimit.x);
        float positionZ = GetClampedFloatValue(targetPosition.z, _minLimit.y, _maxLimit.y);

        Vector3 nextPosition = new Vector3(positionX, targetPosition.y, positionZ);

        transform.position = Vector3.Lerp(transform.position, nextPosition, _speed * Time.deltaTime);
    }

    private float GetClampedFloatValue(float target, float max, float min) => Mathf.Clamp(target, max, min);
}
