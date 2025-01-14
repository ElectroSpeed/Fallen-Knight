using System.Collections.Generic;
using UnityEngine;

public class PathModifier : MonoBehaviour
{
    [SerializeField] private GameObject _normalPrefab;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private GameObject _anglePrefab;
    [SerializeField] private GameObject _intersectionPrefab;
    
    [SerializeField] private GameObject _startPrefab;

    private MapManager _mapManager;
    private readonly List<Vector3> _adjacentPath = new List<Vector3>();
    private int _adjacentPathCount;

    private void Start()
    {
        _mapManager = FindFirstObjectByType<MapManager>();
        AddPathToMap();
        Invoke(nameof(AfterStart), 0.01f);
    }

    private void AfterStart()
    {
        UpdatePath();
    }
    
    private void UpdatePath()
    {
        CheckAdjacentPath();
        UpdatePathType();
    }

    private void AddPathToMap()
    {
        if (_mapManager != null && _mapManager._mapPath != null)
        {
            if (!_mapManager._mapPath.Contains(transform.position))
            {
                _mapManager._mapPath.Add(transform.position);
            }
        }
    }

    private void CheckAdjacentPath()
    {
        _adjacentPath.Clear();
        int cellScale = (int)transform.GetChild(0).GetChild(0).localScale.x;
        Vector3[] adjacentPositions =
        {
            new Vector3(transform.position.x, transform.position.y, transform.position.z - cellScale),
            new Vector3(transform.position.x, transform.position.y, transform.position.z + cellScale),
            new Vector3(transform.position.x + cellScale, transform.position.y, transform.position.z),
            new Vector3(transform.position.x - cellScale, transform.position.y, transform.position.z)
        };

        foreach (var position in adjacentPositions)
        {
            if (_mapManager == null)
            {
                return;
            }

            if (_mapManager != null && _mapManager._mapPath.Contains(position))
            {
                _adjacentPath.Add(position);
            }
        }
        _adjacentPathCount = _adjacentPath.Count;
    }

    private void UpdatePathType()
    {
        GameObject childrenPath;
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        if (gameObject == _mapManager._mapWaypoints[0])
        {
            childrenPath = Instantiate(_startPrefab, transform);
            childrenPath.transform.parent = transform;
        }

        switch (_adjacentPathCount)
        {
            case 0:
                childrenPath = Instantiate(_normalPrefab, transform);
                childrenPath.transform.parent = transform;
                break;

            case 1:
                childrenPath = Instantiate(_linePrefab, transform);
                childrenPath.transform.parent = transform;
                GetRotationForLineAdjacent(childrenPath);
                break;

            case 2:
                if (IsStraightLine())
                {
                    childrenPath = Instantiate(_linePrefab, transform);
                    childrenPath.transform.parent = transform;
                    GetRotationForLineAdjacent(childrenPath);
                }
                else
                {
                    childrenPath = Instantiate(_anglePrefab, transform);
                    childrenPath.transform.parent = transform;
                    GetRotationForAngle(childrenPath);
                }
                break;

            case 3:
                childrenPath = Instantiate(_intersectionPrefab, transform);
                childrenPath.transform.parent = transform;
                break;

            case 4:
                childrenPath = Instantiate(_intersectionPrefab, transform);
                childrenPath.transform.parent = transform;
                break;
        }
    }

    private bool IsStraightLine()
    {
        int cellScale = (int)transform.GetChild(0).GetChild(0).localScale.x;
        bool isVerticalLine = _adjacentPath.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z - cellScale)) && _adjacentPath.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z + cellScale));
        bool isHorizontalLine = _adjacentPath.Contains(new Vector3(transform.position.x - cellScale, transform.position.y, transform.position.z)) && _adjacentPath.Contains(new Vector3(transform.position.x + cellScale, transform.position.y, transform.position.z));
        return isVerticalLine || isHorizontalLine;
    }

    private void GetRotationForLineAdjacent(GameObject cell)
    {
        int cellScale = (int)transform.GetChild(0).GetChild(0).localScale.x;
        bool verticalLine = _adjacentPath.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z - cellScale)) || _adjacentPath.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z + cellScale));
        bool horizontalLine = _adjacentPath.Contains(new Vector3(transform.position.x - cellScale, transform.position.y, transform.position.z)) || _adjacentPath.Contains(new Vector3(transform.position.x + cellScale, transform.position.y, transform.position.z));
        
        if (verticalLine)
        {
            cell.transform.Rotate(0,0,0);
        }
        else if (horizontalLine)
        {
            cell.transform.Rotate(0, 90, 0);
        }
    }

    private void GetRotationForAngle(GameObject cell)
    {
        int cellScale = (int)transform.GetChild(0).GetChild(0).localScale.x;

        if (_adjacentPath.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z - cellScale)) && 
            _adjacentPath.Contains(new Vector3(transform.position.x + cellScale, transform.position.y, transform.position.z)))
        {
            // Top-Right
            cell.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_adjacentPath.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z + cellScale)) && 
                 _adjacentPath.Contains(new Vector3(transform.position.x + cellScale, transform.position.y, transform.position.z)))
        {
            // Bottom-Right
            cell.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (_adjacentPath.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z + cellScale)) && 
                 _adjacentPath.Contains(new Vector3(transform.position.x - cellScale, transform.position.y, transform.position.z)))
        {
            // Bottom-Left
            cell.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (_adjacentPath.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z - cellScale)) && 
                 _adjacentPath.Contains(new Vector3(transform.position.x - cellScale, transform.position.y, transform.position.z)))
        {
            // Top-Left
            cell.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}