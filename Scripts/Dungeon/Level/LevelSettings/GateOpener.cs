using UnityEngine;

public class GateOpener : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string _openGate = "OpenGate";

    public void Open() => _animator.SetTrigger(_openGate);
}
