
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mobs : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private Slider _healthBar;
    private int _health;
    
    [SerializeField] private int _priceForDeath;
    [HideInInspector] public int _distanceTravelled;
    
    public float _speed;
    
    public List<WayPoint> _wayPoints = new List<WayPoint>();
    private GameObject _targetWayPoint;
    
    private void Start()
    {
        enabled = false;
        _health = _maxHealth;
 //       _healthBar.maxValue = _maxHealth;
 //       _healthBar.value = _health;

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
        foreach (var wayPoint in GameManager.Instance._mapWaypoints)
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
                GameManager.Instance.RemoveCastleHealth(1);
                Destroy(gameObject);
                return;
            }
            _targetWayPoint = _wayPoints[0]._points[0];
        }
        _distanceTravelled += 1;
        transform.position = Vector3.MoveTowards(transform.position, _targetWayPoint.transform.position, _speed * Time.deltaTime);
    }
    
    private List<Vector3> GenerateArcPath(List<GameObject> anglePoints, int arcResolution)
    {
        List<Vector3> path = new List<Vector3>();
        
        Vector3 startPoint = anglePoints[1].transform.position;
        Vector3 pivotPoint = anglePoints[0].transform.position;
        Vector3 endPoint = anglePoints[2].transform.position;
        
        path.Add(startPoint);
        
        Vector3 center = (startPoint + endPoint) / 2 + (pivotPoint - (startPoint + endPoint) / 2) * 2;
        
        float radius = Vector3.Distance(center, startPoint);
        
        float startAngle = Mathf.Atan2(startPoint.y - center.y, startPoint.x - center.x);
        float endAngle = Mathf.Atan2(endPoint.y - center.y, endPoint.x - center.x);
        
        if (endAngle < startAngle)
        {
            endAngle += 2 * Mathf.PI;
        }
        
        for (int i = 0; i <= arcResolution; i++)
        {
            float t = (float)i / arcResolution;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            Vector3 point = new Vector3(center.x + Mathf.Cos(angle) * radius, center.y + Mathf.Sin(angle) * radius, startPoint.z);
            path.Add(point);
        }
        
        path.Add(endPoint);

        return path;
    }

    public void RemoveHealth(int amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            GameManager.Instance.AddCoin(_priceForDeath);
            Debug.Log("Mob Destroyed");
        }
        UpdateHealth();
    }
    
    private void UpdateHealth()
    {
        _healthBar.value = _health;
    }
}
