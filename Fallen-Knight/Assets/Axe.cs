using System;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Mob>() != null)
        {
            transform.parent.parent.parent.parent.GetComponent<RotativeTower>().Damage(other.gameObject);
        }
    }
}
