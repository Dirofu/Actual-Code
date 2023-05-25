using Agava.YandexGames;
using UnityEngine;

public class AdActivator : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;

    private CoinWallet _wallet;
    private GamePauseSettuper _pause;
    private MenuOpener _menu;
    private Character _playerCharacter;

    public bool AdIsEnable { get; private set; } = false;

    private void Awake()
    {
        _pause = GetComponent<GamePauseSettuper>();
        _menu = GetComponentInChildren<MenuOpener>();
        _wallet = GetComponentInChildren<CoinWallet>();
    }

    private void OnOpenAd()
    {
        AdIsEnable = true;
        _pause.Pause();
    }

    private void OnCloseAd(bool wasView)
    {
        AdIsEnable = false;
        _pause.UnPause();
    }

    private void OnRewardedCloseAd()
    {
        AdIsEnable = false;
        _pause.UnPause();
        _menu.CloseLooseMenu();
    }

    public void ShowInterstitialAd()
    {
#if !UNITY_EDITOR
        InterstitialAd.Show(onOpenCallback: OnOpenAd, onCloseCallback: OnCloseAd);
#endif
    }

    public void ShowVideoAdToContinueGame()
    {
#if !UNITY_EDITOR
        VideoAd.Show(onOpenCallback: OnOpenAd, onRewardedCallback: OnRewardedContinueGame, onCloseCallback: OnRewardedCloseAd);
#else
        OnRewardedContinueGame();
#endif
    }

    public void ShowVideoAdToGetX2Gold()
    {
#if !UNITY_EDITOR
        VideoAd.Show(onOpenCallback: OnOpenAd, onRewardedCallback: OnRewardedGetX2Gold, onCloseCallback: OnRewardedCloseAd);
#else
        OnRewardedGetX2Gold();
#endif
    }

    private void OnRewardedGetX2Gold()
    {
        _wallet.AddCoins(_wallet.Coins);
        _menu.UpdateRewardCoinsOnEndMenu();
    }

    private void OnRewardedContinueGame()
    {
        if (_playerCharacter == null)
            _playerCharacter = _playerMovement.GetComponent<Character>();

        _playerMovement.StartMoving();
        _playerCharacter.RebirthCharacter();
    }
}