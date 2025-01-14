using UnityEngine;

public class RotativeTower : MonoBehaviour
{
    [Header("Rotative Tower Settings")]
    [SerializeField] private GameObject _rotativeWeapon;
    [SerializeField] private GameObject _rangeIndicator;
    [SerializeField] private GameObject _canvas; 
    [Tooltip("This value is in degrees per second")]
    [Range(0f, 360f)] [SerializeField] private float _rotationSpeed;
    [SerializeField] private RotationAxis _rotationAxis;
    [SerializeField] private RotationDirection _rotationDirection;
    [Header("Weapon Settings")]
    public int _damageApplyedToMobs;

    private bool _canPurchase = false;
    
    private enum RotationAxis
    {
        X,
        Y,
        Z
    }
    
    private enum RotationDirection
    {
        Clockwise = 1,
        AntiClockwise = -1
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && hit.collider.gameObject == gameObject)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        
        RotateWeapon();
        _rangeIndicator.SetActive(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit1, Mathf.Infinity) && hit1.collider.gameObject == gameObject);
    }
    
    private void RotateWeapon()
    {
        if (_rotativeWeapon != null)
        {
            switch (_rotationAxis)
            {
                case RotationAxis.Y:
                    _rotativeWeapon.transform.Rotate(new Vector3(0, (int)_rotationDirection * _rotationSpeed, 0) * Time.deltaTime);
                    break;
                case RotationAxis.X:
                    _rotativeWeapon.transform.Rotate(new Vector3((int)_rotationDirection * _rotationSpeed, 0, 0) * Time.deltaTime);
                    break;
                case RotationAxis.Z:
                    _rotativeWeapon.transform.Rotate(new Vector3(0, 0, (int)_rotationDirection *_rotationSpeed) * Time.deltaTime);
                    break;
            }
        }
    }
    
    public void Damage(GameObject other)
    {
        if (other.gameObject.GetComponent<Mob>() != null)
        {
            other.gameObject.GetComponent<Mob>().RemoveHealth(_damageApplyedToMobs);
        }
    }

    public void UpgradeRotativeTower(GameObject newChild)
    {
        if (_canPurchase)
        {
            Quaternion oldRotationWeapon = Quaternion.Euler(_rotativeWeapon.transform.rotation.eulerAngles.x, _rotativeWeapon.transform.rotation.eulerAngles.y, _rotativeWeapon.transform.rotation.eulerAngles.z);
            Destroy(gameObject.transform.GetChild(1).gameObject);
            GameObject newTowerChild = Instantiate(newChild, transform);
            _rotativeWeapon = newTowerChild.transform.GetChild(0).gameObject;
            _rotativeWeapon.transform.rotation = oldRotationWeapon;
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

    public void CanPurchased(int amount)
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