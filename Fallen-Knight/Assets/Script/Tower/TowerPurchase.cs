using UnityEngine;

public class TowerPurchase : MonoBehaviour
{
    private CoinManager _coinManager;
    
    public GameObject _towerToPurchasse;
    public int _moneyToSpend;
    
    private void Start()
    {
        _coinManager = FindFirstObjectByType<CoinManager>();
    }
    
    public void BuyTower(TowerPurchase towerPurchase)
    {
        if (towerPurchase._moneyToSpend > _coinManager._coin || FindAnyObjectByType<DragAndDrop>() != null)
        { 
            return;
        }
        GameObject tower = Instantiate(towerPurchase._towerToPurchasse);
        _coinManager.RemoveCoin(towerPurchase._moneyToSpend);
    }
}
