using UnityEngine;

[ExecuteAlways]
public class Tower : MonoBehaviour
{
    [Header("Tower Type Selection")]
    [SerializeField] private TowerType _towerType;
    private Vector3 _towerPosition;
    
    private enum TowerType
    {
        RotativeTower,
        ProjectileTower,
        ZoneTower
    }
    private void Update()
    {
        ApplyComponentBasedOnTowerType();
    }
    
    public void SetActivationComponent()
    {
        switch (_towerType)
        {
            case TowerType.RotativeTower:
                if (GetComponent<RotativeTower>() != null)
                {
                    var component = gameObject.GetComponent<RotativeTower>().enabled = true;
                }
                break;

            case TowerType.ProjectileTower:
                if (GetComponent<ProjectileTower>() != null)
                {
                    var component = gameObject.GetComponent<ProjectileTower>().enabled = true;
                }
                break;

            case TowerType.ZoneTower:
                if (GetComponent<ZoneTower>() != null)
                {
                    var component = gameObject.GetComponent<ZoneTower>().enabled = true;
                }
                break;

            default:
                Debug.LogWarning("Type de tour non pris en charge");
                break;
        }
    }
    
    private void ApplyComponentBasedOnTowerType()
    {
        RemoveOtherTowerComponents();
        switch (_towerType)
        {
            case TowerType.RotativeTower:
                if (GetComponent<RotativeTower>() == null)
                {
                    gameObject.AddComponent<RotativeTower>();
                }
                break;

            case TowerType.ProjectileTower:
                if (GetComponent<ProjectileTower>() == null)
                {
                    gameObject.AddComponent<ProjectileTower>();
                }
                break;

            case TowerType.ZoneTower:
                if (GetComponent<ZoneTower>() == null)
                {
                    gameObject.AddComponent<ZoneTower>();
                }
                break;

            default:
                Debug.LogWarning("Type de tour non pris en charge");
                break;
        }
    }
    
    private void RemoveOtherTowerComponents()
    {
        if (_towerType != TowerType.RotativeTower && GetComponent<RotativeTower>() != null)
        {
            DestroyImmediate(GetComponent<RotativeTower>());
        }
        if (_towerType != TowerType.ProjectileTower && GetComponent<ProjectileTower>() != null)
        {
            DestroyImmediate(GetComponent<ProjectileTower>());
        }
        if (_towerType != TowerType.ZoneTower && GetComponent<ZoneTower>() != null)
        {
            DestroyImmediate(GetComponent<ZoneTower>());
        }
    }
}

