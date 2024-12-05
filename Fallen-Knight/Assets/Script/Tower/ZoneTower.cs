using System.Collections.Generic;
using UnityEngine;

public class ZoneTower : MonoBehaviour
{
[Header("Tower Settings")]
    [SerializeField] private float _rangeTower;
    [SerializeField] private List<GameObject> _mobsInRange = new List<GameObject>();
    
    [Header("Shooter Settings")]
    [SerializeField] private GameObject _wizard;
    [SerializeField] private float _wizardRate;
    private float _zoneCooldown  = 1f;
    
    [Header("Zone Settings")]
    [SerializeField] private GameObject _zone;
    [SerializeField] private float _zoneDuration;
    [SerializeField] private Effect _zoneEffect;
    private float _zoneActivationCooldown  = 1f;
    
    private void Update()
    {
        Zone();
    }

    private void Zone()
    {
        if (_zoneCooldown < 1f)
        {
            _zoneCooldown += Time.deltaTime / _wizardRate;
        }
        
        if (_mobsInRange.Count == 0)
        {
            return;
        }

        if (_zoneCooldown >= 1f && _wizard != null)
        {
            _zoneCooldown = 0f;
            Debug.Log("StartZone");
        }

        if (_zoneActivationCooldown < 1f)
        {
            _zoneActivationCooldown += Time.deltaTime / _zoneDuration;
        }
        
        if (_zoneDuration >= 1f)
        {
            _zoneActivationCooldown = 0f;
            Debug.Log("EndZone");
        }
    }
}
