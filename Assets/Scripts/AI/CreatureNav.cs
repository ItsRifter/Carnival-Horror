using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureNav : MonoBehaviour
{
    public NavMeshAgent agent;
    private float randomSearchRadius;
    public float hearingRadius;
    public Vector3 destination;
    public bool randomDestinationFound;
    public bool hasReachedDestination;
    public Transform playerTransform;
    public float maxWaitTime;
    public float waitTimeLeft;
    public SphereCollider sphereCollider;
    public bool canHearPlayer;
    public Vector3 lastHeardPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        waitTimeLeft = maxWaitTime;
        randomSearchRadius = agent.height * 20.0f;
        sphereCollider.radius = hearingRadius;
    }

    // Update is called once per frame
    void Update()
    { 
        //Find a random point to go to if we haven't already.
        if (!randomDestinationFound && CheckForRandomNavMeshPoint(playerTransform.position, randomSearchRadius, out destination))
        {
            randomDestinationFound = true;
        }
        else
        {
            //Move to destination then wait for specified period of time once we've reached it.
            agent.SetDestination(destination);

            if (agent.remainingDistance <= 1.0f)
            {
                Wait();
            }
        }

        if(Vector3.Distance(transform.position, playerTransform.position) <= hearingRadius)
        {
            print("Moving to last heard player position");
            lastHeardPlayerPosition = playerTransform.position;
            canHearPlayer = true;
        }

        if(canHearPlayer)
        {
            agent.SetDestination(lastHeardPlayerPosition);

            if(agent.remainingDistance <= 1.0f && Vector3.Distance(transform.position, playerTransform.position) > hearingRadius)
            {
                canHearPlayer = false;
            }
        }
    }

    bool CheckForRandomNavMeshPoint(Vector3 _sphereCenterPoint, float _searchRange, out Vector3 _destiniation)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = _sphereCenterPoint + Random.insideUnitSphere * _searchRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                _destiniation = hit.position;
                return true;
            }
        }
        _destiniation = Vector3.zero;
        return false;
    }

    private void Wait()
    {
        waitTimeLeft -= Time.deltaTime;
        if (waitTimeLeft <= 0.0f)
        {
            randomDestinationFound = false;
            waitTimeLeft = maxWaitTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerTransform.position, randomSearchRadius);

    }
}

