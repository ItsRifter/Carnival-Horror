using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraBobbing : MonoBehaviour
{
    [SerializeField] 
    bool _enabled = true;

    [SerializeField] 
    float walkBobSpeed = 14f;

	[SerializeField] 
    float walkBobAmount = 0.05f;

    float defaultYPos = 0;
    float timer;

    Camera playerCamera;

    CharacterController controller;

	private void Awake()
	{
        controller = GetComponent<CharacterController>();

		playerCamera = GetComponentInChildren<Camera>();
        defaultYPos = playerCamera.transform.localPosition.y;
	}

	void Start()
    {
       
    }

    void Update()
    {
		if (!_enabled) return;

        HandleHeadBob();
	}

    void HandleHeadBob()
    {
        if (!controller.isGrounded) return;

        if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            timer += Time.deltaTime * walkBobSpeed;

            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * walkBobAmount,
                playerCamera.transform.localPosition.z
            );
        }
    }
}
