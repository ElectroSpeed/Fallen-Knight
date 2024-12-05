using UnityEngine;

public class RotativeTower : MonoBehaviour
{
    [Header("Rotative Tower Settings")]
    [SerializeField] private GameObject _rotativeWeapon;
    [Tooltip("This value is in degrees per second")]
    [Range(0f, 360f)] [SerializeField] private float _rotationSpeed;
    [SerializeField] private RotationAxis _rotationAxis;
    [SerializeField] private RotationDirection _rotationDirection;
    [Header("Weapon Settings")]
    public int _damageApplyedToMobs;
    
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
        RotateWeapon();
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Mobs>() != null)
        {
            other.gameObject.GetComponent<Mobs>().RemoveHealth(_damageApplyedToMobs);
        }
    }

    public void UpgradeRotativeTower(TowerPurchase towerPurchase)
    {
        Quaternion oldRotationWeapon = Quaternion.Euler(_rotativeWeapon.transform.rotation.eulerAngles.x, _rotativeWeapon.transform.rotation.eulerAngles.y, _rotativeWeapon.transform.rotation.eulerAngles.z);
        Destroy(gameObject.transform.GetChild(0).gameObject);
        GameObject _upgradedTower = Instantiate(towerPurchase._towerToPurchasse, transform.position,transform.rotation, transform);
        _rotativeWeapon = _upgradedTower.transform.GetChild(0).gameObject;
        _rotativeWeapon.transform.rotation = oldRotationWeapon;
        Debug.Log("Purchase");
    }
}
