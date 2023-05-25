using Agava.YandexGames;
using System.Collections;
using UnityEngine;

public class PlayerAuthorization : MonoBehaviour
{
    [SerializeField] private GameObject _isAuthorize;
    [SerializeField] private GameObject _isNotAuthorize;

    private void Start()
    {
        CheckAuthorize();
    }

    public void OnGetProfileDataButtonClick()
    {
        PlayerAccount.GetProfileData((result) =>
        {
            string name = result.publicName;
            if (string.IsNullOrEmpty(name))
                result.publicName = $"Warrior-{result.uniqueID}";
        });
    }

    public void AuthorizeButton()
    {
        StartCoroutine(AuthorizeProcess());
    }

    private void CheckAuthorize()
    {
#if UNITY_WEBGL
        if (PlayerAccount.IsAuthorized == true)
        {
            _isAuthorize.SetActive(true);
            _isNotAuthorize.SetActive(false);
        }
#endif
    }

    private IEnumerator AuthorizeProcess()
    {
        PlayerAccount.Authorize(onSuccessCallback: CheckAuthorize);

        while (PlayerAccount.IsAuthorized == false)
        {
            yield return new WaitForEndOfFrame();
        }

        CheckAuthorize();
        OnRequestPersonalProfileDataPermissionButtonClick();
        StopCoroutine(AuthorizeProcess());
    }

    private void OnRequestPersonalProfileDataPermissionButtonClick()
    {
        PlayerAccount.RequestPersonalProfileDataPermission();
    }
}
