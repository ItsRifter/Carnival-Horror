using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public MeshRenderer gateMeshRenderer;
    public Collider gateCollider;
    private AudioSource audioSource;
    private bool hasEnteredMaze;

    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //This boolean ensures that the gate closing sound is only played once.
        if (hasEnteredMaze) return;

        //We make the collider a non-trigger collider to ensure the player cannot pass through the gate
        //once they enter the maze.
        if(other.gameObject.tag == "Player")
        {
            gateMeshRenderer.enabled = true;
            gateCollider.isTrigger = false;
            audioSource.Play();
            hasEnteredMaze = true;
        }
    }
}
