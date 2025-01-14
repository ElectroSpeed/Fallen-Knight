using UnityEngine;

public class TowerCell : MonoBehaviour
{
    private Camera _cam;
    private bool _containTower = false;
    private LayerMask _layerMask = 1 << 3;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceTower();
        }
    }

    private void PlaceTower()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask) && hit.collider.gameObject == gameObject)
        {
            if (_containTower)
            {
                return;
            }
            
            DragAndDrop dragTower = FindFirstObjectByType<DragAndDrop>();
            if (dragTower != null)
            {
                _containTower = true;
                RotativeTower rotativeTower = dragTower.GetComponent<RotativeTower>();
                if (rotativeTower != null)
                {
                    rotativeTower.enabled = true;
                }
                ProjectileTower projectileTower = dragTower.GetComponent<ProjectileTower>();
                if (projectileTower != null)
                {
                    projectileTower.enabled = true;
                }
                Destroy(dragTower);
            }
        }
    }
}
