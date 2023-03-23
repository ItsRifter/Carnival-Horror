using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float speed = 25.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 10.0f;

    float cameraHeight = 2.0f;

    public Transform heightChecker;
    public Transform groundChecker;

    public float checkDist = 0.4f;

    public LayerMask groundMask;
    public LayerMask heightMask;

    CharacterController controller;

    Vector3 velocity;
    bool isGrounded;
    bool canStand;
    float standingSpeed = 5.0f;

    public static bool allowControls = true;
    public float totalBreathingTime;
    public float breathingTimeLeft;
    public bool isHoldingBreathOnCooldown;
    public bool isHoldingBreath;
    public TMP_Text holdingBreathText;
    public Slider breathingUIBar;
    public Image barFillImage;

    public AudioSource heavyBreathingAudioSource;

    // Start is called before the first frame update

    void Start()
    {
        breathingUIBar.maxValue = totalBreathingTime;
        breathingTimeLeft = totalBreathingTime;
        controller = GetComponent<CharacterController>();
    }

    //Toggles the player controls
    public static void ToggleControls(bool setter)
    {
        allowControls = setter;
        PlayerLook.allowLooking = setter;
    }

    // Update is called once per frame
    void Update()
    {
        breathingUIBar.value = breathingTimeLeft;
        //If controls are disallowed, still allow last movement + gravity then ignore player controls
        if (!allowControls)
        {
			velocity.y += gravity * Time.deltaTime;
			controller.Move(velocity * Time.deltaTime);
			return;
        }

        //Checks if the player after finish crouching can they stand up
        canStand = !Physics.CheckSphere(heightChecker.position, checkDist, heightMask);

        //Checks if the player is grounded underneath their feet
        isGrounded = Physics.CheckSphere(groundChecker.position, checkDist, groundMask);

        //If we were falling and just became grounded on floor
        //Reset velocity to -2
        if( isGrounded && velocity.y < 0 )
            velocity.y = -2.0f;

        //Get move inputs
        float posX = Input.GetAxis("Horizontal");
        float posZ = Input.GetAxis("Vertical");

        //combine inputs into a vector3 and move based on rotation
        Vector3 move = transform.right * posX + transform.forward * posZ;

        //make the controller move
        controller.Move(move * speed * Time.deltaTime);

        //If we pressed the jump key AND we're grounded, make the player jump
        /*if(Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);*/

        //If the left control is held down...
        if(Input.GetKey(KeyCode.LeftControl))
        {
            //Set the height of the player by half and lerp the values
            controller.height = Mathf.Lerp(controller.height, cameraHeight / 2, Time.deltaTime * standingSpeed);
        } 
        //Else if not held down...
        else
        {
            //If we can stand up (there is no obstacle in the way)
            //and the height is less than default, set the height and lerp the values
            if (canStand && controller.height < 2.0f ) 
                controller.height = Mathf.Lerp(controller.height, cameraHeight, Time.deltaTime * standingSpeed);
        }
        
        //Applies gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Hold breath when spacebar is pressed.
        if(Input.GetKey(KeyCode.Space) && breathingTimeLeft > 0 && !isHoldingBreathOnCooldown) 
        {
            HoldBreath();
        }
        else
        {
            holdingBreathText.text = "Not holding breath";
            isHoldingBreath = false;
            //Slowly regain breath while not holding breath.
            if (breathingTimeLeft < totalBreathingTime)
            {
                breathingTimeLeft += Time.deltaTime * .4f;
                print("Slowly recovering breath");
            }
            else
            {
                heavyBreathingAudioSource.Stop();
                barFillImage.color = Color.white;
                breathingTimeLeft = totalBreathingTime;
                isHoldingBreathOnCooldown = false;
                isHoldingBreath = false;
            }
        }
	}

    void HoldBreath()
    {
        isHoldingBreath = true;
        breathingTimeLeft -= Time.deltaTime;
        print("Hold breath");
        holdingBreathText.text = "Holding Breath";

        //If we can no longer hold our breath, take a deep breath.
        if (breathingTimeLeft < 0)
        {
            barFillImage.color = Color.grey;
            isHoldingBreath = false;
            isHoldingBreathOnCooldown = true;
            breathingTimeLeft = 0;
            heavyBreathingAudioSource.Play();
        }
    }
}
