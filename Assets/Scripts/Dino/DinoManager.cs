using TMPro;
using UnityEngine;

/// <summary>
/// Holds key variables in dinosaur stage
/// HAS THINGS TO DELETE LATER
/// </summary>
public class DinoManager : MonoBehaviour
{

    /// <summary>
    /// If wizard is in cave
    /// </summary>
    public static bool s_inCave = false;

    /// <summary>
    /// Dialogue for the dino level
    /// </summary>
    public DialogueAsset[] dialogueAsset;

    /// <summary>
    /// If initial dialogue started
    /// </summary>
    private static bool initialDiaStarted = false;

    // Delete later
    public static bool levelOver = false;
    private static bool resetText = false;
    /// <summary>
    /// DELETE
    /// </summary>
    [SerializeField] private TMP_Text test;

    private void Update()
    {
        // Initial dialogue when level is loaded
        if (!initialDiaStarted)
        {
            initialDiaStarted = true;

            // Start dialogue
            GameUIHandler.Instance.StartDialogue(dialogueAsset[0].dialogue);
            GameUIHandler.Instance.SetHintText("Perhaps we can make use of Fairy's freezing ability...");
        }

        // Delete later
        if (levelOver)
        {
            levelOver = false;
            ShowGameOver();
        }
        
        // Delete later
        if (resetText)
        {
            resetText = false;
            test.SetText("");
        }

    }

    /// <summary>
    /// Resets all variables in the dino level
    /// </summary>
    public static void ResetVariables()
    {
        s_inCave = false;
        initialDiaStarted = false;
        levelOver = false;
        resetText = true;
    }

    // DELETE LATER
    private void ShowGameOver() 
    {
        int numOrbs = 0;

        foreach (bool orb in GameManager.s_curOrbs)
        {
            if (orb)
            {
                numOrbs++;
            }
        }

        test.SetText("LEVEL CLEAR SCREEN! Num orbs collected: " + numOrbs);
    }

    void JoinConversation()
    {
        ResetManager.Instance.StopAllVelocity();
        ResetManager.Instance.DisableFairyMovement(true);
        ResetManager.Instance.DisablePower(true);
        ResetManager.Instance.DisableWizardInput(true);
    }

    void LeaveConversation()
    {
        ResetManager.Instance.DisableFairyMovement(false);
        ResetManager.Instance.DisablePower(false);
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
        GameUIHandler.Instance.ShowOrbDisplay();
    }

}
