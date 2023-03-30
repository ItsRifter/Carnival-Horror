using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAudio : MonoBehaviour
{
    public Transform creatureTransform;
    public AudioSource creatureAudioSource;
    public AudioLowPassFilter audioLowPassFilter;
    public float cutoffPercentage;
    public float distanceMultiplier;
    public float distanceBetweenPlayerAndCreature;
    public int maxObstacleCountForFullCutoffFrequency;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*(print("SPatial blend: " + creatureAudioSource.spatialBlend);
        int layerMask = 1 << 6;
        RaycastHit hit;
        Vector3 dir = (creatureTransform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, ~layerMask) && hit.transform == creatureTransform && CreatureVisibilityCheck())
        {
            //audioLowPassFilter.enabled = false; 
            Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
            audioLowPassFilter.cutoffFrequency = Mathf.Lerp(audioLowPassFilter.cutoffFrequency, 10000f, Time.deltaTime * 2f);
        }
        else
        {
            //Lower cutoff frequency to create more muffled effect.
            audioLowPassFilter.cutoffFrequency = Mathf.Lerp(audioLowPassFilter.cutoffFrequency, 1000f, Time.deltaTime * 2f);
            Debug.DrawRay(transform.position, dir * 1000, Color.white);
        }*/

        distanceBetweenPlayerAndCreature = Vector3.Distance(transform.position, creatureTransform.position);

        int layerMask = 1 << 6;
        RaycastHit[] hits;
        RaycastHit hit;
        Vector3 dir = (creatureTransform.position - transform.position).normalized;
        hits = Physics.RaycastAll(transform.position, dir, distanceBetweenPlayerAndCreature, ~layerMask);
        //print("Number of raycast hits: " + hits.Length);
        CreatureWithinViewFrustrum();

        if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, ~layerMask) && hit.transform == creatureTransform && CreatureWithinViewFrustrum())
        {
            //audioLowPassFilter.enabled = false; 
            Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
            audioLowPassFilter.cutoffFrequency = Mathf.Lerp(audioLowPassFilter.cutoffFrequency, 10000f, Time.deltaTime * 2f);
        }
        else if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, ~layerMask) && hit.transform == creatureTransform && !CreatureWithinViewFrustrum())
        {
            //print("Behind player");
            audioLowPassFilter.cutoffFrequency = Mathf.Lerp(audioLowPassFilter.cutoffFrequency, Mathf.Clamp(10000f * cutoffPercentage, 800f, 3000f), Time.deltaTime * 2f);
        }
        else
        {
            //Lower cutoff frequency to create more muffled effect.
            //distanceMultiplier = 1 + Mathf.Clamp(hit.distance, 0, 80) / 80;
            //maxObstacleCountForFullCutoffFrequency in cutoffPercentage represents the max amount of obstacles needed between the player to reduce the cutoff frequency to roughly 800 (As muffled as possible while being audible).
            cutoffPercentage = 1 - Mathf.Clamp(hits.Length, 0, maxObstacleCountForFullCutoffFrequency) / maxObstacleCountForFullCutoffFrequency;
            //creatureAudioSource.volume = Mathf.Lerp(creatureAudioSource.volume, 1f * cutoffPercentage, Time.deltaTime * 2f);
            audioLowPassFilter.cutoffFrequency = Mathf.Lerp(audioLowPassFilter.cutoffFrequency, Mathf.Clamp(10000f * cutoffPercentage, 800f, 10000f), Time.deltaTime * 2f);
            Debug.DrawRay(transform.position, dir * 1000, Color.white);
        }
    }

    bool CreatureWithinViewFrustrum()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, creatureTransform.GetComponent<Collider>().bounds))
        {
            //print("The object" + creatureTransform.name + "has appeared");
            return true;

        }
        else
        {
            // print("The object" + creatureTransform.name + "has disappeared");
            return false;
        }
    }
}

