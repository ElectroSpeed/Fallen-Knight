using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField] private float _rangeTower;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _rangeIndicator;
    [SerializeField] private List<GameObject> _targetsInRange = new List<GameObject>();
    [SerializeField] private Effect _towerEffect;

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
    [SerializeField] private int _shootDamage;

    private bool _canPurchase = false;

    private float _timeBetweenEffects = 5f;

    private void Update()
    {
        Debug.Log(_targetsInRange.Count);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && hit.collider.gameObject == gameObject)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.radius = _rangeTower;
            _rangeIndicator.transform.localScale = new Vector3(_rangeTower * 2, 0.1f, _rangeTower * 2);
            _rangeIndicator.SetActive(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit1, Mathf.Infinity) && hit1.collider.gameObject == gameObject);
        }

        if (_target == null)
        {
            GetTargetToShoot();
        }

        if (_towerEffect != null && _targetsInRange != null)
        {
            ApplyTowerEffect();
        }

        ShooterRotation();
        ShootProjectile();
    }

    private void ApplyTowerEffect()
    {
        if (_towerEffect != null)
        {
            _timeBetweenEffects += Time.deltaTime;
            if (_timeBetweenEffects >= 5f)
            {
                foreach (var target in _targetsInRange)
                {
                    _towerEffect.ApplyEffect(target);
                }
                _timeBetweenEffects = 0f;
            }
        }
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
            if (mob == null)
            {
                _targetsInRange.Remove(mob);
                return;
            }

            if (mob.GetComponent<Mob>() != null)
            {
                int distanceTravelled = mob.GetComponent<Mob>()._distanceTravelled;
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
            var transformPosition = _target.transform.position - _shooter.transform.position;
            transformPosition.Set(transformPosition.x, 0, transformPosition.z);
            RotateToTarget(Quaternion.LookRotation(transformPosition));
        }
        else
        {
            RotateToTarget(_initialShooterRotation.normalized);
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
            GameObject projectile = Instantiate(_projectile, _shooter.transform.position, _shooter.transform.rotation);
            projectile.GetComponent<Projectile>()._projectileSpeed = _projectileSpeed;
            projectile.GetComponent<Projectile>()._damage = _shootDamage;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Mob>() != null)
        {
            _targetsInRange.Add(other.gameObject);
            if (_target == null)
            {
                GetTargetToShoot();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Mob>() != null)
        {
            _targetsInRange.Remove(other.gameObject);
            if (other.gameObject == _target)
            {
                _target = null;
                GetTargetToShoot();
            }
        }
    }

    public void UpgradeTower(GameObject newChild)
    {
        if (_canPurchase)
        {
            Destroy(gameObject.transform.GetChild(1).gameObject);
            GameObject newTowerChild = Instantiate(newChild, transform);
            _shooter = newTowerChild.transform.GetChild(0).gameObject;
            _initialShooterRotation = _shooter.transform.rotation;
            GameObject child = newTowerChild.transform.GetChild(3).gameObject;
            _rangeTower = child.transform.localScale.x / 2;
            _rangeIndicator = newTowerChild.transform.GetChild(3).gameObject;
        }
    }

    public void UpgradeProjectileTower(GameObject projectile)
    {
        if (_canPurchase)
        {
            projectile.GetComponent<Projectile>()._damage = _shootDamage;
            projectile.GetComponent<Projectile>()._projectileSpeed = _projectileSpeed;
            _shooterRate -= 0.2f;
        }
    }

    public void RemoveCoin(int amount)
    {
        if (_canPurchase)
        {
            CoinManager coinManager = FindFirstObjectByType<CoinManager>();
            coinManager.RemoveCoin(amount);
        }
    }

    public void SellTower(int amount)
    {
        CoinManager coinManager = FindFirstObjectByType<CoinManager>();
        coinManager.AddCoin(amount);
        Destroy(gameObject);
    }

    public void AddTowerEffect(Effect towerEffect)
    {
        if (_canPurchase)
        {
            _towerEffect = towerEffect;
        }
    }

    public void CanPurchase(int amount)
    {
        CoinManager coinManager = FindFirstObjectByType<CoinManager>();
        if (coinManager._coin >= amount)
        {
            _canPurchase = true;
        }
        else
        {
            _canPurchase = false;
        }
    }
}