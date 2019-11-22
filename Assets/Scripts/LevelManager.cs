using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject m_HouseWaypoint;

    public GameObject GetHouseWaypoint { get { return m_HouseWaypoint; } }
}
