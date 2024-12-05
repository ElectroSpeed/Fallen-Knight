using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private TMP_Text _coinText;
    private int _coin;
    
    [SerializeField] private TMP_Text _castleHealthText;
    [SerializeField] private int _castleHealth;
    
    public List<GameObject> _mapWaypoints = new List<GameObject>();
    
    public List<Vector3> _mapCellTrajectory = new List<Vector3>();
    [HideInInspector] public List<Vector3> _mapCellTransform = new List<Vector3>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
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
    
    public void RemoveCastleHealth(int amount)
    {
        _castleHealth -= amount;
        UpdateCastleHealthNumber();
        if (_castleHealth == 0)
        {
            Debug.Log("Game Over");
        }
    }
    
    private void UpdateCastleHealthNumber()
    {
        if (_castleHealthText != null)
        {
            _castleHealthText.text = _castleHealth.ToString();
        }
    }

    public void BuyTower(TowerPurchase towerPurchase)
    {
        if (towerPurchase._moneyToSpend > _coin)
        {
           GameObject tower = Instantiate(towerPurchase._towerToPurchasse);
           
        }
    }
}
