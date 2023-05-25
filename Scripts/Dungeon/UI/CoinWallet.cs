using System;
using System.Collections;
using UnityEngine;

public class CoinWallet : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _coinText;
    [SerializeField] private float _speedToIncrease;

    private SaveSystem _saveSystem;

    private int _coins;
    private float _coinsOnScreen;

    public int Coins => _coins;

    public void SetSave(SaveSystem save) => _saveSystem = save;
    public void SaveCoins() => _saveSystem.SaveCoinsFromWalletOnEndDungeon();

    public void AddCoins(int coins)
    {
        if (coins < 0)
            throw new ArgumentOutOfRangeException(nameof(coins));

        _coins += coins;
        StartCoroutine(ShowAddCoinsWithSmoothlyEffect());
    }

    private void ShowCoinsOnScreen()
    {
        _coinText.text = ((int)_coinsOnScreen).ToString();
    }

    private IEnumerator ShowAddCoinsWithSmoothlyEffect()
    {
        while (_coins != _coinsOnScreen)
        {
            _coinsOnScreen = Mathf.MoveTowards(_coinsOnScreen, _coins, _speedToIncrease);

            ShowCoinsOnScreen();
            yield return new WaitForEndOfFrame();
        }
    }
}
