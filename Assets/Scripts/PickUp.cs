using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    private GameObject m_EggPrefab;

    public bool isPickUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("PICK UP");
            PowerUp();
        }
    }

    private void PowerUp()
    {
        m_EggPrefab.GetComponentInChildren<MeshRenderer>().enabled = false;
        isPickUp = true;
        Invoke("RestartTimer", 3);
    }

    private void RestartTimer()
    {
        isPickUp = false;
        //DestroyEgg();
    }

    private void DestroyEgg()
    {
        Destroy(m_EggPrefab);
    }
}
