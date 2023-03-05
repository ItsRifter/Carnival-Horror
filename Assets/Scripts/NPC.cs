using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //NPC dialogue mechanic
    //You walk up to an NPC, press E, and they'll start talking to you.
    //Once dialogue has finished, you can walk away.]
    //While talking, you can't move.
    // Start is called before the first frame update

    public string[] dialogueText;
    public int currentDialogueLine;
    public float dialogueActiviationRange;
    public Transform playerTransform;
    public GameObject dialoguePanelGO;
    public TMP_Text dialogueTextBox;
    public SphereCollider sphereCollider;
    public GameObject dialoguePrompt;
    public PlayerMove playerMove;
    public bool playerIsTalking;

    void Start()
    {
        sphereCollider.radius = dialogueActiviationRange;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        //If we are looking at NPC and we're within range, enable button prompt.
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, dialogueActiviationRange) && Vector3.Distance(transform.position, playerTransform.position) <= dialogueActiviationRange)
        {
            dialoguePrompt.SetActive(true);

            //Player is talking
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerIsTalking = true;
                playerMove.enabled = false;
                if (currentDialogueLine < dialogueText.Length - 1)
                {
                    currentDialogueLine++;
                    dialoguePanelGO.SetActive(true);
                    dialogueTextBox.text = dialogueText[currentDialogueLine];
                    print("Show dialogue");
                }
                else
                {
                    playerIsTalking = false;
                    dialoguePanelGO.SetActive(false);
                }
            }
        }
        else
        {
            dialoguePrompt.SetActive(false);
        }

        if(!playerIsTalking)
        {
            currentDialogueLine = -1;
            playerMove.enabled = true;
        }
    }
}
