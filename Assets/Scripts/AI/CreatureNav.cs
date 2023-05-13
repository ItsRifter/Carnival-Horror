using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

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
    public Transform[] wanderDestinations;
    private bool startCooldownHasEnded = false;

    private bool hasPlayerHeldBreathBefore;
    public Image breathingPromptPanel;
    public TMP_Text breathingPromptText;

    [SerializeField]
    float attackDist;

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
        if(!startCooldownHasEnded)
        {
            //waitTimeLeft = 5.0f;
            Wait();
        }

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
        if (!randomDestinationFound && /*CheckForRandomNavMeshPoint(playerTransform.position, randomSearchRadius, out destination) &&*/ !isMovingToLastHeardPlayerPos && startCooldownHasEnded)
        {
            destination = RandomlySelectDestination();
            randomDestinationFound = true;
        }
        else
        {
            //Move to random destination then wait for specified period of time once we've reached it.
            //Once creature has finished waiting, it goes back to moving randomly.
            print("Destination: " + agent.SetDestination(destination) + destination);
            print("Path status: " + agent.pathStatus);
            agent.SetDestination(destination);
            print("agent destination: "+ agent.destination);
            print("Agent remaining distance: " + agent.remainingDistance);

            if (Vector3.Distance(transform.position, destination) <= 1.0f)
            {
                hasReachedDestination = true;
            }

            if(hasReachedDestination)
            {
                print("Waiting");
                Wait();
            }
        }

        if (Vector3.Distance(transform.position, playerTransform.position) <= hearingRadius * 1.3f && !playerMove.isHoldingBreath)
        {
            if (!hasPlayerHeldBreathBefore)
            {
                breathingPromptPanel.enabled = true;
                breathingPromptText.enabled = true;
            }
        }

        //If player is within hearing range of the creature, set canHearPlayer to true.
        if (Vector3.Distance(transform.position, playerTransform.position) <= hearingRadius && !playerMove.isHoldingBreath)
        {
      
            //print("Moving to last heard player position");
            lastHeardPlayerPosition = playerTransform.position;
            canHearPlayer = true;
            isMovingToLastHeardPlayerPos = true;
        } else if(Vector3.Distance(transform.position, playerTransform.position) <= hearingRadius && playerMove.isHoldingBreath)
        {
            canHearPlayer = false;
            breathingPromptPanel.enabled = false;
            breathingPromptText.enabled = false;
            hasPlayerHeldBreathBefore = true;
            print("Player is within hearing radius, but player is holding their breath so ignore them.");
        }

        if (isMovingToLastHeardPlayerPos)
        {
            agent.SetDestination(lastHeardPlayerPosition);
            destinationDebugTransform.position = lastHeardPlayerPosition;

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

    public Vector3 RandomlySelectDestination()
    {
        int randInt = Random.Range(0, wanderDestinations.Length - 1);
        return wanderDestinations[randInt].position;
    }

    private void Wait()
    {
        waitTimeLeft -= Time.deltaTime;
        if (waitTimeLeft <= 0.0f)
        {
            startCooldownHasEnded = true;
            randomDestinationFound = false;
            hasReachedDestination = false;
            waitTimeLeft = Random.Range(3.0f, maxWaitTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerTransform.position, randomSearchRadius);

    }
}

