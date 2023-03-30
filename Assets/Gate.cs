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
        if (hasEnteredMaze) return;

        gateMeshRenderer.enabled = true;
        gateCollider.isTrigger = false;
        audioSource.Play();
        hasEnteredMaze = true;
    }
}
