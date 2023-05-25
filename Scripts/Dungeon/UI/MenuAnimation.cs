using System.Collections;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    private Animator _animator;

    private const string OpenAnimation = "Open";
    private const string CloseAnimation = "Close";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Open();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        _animator.Play(OpenAnimation);
    }

    public void Close()
    {
        if (gameObject.activeSelf)
            StartCoroutine(PlayCloseAnimation());
    }

    private IEnumerator PlayCloseAnimation()
    {
        _animator.Play(CloseAnimation);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
        StopCoroutine(PlayCloseAnimation());
    }
}
