using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject _normalPrefab;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private GameObject _anglePrefab;
    [SerializeField] private GameObject _intersectionPrefab;
    [SerializeField] private GameObject _tIntersectionPrefab;

    private List<Vector3> _adjacentCells = new List<Vector3>();
    private int _adjacentCellCount;

    private void Start()
    {
        if (!Application.isPlaying)
        {
            AddToMap();
            CheckAdjacentCell();
            UpdateCellType();
        }
        else
        {
            AddToMap();
            Invoke(nameof(AfterStart), 0.01f);
        }
    }

    private void AfterStart()
    {
        CheckAdjacentCell();
        UpdateCellType();
    }

    private void AddToMap()
    {
        if (GameManager.Instance != null && GameManager.Instance._mapCellTransform != null)
        {
            if (!GameManager.Instance._mapCellTransform.Contains(transform.position))
            {
                GameManager.Instance._mapCellTransform.Add(transform.position);
            }
        }
    }

    private void CheckAdjacentCell()
    {
        _adjacentCells.Clear();
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
            if (GameManager.Instance != null && GameManager.Instance._mapCellTransform.Contains(position))
            {
                _adjacentCells.Add(position);
            }
        }
        _adjacentCellCount = _adjacentCells.Count;
    }

    private void UpdateCellType()
    {
        GameObject childrenPath;
        if (transform.childCount > 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }

        switch (_adjacentCellCount)
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
        bool verticalLine = _adjacentCells.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z - cellScale)) && _adjacentCells.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z + cellScale));
        bool horizontalLine = _adjacentCells.Contains(new Vector3(transform.position.x - cellScale, transform.position.y, transform.position.z)) && _adjacentCells.Contains(new Vector3(transform.position.x + cellScale, transform.position.y, transform.position.z));
        return verticalLine || horizontalLine;
    }

    private void GetRotationForLineAdjacent(GameObject cell)
    {
        int cellScale = (int)transform.GetChild(0).GetChild(0).localScale.x;
        bool verticalLine = _adjacentCells.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z - cellScale)) || _adjacentCells.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z + cellScale));
        bool horizontalLine = _adjacentCells.Contains(new Vector3(transform.position.x - cellScale, transform.position.y, transform.position.z)) || _adjacentCells.Contains(new Vector3(transform.position.x + cellScale, transform.position.y, transform.position.z));
        
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

        if (_adjacentCells.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z - cellScale)) && 
            _adjacentCells.Contains(new Vector3(transform.position.x + cellScale, transform.position.y, transform.position.z)))
        {
            // Top-Right
            cell.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_adjacentCells.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z + cellScale)) && 
                 _adjacentCells.Contains(new Vector3(transform.position.x + cellScale, transform.position.y, transform.position.z)))
        {
            // Bottom-Right
            cell.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (_adjacentCells.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z + cellScale)) && 
                 _adjacentCells.Contains(new Vector3(transform.position.x - cellScale, transform.position.y, transform.position.z)))
        {
            // Bottom-Left
            cell.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (_adjacentCells.Contains(new Vector3(transform.position.x, transform.position.y, transform.position.z - cellScale)) && 
                 _adjacentCells.Contains(new Vector3(transform.position.x - cellScale, transform.position.y, transform.position.z)))
        {
            // Top-Left
            cell.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}
