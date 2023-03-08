using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{
    public AudioSource audioSource;
    public float timeLeftUntilPitchChange;
    public float minTimeLeftUntilPitchChange;
    public float maxTimeLeftUntilPitchChange;
    public float minPitch;
    public float maxPitch;
    public float currentPitch;
    public bool hasChosenRandomPitch;
    public float pitchLerpValue;
    public float[] randomPitch;
    public int randInt;

    // Start is called before the first frame update
    void Start()
    {
        timeLeftUntilPitchChange = Random.Range(minTimeLeftUntilPitchChange, maxTimeLeftUntilPitchChange);
    }

    // Update is called once per frame
    void Update()
    {
        timeLeftUntilPitchChange -= Time.deltaTime;
        currentPitch = audioSource.pitch;

        if (timeLeftUntilPitchChange < 0)
        {
            LerpPitch();
        }
    }

    void LerpPitch()
    {
        if (!hasChosenRandomPitch)
        {
           //Choose a random pitch from the array of pitches.
           randInt = Random.Range(0, randomPitch.Length);
           hasChosenRandomPitch = true;
        }
        else
        {
            //Lerp between the current pitch and the randomly chosen pitch.
            audioSource.pitch = Mathf.Lerp(currentPitch, randomPitch[randInt], pitchLerpValue);
            pitchLerpValue += Time.deltaTime * .05f;
        }

        //Once finished lerping, randomly select a new time for the next pitch change.
        if (pitchLerpValue > 1f)
        {
            timeLeftUntilPitchChange = Random.Range(minTimeLeftUntilPitchChange, maxTimeLeftUntilPitchChange);
            hasChosenRandomPitch = false;
            pitchLerpValue = 0f;
        }
    }

}
