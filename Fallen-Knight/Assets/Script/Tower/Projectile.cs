using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject _target;
    public float _projectileSpeed;
    public int _damage;
    public Effect _effect;
    
    private Vector3 _targetPosition;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _projectileSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPosition) == 0f)
        {
            if (_effect != null)
            {
                _effect.ApplyEffect(_target);
                Destroy(gameObject);
            }
            else
            {
                _target.GetComponent<Mob>().RemoveHealth(_damage);
                Destroy(gameObject);
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