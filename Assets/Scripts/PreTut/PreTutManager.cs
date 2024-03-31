using UnityEngine;

public class PreTutManager : MonoBehaviour
{
    public DialogueAsset[] dialogueAsset;
    public GameObject background1;
    public GameObject background2;

    public static bool fairyDoor = false;

    private static bool setBackground2 = false;
    private static bool startedDia1 = false;
    private static bool startedDia2 = false;
    private static bool fairyDiaStarted = false;
    private static bool levelReset = false;

    // Update is called once per frame
    void Update()
    {
        if (!startedDia1 && GameManager.s_level == "PreTut")
        {
            startedDia1 = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[0].dialogue);
            GameUIHandler.Instance.SetHintText("Wizard uses the arrow keys for movement. Fairy uses WASD");
        }
        if (!startedDia2 && startedDia1 && GameManager.s_curPhase == 2)
        {
            startedDia2 = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[1].dialogue);
            GameUIHandler.Instance.SetHintText("Wizard can jump using the space bar");
        }

        if (setBackground2 == false && GameManager.s_curPhase == 2)
        {
            setBackground2 = true;
            SetBackground();
        }

        if (fairyDoor && !fairyDiaStarted)
        {
            fairyDiaStarted = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[2].dialogue);
        }

        if (levelReset)
        {
            levelReset = false;
        }
    }

    private void SetBackground()
    {
        background1.SetActive(false);
        background2.SetActive(true);

    }

    public static void ResetVariables(int phase)
    {
        if (phase == 1)
        {
            startedDia1 = false;
        }
        else
        {
            startedDia1 = true;
        }
        startedDia2 = false;
        fairyDoor = false;
        fairyDiaStarted = false;

    }

    void JoinConversation()
    {
        ResetManager.Instance.DisableAll(true);
    }

    void LeaveConversation()
    {
        ResetManager.Instance.DisableFairyMovement(false);
        ResetManager.Instance.DisablePower(true);
        ResetManager.Instance.DisableWizardInput(false);
    }

    private void OnEnable()
    {
        GameUIHandler.OnDialogueStarted += JoinConversation;
        GameUIHandler.OnDialogueEnded += LeaveConversation;
    }

    private void OnDisable()
    {
        GameUIHandler.OnDialogueStarted -= JoinConversation;
        GameUIHandler.OnDialogueEnded -= LeaveConversation;
    }
}
