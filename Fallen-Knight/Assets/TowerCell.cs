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
    
    private void OnMouseDown()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        DragAndDrop dragTower = FindFirstObjectByType<DragAndDrop>();
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask) || _containTower)
        {
            return;
        }
        else
        {
            PlaceTower(dragTower);
        }
    }

    private void PlaceTower(DragAndDrop dragTower)
    {
        _containTower = true;
        dragTower.GetComponent<Tower>().SetActivationComponent();
        Destroy(dragTower);
    }
}
