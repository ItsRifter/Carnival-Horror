using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DialogueOptions
{
    [Tooltip("The text option for the user to select")]
    public string optionText;

    [Tooltip("Where to branch the next dialogue based off this option")]
    public int branchPath;
}

[System.Serializable]
public struct DialogueSegment
{
    [Tooltip("Sprite of speaker")]
    public Sprite speakerSprite;

    [Tooltip("Who is currently speaking")]
    public string speakerName;

    [Tooltip("What the message should contain to the user")]
    public string message;

    [Tooltip("Dialogue options the user can pick, leave empty for no options")]
    public DialogueOptions[] options; 

    [Tooltip("Which message to display next provided there is no dialogue options")]
    public int next;

    [Tooltip("Is this segment the last message, is so the UI will disable")]
    public bool endOfTree;
}

public class DialogueTree : MonoBehaviour
{
    //NPC dialogue mechanic
    //You walk up to an NPC, press E, and they'll start talking to you.
    //Once dialogue has finished, you can walk away.]
    //While talking, you can't move.
    // Start is called before the first frame update

    int curDialogueIndex;

    [SerializeField]
    float dialogueActiviationRange;

    public Transform playerTransform;
    public GameObject dialoguePanelGO;
    public TMP_Text dialogueTextBox;
    public TMP_Text speakerBox;
    public Image speakerImage;

    SphereCollider sphereCollider;
    public GameObject dialoguePrompt;
    public PlayerMove playerMove;

    [SerializeField]
    GameObject[] optionBtns;

    public DialogueSegment[] dialogueSegments;

    public static bool playerIsTalking;
    bool shouldUpdate;
    bool canTalk;
    bool inConversation;
    bool wasBranchingSegment;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = dialogueActiviationRange;
        curDialogueIndex = 0;
        canTalk = false;
        inConversation = false;
        wasBranchingSegment = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Can the player talk and not in a conversation already
        if(canTalk && !inConversation)        
        {
            //Show to the player they can talk
            dialoguePrompt.SetActive(true);

            //Starts the conversation after the player interacts
            if (Input.GetKeyDown(KeyCode.E))
            {
                inConversation = true;
                RestrictPlayerActions();
                wasBranchingSegment = true;
            }    
        }
        else
        {
            dialoguePrompt.SetActive(false);
        }

        if (inConversation)
        {
            //Should the tree update
            if (ShouldUpdateTree())
            {
                shouldUpdate = false;

                //The last dialogue segment is not the last message
                if (!dialogueSegments[curDialogueIndex].endOfTree)
                {
                    //If the last segment didn't have branching options, move onto the next segment
                    if(!wasBranchingSegment)
                        curDialogueIndex = dialogueSegments[curDialogueIndex].next;

                    wasBranchingSegment = false;

                    //Update speaker and dialogue text 
                    if (dialogueSegments[curDialogueIndex].speakerSprite != null)
                    {
                        speakerImage.enabled = true;
                        speakerImage.sprite = dialogueSegments[curDialogueIndex].speakerSprite;
                    }
                    else
                    {
                        speakerImage.enabled = false;
                    }
                    speakerBox.text = dialogueSegments[curDialogueIndex].speakerName;          
                    dialogueTextBox.text = dialogueSegments[curDialogueIndex].message;

                    //New segment has branch options
                    if(dialogueSegments[curDialogueIndex].options.Length > 0)
                    {
                        ShowOptions();
                    }

                    dialoguePanelGO.SetActive(true);
                }
                //if it is the last message, allow player movement and disable the panel
                else
                {
                    //playerIsTalking = false;
                    dialoguePanelGO.SetActive(false);
                    RestorePlayerActions();
                    inConversation = false;
                }
            }
        }
    }

    private bool ShouldUpdateTree()
    {
        //if we should update by default
        if (shouldUpdate) return true;

        //if the player pressed the E key to continue
        if (Input.GetKeyDown(KeyCode.E))
        {
            //While there are no current options in the segment
            if (dialogueSegments[curDialogueIndex].options.Length == 0)
                return true;
        }

        return false;
    }

    private void RestrictPlayerActions()
    {
        PlayerLook.ToggleLooking(false);
        PlayerLook.SetCursorState(CursorLockMode.Confined);

        playerIsTalking = true;
        playerMove.enabled = false;
    }

    private void RestorePlayerActions()
    {
        PlayerLook.ToggleLooking(true);
        PlayerLook.SetCursorState(CursorLockMode.Locked);
        curDialogueIndex = 0;
        playerMove.enabled = true;
        playerIsTalking = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            canTalk = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            canTalk = false;
        }
    }

    private void ShowOptions()
    {
        wasBranchingSegment = true;
        int optionIndex = 0;

        //Show every option buttons and set their option text
        foreach (var btn in optionBtns)
        {
            //check if the length of options exceed the index, give a warning debug message if it does
            if (dialogueSegments[curDialogueIndex].options.Length - 1 < optionIndex)
            {
                Debug.LogWarning("A option in this dialogue exceeds the index");
                break;
            }

            string optionTxt = dialogueSegments[curDialogueIndex].options[optionIndex].optionText;

            //If an option text has null or empty string, continue on but leave a warning debug message
            if (string.IsNullOrEmpty(optionTxt))
            {
                Debug.LogWarning("A option in this dialogue has no option text");
                continue;
            }

            btn.SetActive(true);
            btn.GetComponentInChildren<TMP_Text>().text = optionTxt;

            optionIndex++;
        }
    }

    private void HideOptions()
    {
        foreach (var btn in optionBtns)
        {
            btn.SetActive(false);
        }
    }

    public void SelectOption(int index = 0)
    {
        curDialogueIndex = dialogueSegments[curDialogueIndex].options[index].branchPath;

        speakerBox.text = dialogueSegments[curDialogueIndex].speakerName;
        dialogueTextBox.text = dialogueSegments[curDialogueIndex].message;

        HideOptions();

        shouldUpdate = true;
    }
}
