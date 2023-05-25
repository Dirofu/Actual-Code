using System;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public event Action<InteractableObject> TriggerStay;
    public event Action<InteractableObject> TriggerExit;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
            TriggerStay?.Invoke(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
            TriggerExit?.Invoke(this);
    }
}
