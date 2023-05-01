using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CreatureNav : MonoBehaviour
{
    public NavMeshAgent agent;
    private float randomSearchRadius;
    public float hearingRadius;
    public Vector3 destination;
    //For showing where the creature is headed.
    public Transform destinationDebugTransform;
    public bool randomDestinationFound;
    public bool hasReachedDestination;
    public Transform playerTransform;
    public float maxWaitTime;
    public float waitTimeLeft;
    public SphereCollider sphereCollider;
    public bool canHearPlayer;
    public Vector3 lastHeardPlayerPosition;
    public PlayerMove playerMove;
    public bool isMovingToLastHeardPlayerPos;
    public AudioClip[] creatureSounds;
    public AudioSource audioSource;

    [SerializeField]
    float attackDist;

    // Start is called before the first frame update
    void Start()
    {
        waitTimeLeft = Random.Range(3.0f, maxWaitTime);
        randomSearchRadius = agent.height * 20.0f;
        sphereCollider.radius = hearingRadius;
    }

    // Update is called once per frame
    void Update()
    {
        destinationDebugTransform.position = destination;
        for (int i = 0; i < agent.path.corners.Length - 1; i++)
        {
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
        }

        if (!audioSource.isPlaying)
        {
            int randInt = Random.Range(0, creatureSounds.Length);
            audioSource.clip = creatureSounds[randInt];
            audioSource.Play();
        }
        

        //If the creature is within range of player to attack
        if(Physics.CheckSphere(transform.position, attackDist, LayerMask.GetMask("Player") ) )
        {
            PlayerMove.ToggleControls(false);
            PlayerLook.KillPlayer();
        }

        //Find a random point to go to if we haven't already.
        if (!randomDestinationFound && CheckForRandomNavMeshPoint(playerTransform.position, randomSearchRadius, out destination) && !isMovingToLastHeardPlayerPos)
        {
            randomDestinationFound = true;
        }
        else
        {
            //Move to random destination then wait for specified period of time once we've reached it.
            //Once creature has finished waiting, it goes back to moving randomly.
            print("Destination: " + agent.SetDestination(destination) + destination);
            print("Path status: " + agent.pathStatus);
            /*if(agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                randomDestinationFound = false;
            }*/
            agent.SetDestination(destination);

            if (agent.remainingDistance <= 1.0f)
            {
                print("Waiting");
                Wait();
            }
        }

        //If player is within hearing range of the creature, set canHearPlayer to true.
        if(Vector3.Distance(transform.position, playerTransform.position) <= hearingRadius && !playerMove.isHoldingBreath)
        {
            //print("Moving to last heard player position");
            lastHeardPlayerPosition = playerTransform.position;
            canHearPlayer = true;
            isMovingToLastHeardPlayerPos = true;
        }else if(Vector3.Distance(transform.position, playerTransform.position) <= hearingRadius && playerMove.isHoldingBreath)
        {
            canHearPlayer = false;
            print("Player is within hearing radius, but player is holding their breath so ignore them.");
        }

        if (isMovingToLastHeardPlayerPos)
        {
            agent.SetDestination(lastHeardPlayerPosition);

            if(agent.remainingDistance <= 1.0f)
            {
                isMovingToLastHeardPlayerPos = false;
            }
        }
    }

    bool CheckForRandomNavMeshPoint(Vector3 _sphereCenterPoint, float _searchRange, out Vector3 _destiniation)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = _sphereCenterPoint + Random.insideUnitSphere * _searchRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, 1))
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
            waitTimeLeft = Random.Range(3.0f, maxWaitTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerTransform.position, randomSearchRadius);

    }
}

