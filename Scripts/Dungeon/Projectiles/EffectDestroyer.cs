using System.Collections;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    [SerializeField] private float _timeBeforeDestroy;

    private void Start()
    {
        StartCoroutine(DestroyByTime());
    }

    private IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(_timeBeforeDestroy);
        Destroy(gameObject);
        StopCoroutine(DestroyByTime());
    }
}
