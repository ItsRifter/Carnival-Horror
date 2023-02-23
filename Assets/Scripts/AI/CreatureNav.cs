using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureNav : MonoBehaviour
{
    public NavMeshAgent agent;
    private float searchRadius;
    public Vector3 destination;
    public bool randomDestinationFound;
    public bool hasReachedDestination;
    public Transform playerTransform;
    public float maxWaitTime;
    public float waitTimeLeft;

    // Start is called before the first frame update
    void Start()
    {
        waitTimeLeft = maxWaitTime;
        searchRadius = agent.height * 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //print("Agent path: " + agent.pathStatus);
        if (!randomDestinationFound && CheckForRandomNavMeshPoint(playerTransform.position, searchRadius, out destination))
        {
            randomDestinationFound = true;
        }
        else
        {
            agent.SetDestination(destination);

            if (agent.remainingDistance <= 1.0f)
            {
                Wait();
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
        Gizmos.DrawWireSphere(playerTransform.position, searchRadius);

    }
}

