using System.Collections.Generic;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class Mob : MonoBehaviour
{
private CastleHealthManager _castleHealthManager;
    private CoinManager _coinManager;
    private MapManager _mapManager;
    
    public int _maxHealth;
    [SerializeField] private Slider _healthBar;
    [HideInInspector] public int _health;
    
    [SerializeField] private int _priceForDeath;
    [HideInInspector] public int _distanceTravelled;
    
    public float _speed;
    private float _originalSpeed;
    
    public List<WayPoint> _wayPoints = new List<WayPoint>();
    private GameObject _targetWayPoint;
    
    private void Start()
    {
        enabled = false;
        _health = _maxHealth;
        _castleHealthManager = FindFirstObjectByType<CastleHealthManager>();
        _coinManager = FindFirstObjectByType<CoinManager>();
        _mapManager = FindFirstObjectByType<MapManager>();
        _healthBar.maxValue = _maxHealth;
        _healthBar.value = _health;
        _originalSpeed = _speed;
        
        Invoke(nameof(AfterStart), 0.02f);
    }
    
    private void AfterStart()
    {
        GetWayPoints();
        DrawTrajectory();
        enabled = true;
    }
    
    private void Update()
    {
        MobMovement();
    }

    private void GetWayPoints()
    {
        _wayPoints.Clear();
        foreach (var wayPoint in _mapManager._mapWaypoints)
        {
            _wayPoints.Add(wayPoint.transform.GetChild(0).GetComponent<WayPoint>());
        }
    }
    
    private void DrawTrajectory()
    {
        if (_wayPoints.Count >= 2)
        {
            for (int i = 0; i < _wayPoints.Count - 1; i++)
            {
                Debug.DrawLine(_wayPoints[i].transform.position, _wayPoints[i + 1].transform.position, Color.green, 100f);
            }
        }
    }

    private void MobMovement()
    {
        if (_targetWayPoint == null && _wayPoints.Count != 0)
        {
            _targetWayPoint = _wayPoints[0]._points[0];
        }

        if (_targetWayPoint == null)
        {
            return;
        }
        
        if (Vector3.Distance(transform.position, _targetWayPoint.transform.position) < 0.1f)
        {
            _wayPoints.RemoveAt(0);
            if (_wayPoints.Count == 0)
            {
                _targetWayPoint = null;
                _castleHealthManager.RemoveCastleHealth(1);
                Destroy(gameObject);
                return;
            }
            _targetWayPoint = _wayPoints[0]._points[0];
        }
        _distanceTravelled += 1;
        var direction = (_targetWayPoint.transform.position - transform.position).normalized;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
        transform.position = Vector3.MoveTowards(transform.position, _targetWayPoint.transform.position, _speed * Time.deltaTime);
    }
    
    #region Life

    public void RemoveHealth(int amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            _coinManager.AddCoin(_priceForDeath);
            Destroy(gameObject);
        }
        UpdateHealth();
    }
    
    private void UpdateHealth()
    {
        _healthBar.value = _health;
    }
    
    
    public void ReduceSpeed(float percentage, float duration)
    {
        _speed *= (1 - percentage / 100);
        Invoke(nameof(ResetSpeed), duration);
    }
    
    private void ResetSpeed()
    {
        _speed = _originalSpeed;
    }

    #endregion
}
