using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenHouse : MonoBehaviour
{
    public delegate void ChickenHouseEvent();
    public static event ChickenHouseEvent OnChickenEnterHome;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ChickenHouseCollider"))
        {
            Debug.Log("Chicken in da house" + other.name);
            if (OnChickenEnterHome != null)
            {
                OnChickenEnterHome();
            }
            Destroy(other.gameObject.transform.parent.gameObject);
            
        }
    }
}
