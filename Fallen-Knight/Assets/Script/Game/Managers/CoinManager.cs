using System;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinText;
    [HideInInspector] public int _coin;

    private void Start()
    {
        _coin = 200;
        _coinText.text = _coin.ToString();
    }

    public void AddCoin(int amount)
    {
        _coin += amount;
        UpdateCoinNumber();
    }
    
    public void RemoveCoin(int amount)
    {
        _coin -= amount;
        UpdateCoinNumber();
    }
    
    private void UpdateCoinNumber()
    {
        if (_coinText != null)
        {
            _coinText.text = _coin.ToString();
        }
    }
}
