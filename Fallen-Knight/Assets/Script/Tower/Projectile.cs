using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject _target;
    public float _projectileSpeed;
    private Vector3 _targetPosition;

    private void Update()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _projectileSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _targetPosition) == 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Start()
    {
        if (_target != null)
        {
            _targetPosition = _target.transform.position;
        }
    }
}

