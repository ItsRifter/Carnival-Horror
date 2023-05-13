using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerLook : MonoBehaviour
{
    //The mouse sensitivity
    public float mouseSens = 100.0f;

    public Transform playerBody;

    float xRot = 0.0f;

    public static bool allowLooking = true;

    static bool isDead = false;

    //Player respawn position.
    public Transform respawnTransform;
    public Transform creatureResetTransform;
    public NavMeshAgent creatureAgent;
    //public Vector3 creatureRespawnPos;
    private bool hasResetPosition = false;
    public CharacterController controller;
    public PlayerMove playerMove;
    public AudioSource playerDeathSFX;
    private bool hasPlayedDeathSFX;

    //By default, lock the cursor
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
        //Kill the player, setting isDead to true
        KillPlayer();
        if (!playerDeathSFX.isPlaying && !hasPlayedDeathSFX)
        {
            playerDeathSFX.Play();
            hasPlayedDeathSFX = true;
        }
        yield return new WaitForSeconds(6);

        //Reset isDead and controls before reloading scene
        isDead = false;
        PlayerMove.ToggleControls(true);

        if(!hasResetPosition)
        {
            ResetGame();
        }
	}

    void Update()
    {
        //If the player is dead, do death animations
        if(isDead)
        {
            //Make the player's transform act like they fell down
            playerBody.transform.rotation = 
                Quaternion.RotateTowards(playerBody.transform.rotation, 
                new Quaternion(-0.5f, 0f, 0f, 0f), Time.deltaTime * 150.0f);

            hasResetPosition = false;

            //Await dying and restarting the level
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

    public static void ToggleLooking(bool toggle)
    {
        allowLooking = toggle;
    }

    //Sets the cursor state
    public static void SetCursorState(CursorLockMode state)
    {
        Cursor.lockState = state;
    }

    void ResetGame()
    {
        //The character controller is disabled before resetting the player's transform, as it was causing the player's position to not reset.
        controller.enabled = false;
        playerBody.transform.position = respawnTransform.position;
        playerBody.transform.rotation = Quaternion.Euler(0, 90, 0);
        controller.enabled = true;
        creatureAgent.Warp(creatureResetTransform.position);
        playerMove.ResetBreathingTime();
        hasResetPosition = true;
        hasPlayedDeathSFX = false;
    }
}
