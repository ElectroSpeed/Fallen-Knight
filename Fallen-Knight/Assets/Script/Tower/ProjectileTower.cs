using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField] private float _rangeTower;
    [SerializeField] private GameObject _target;
    [SerializeField] private List<GameObject> _targetsInRange = new List<GameObject>();
    
    [Header("Shooter Settings")]
    [SerializeField] private GameObject _shooter;
    [SerializeField] private Quaternion _initialShooterRotation;
    [Space(10f)]
    [SerializeField] private float _shooterRate;
    [SerializeField] private float _rotationSpeed;
    [Range(0f, 180f)] [SerializeField] private float _angleTreshold;
    private float _rotationTime;
    private float _shootTime = 1f;
    
    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed;
    
    private void Update()
    {
        if (_target == null)
        {
            GetTargetToShoot();
        }
        ShooterRotation();
        ShootProjectile();
    }

    private void GetTargetToShoot()
    {
        if (_targetsInRange == null || _target != null)
        {
            return;
        }

        GameObject targetToShoot = null;
        foreach (var mob in _targetsInRange)
        {
            float maxDistance = 0f;
            if (mob.GetComponent<Mobs>() != null)
            {
                int distanceTravelled = mob.GetComponent<Mobs>()._distanceTravelled;
                if (distanceTravelled > maxDistance)
                {
                    maxDistance = distanceTravelled;
                    targetToShoot = mob;
                }
            }
        }
        _target = targetToShoot;
    }
    
    private void ShooterRotation()
    {
        if (_target != null)
        {
            RotateToTarget(Quaternion.LookRotation(_target.transform.position - _shooter.transform.position));
        }
        else
        {
            RotateToTarget(_initialShooterRotation);
        }
    }
    
    private void RotateToTarget(Quaternion targetRotation)
    {
        _shooter.transform.rotation = Quaternion.RotateTowards(_shooter.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void ShootProjectile()
    {
        if (_shootTime < 1f)
        {
            _shootTime += Time.deltaTime / _shooterRate;
        }
        
        if (_target == null || Math.Abs(AngleBetweenShooterAndTarget()) > _angleTreshold)
        {
            return;
        }
        
        if (_shootTime >= 1f && _projectile != null && _shooter != null)
        {
            _shootTime = 0f;
            _targetsInRange.Remove(_target);
            Debug.Log("Shoot");
            GameObject projectile = Instantiate(_projectile, _shooter.transform.position, _shooter.transform.rotation);
            projectile.GetComponent<Projectile>()._projectileSpeed = _projectileSpeed;
            projectile.GetComponent<Projectile>()._target = _target;
        }
    }
    
    private float AngleBetweenShooterAndTarget()
    {
        if (_target != null)
        {
            float shooterRotationY = _shooter.transform.rotation.eulerAngles.y;
            float angleToTargetY = Quaternion.LookRotation(_target.transform.position - _shooter.transform.position).eulerAngles.y;
    
            float rotationDifference = Mathf.DeltaAngle(shooterRotationY, angleToTargetY);
            return rotationDifference;
        }
        return 180f;
    }
}