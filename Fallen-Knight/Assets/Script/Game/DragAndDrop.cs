using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Camera _cam;
    private LayerMask _layerMask = 1 << 3;

    private void Start()
    {
        _cam = Camera.main;
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~_layerMask))
        {
            return new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
        
        Vector3 mousePosWorld = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cam.WorldToScreenPoint(transform.position).z));
        return new Vector3(mousePosWorld.x, transform.position.y, mousePosWorld.z);
    }

    private void Update()
    {
        DragTower();
    }

    public void DragTower()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
        {
            transform.position = new Vector3(GetMousePosition().x, 1, GetMousePosition().z);
        }
        else
        {
            transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
        }
    }
}