using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashLightComponent;
    private AudioSource flashLightToggleSFX;
    // Start is called before the first frame update
    void Start()
    {
        flashLightToggleSFX = GetComponent<AudioSource>();  
    }

    // Update is called once per frame
    void Update()
    {
        //If F is pressed, toggle flashlight
        if(Input.GetKeyDown(KeyCode.F))
        {
            float randPitch = Random.Range(0.9f, 1.1f);
            flashLightToggleSFX.pitch = randPitch;
            flashLightToggleSFX.Play();
            flashLightComponent.enabled = !flashLightComponent.enabled;
        }
    }
}
