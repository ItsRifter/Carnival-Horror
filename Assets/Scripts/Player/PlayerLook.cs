using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLook : MonoBehaviour
{
    //The mouse sensitivity
    public float mouseSens = 100.0f;

    public Transform playerBody;

    float xRot = 0.0f;

    public static bool allowLooking = true;

    static bool isDead = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Kills the player, sets isDead to true and await to reload scene
    public static void KillPlayer()
    {
        isDead = true;
	}

    public IEnumerator AwaitDeath()
    {
        KillPlayer();

		yield return new WaitForSeconds(7);

        //Reset isDead and controls before reloading scene
        isDead = false;
        PlayerMove.ToggleControls(true);

		//Reloads the maze level
		SceneManager.LoadScene(0);

	}

    float deathRot;

    void Update()
    {
        //If the player is dead, do death aniamtions
        if(isDead)
        {
            //deathRot = Mathf.Lerp(deathRot, 1.0f, Time.deltaTime);

            //playerBody.Rotate(Vector3.left, deathRot);
           
            playerBody.transform.rotation = Quaternion.RotateTowards(playerBody.transform.rotation, new Quaternion(-0.5f, 0f, 0f, 0f), Time.deltaTime * 50.0f);

			StartCoroutine(AwaitDeath());
		}

        //If the player is not allowed to look, stop here
        if (!allowLooking)
            return;

        //Grab mouse left/right and up/down times by sensitity and deltatime
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        //Move the x rotation and clamp the values
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90.0f, 90.0f);

        //Apply rotation
        transform.localRotation = Quaternion.Euler(xRot, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
