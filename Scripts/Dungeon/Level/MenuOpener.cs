using System.Collections;
using UnityEngine;

public class MenuOpener : MonoBehaviour
{
    [SerializeField] private GamePauseSettuper _pauseSettuper;
    [SerializeField] private Character _player;
    [SerializeField] private CoinWallet _wallet;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private EndMenu _finishMenu;

    [Header("Loose Menu Settings")]
    [SerializeField] private EndMenu _looseMenu;
    [SerializeField] private float _timeBeforeShowLooseMenu;

    public bool PauseStatus => _pauseMenu.activeInHierarchy;

    private void OnEnable()
    {
        _player.Died += OnDied;
    }

    private void OnDisable()
    {
        _player.Died -= OnDied;
    }

    public void UpdateRewardCoinsOnEndMenu()
    {
        _finishMenu.SetCoins(_wallet.Coins);
        _looseMenu.SetCoins(_wallet.Coins);
    }

    public void OpenFinishMenu()
    {
        OpenMenu(_finishMenu.gameObject);
        _finishMenu.SetCoins(_wallet.Coins);
    }
    public void OpenLooseMenu()
    {
        OpenMenu(_looseMenu.gameObject);
        _looseMenu.SetCoins(_wallet.Coins);
    }

    public void OpenPauseMenu()
    {
        if (_finishMenu.gameObject.activeInHierarchy == false && 
            _looseMenu.gameObject.activeInHierarchy == false)
            OpenMenu(_pauseMenu);
    }

    public void CloseFinishMenu() => CloseMenu(_finishMenu.gameObject);
    public void CloseLooseMenu() => CloseMenu(_looseMenu.gameObject);
    public void ClosePauseMenu() => CloseMenu(_pauseMenu);

    private void OnDied(Character player)
    {
        StartCoroutine(WaitToShowLooseMenu());
    }

    private void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
        _pauseSettuper.Pause();
    }

    private void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
        _pauseSettuper.UnPause();
    }

    private IEnumerator WaitToShowLooseMenu()
    {
        yield return new WaitForSeconds(_timeBeforeShowLooseMenu);
        OpenLooseMenu();
        StopCoroutine(WaitToShowLooseMenu());
    }
}
