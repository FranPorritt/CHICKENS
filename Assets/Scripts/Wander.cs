using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    private enum State
    {
        Wandering,
        Fleeing,
        GoingHome,
    };

    [SerializeField]
    private NavMeshAgent agent;

    private Transform player;
    private NavMeshAgent myNMagent;
    private Transform startTransform;

    [SerializeField]
    private float runToMin;
    [SerializeField]
    private float runToMax;

    private State m_CurrentState;

    [SerializeField]
    private float m_MapExtentX;
    [SerializeField]
    private float m_MapExtentZ;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        m_CurrentState = State.Wandering;
    }

    // Use this for initialization
    void Start()
    {
        RandomPosition();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        myNMagent = this.GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        switch (m_CurrentState)
        {
            case State.Wandering:
                {
                    Wandering();
                    break;
                }

            case State.Fleeing:
                {
                    RunFrom();
                    break;
                }

            default:
                {
                    break;
                }
        }
    }

    private void RandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-m_MapExtentX, m_MapExtentX), Random.Range(0f, 0f), Random.Range(-m_MapExtentZ, m_MapExtentZ));
        agent.SetDestination(randomPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && (m_CurrentState != State.GoingHome))
        {
            m_CurrentState = State.Fleeing;
        }

        if ((other.tag == "House") && (m_CurrentState != State.GoingHome))
        {
            m_CurrentState = State.GoingHome;
            ChickenHome();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player") && (m_CurrentState != State.GoingHome))
        {
            m_CurrentState = State.Wandering;
            RandomPosition();
        }
    }

    public void Wandering()
    {
        if (agent.remainingDistance < 0.5)
        {
            RandomPosition();
        }
    }

    public void ChickenHome()
    {
        agent.SetDestination(LevelManager.Instance.GetHouseWaypoint.transform.position);
    }

    public void RunFrom()
    {
        // stores starting transform
        startTransform = transform;

        //points chicken away from player
        transform.rotation = Quaternion.LookRotation(transform.position - player.position);

        Vector3 runTo = transform.position + transform.forward * Random.Range(runToMin, runToMax);

        NavMeshHit hit;

        NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetAreaFromName("Walkable"));

        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;

        myNMagent.SetDestination(hit.position);
    }
}
