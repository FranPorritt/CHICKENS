using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TitleWander : MonoBehaviour
{

    [SerializeField]
    private NavMeshAgent agent;
    
    private NavMeshAgent myNMagent;
    private Transform startTransform;

    [SerializeField]
    private float runToMin;
    [SerializeField]
    private float runToMax;
    
    [SerializeField]
    private float m_MapExtentX;
    [SerializeField]
    private float m_MapExtentZ;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
        RandomPosition();
        
        myNMagent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Wandering();
    }

    private void RandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-m_MapExtentX, m_MapExtentX), Random.Range(0f, 0f), Random.Range(-m_MapExtentZ, m_MapExtentZ));
        agent.SetDestination(randomPosition);
    }

    public void Wandering()
    {
        if (agent.remainingDistance < 0.5)
        {
            RandomPosition();
        }
    }
}
