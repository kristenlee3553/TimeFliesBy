using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

/// <summary>
/// Stores key variables of tutorial as well controls the dialogue
/// </summary>
public class TutorialManager : MonoBehaviour
{
    public static bool s_hasRipeApple = false;
    public static bool s_hasUnripeApple = false;

    public static bool s_hasSeed = false;
    public static bool s_plantedSeed = false;

    public static bool s_disablePower = false;

    private static bool phase5FirstReached = false;
    private static bool phase4FirstBack = false;
    private static bool phase5SecondReached = false;

    private static bool firstTimeLordMeetingStarted = false;
    private static bool seedPickUpDiaStarted = false;
    private static bool seedPlantDiaStarted = false;

    private static bool firstPowerDiaStarted = false;
    public static bool s_firstPower = false;
    private static bool firstFreeze = false;
    private static bool freezePowerTutStarted = false;

    private static bool unripeDiaStarted = false;
    private static bool ripeDiaStarted = false;

    // Delete later
    public static bool levelOver = false;

    [SerializeField] private Camera camera;
    private VideoPlayer vp;

    /// <summary>
    /// DELETE
    /// </summary>
    [SerializeField] private TMP_Text test;

    public DialogueAsset[] dialogueAsset;

    private void Start()
    {
        vp = camera.GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        // Initial dialogue when level is loaded
        if (!firstTimeLordMeetingStarted)
        {
            firstTimeLordMeetingStarted = true;

            // Start dialogue
            GameUIHandler.Instance.StartDialogue(dialogueAsset[0].dialogue);
            GameUIHandler.Instance.SetHintText("We should pick up the seed bag.");
        }

        // On pick up of seed
        if (!seedPickUpDiaStarted && s_hasSeed)
        {
            seedPickUpDiaStarted = true;
            GameUIHandler.Instance.SetHintText("Where shall we plant this seed?");
            GameUIHandler.Instance.StartDialogue(dialogueAsset[1].dialogue);
        }

        // On seed planted
        if (!seedPlantDiaStarted && s_plantedSeed)
        {
            seedPlantDiaStarted = true;
            GameUIHandler.Instance.SetHintText("The fairy can press E to move time forwards.");
            GameUIHandler.Instance.StartDialogue(dialogueAsset[2].dialogue);
        }
       
        // When fairy moves time forward for the first time
        if (seedPlantDiaStarted && !ResetManager.Instance.IsPowerDisabled()
            && !firstPowerDiaStarted)
        {
            if (Input.GetKeyUp(GameManager.s_keyBinds[GameManager.KeyBind.MoveTimeFor]))
            {
                s_firstPower = true;
                firstPowerDiaStarted = true;
                GameUIHandler.Instance.SetHintText("How tall can this tree grow?");
                StartCoroutine(StartFirstPower());
            }
           
        }

        // First time reaching the last phase
        if (!phase5FirstReached && GameManager.s_curPhase == 5)
        {
            phase5FirstReached = true;
        }

        // If player reached golden apple phase then went back to phase 4. 
        // Trigger dialogue of mentor teaching player about freezing
        if (phase5FirstReached && GameManager.s_curPhase == 4
            && !PreserveManager.Instance.IsPreserving() && !phase4FirstBack)
        {
            GameUIHandler.Instance.SetHintText("I recall the golden apple appears when the tree is fulling grown");
            phase4FirstBack = true;
            StartCoroutine(StartFreezeIntro());
        }

        // Freezing tutorial starts
        if (phase5FirstReached && phase4FirstBack && !phase5SecondReached
            && GameManager.s_curPhase == 5)
        {
            GameUIHandler.Instance.SetHintText(
                "The fairy can freeze objects with the space bar. " +
                "Maybe we can freeze the apple...");
            phase5SecondReached = true;
            freezePowerTutStarted = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[5].dialogue);
        }

        // Fairy first freezes the apple
        if (freezePowerTutStarted && !firstFreeze && PreserveManager.Instance.IsPreserving())
        {
            if (PreserveManager.Instance.GetPreservedObject().name.Equals("RipeApple"))
            {
                GameUIHandler.Instance.SetHintText("Maybe the wizard can " +
                    "jump on a smaller tree to reach the apple");
                firstFreeze = true;
                StartCoroutine(StartFirstFreeze());
            }
        }

        // Pick up of unripe apple
        if (s_hasUnripeApple && !unripeDiaStarted)
        {
            unripeDiaStarted = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[8].dialogue);
        }

        // Level complete
        if (s_hasRipeApple && !ripeDiaStarted)
        {
            ripeDiaStarted = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[7].dialogue);
        }

        // Delete later
        if (levelOver)
        {
            levelOver = false;
            test.SetText("LEVEL CLEAR ART AND SCREEN!");
            ResetManager.Instance.DisableFairyMovement(false);
            ResetManager.Instance.DisablePower(false);
            ResetManager.Instance.DisableWizardInput(false);
        }
    }

    /// <summary>
    /// Showcase fairy using power. Then play dialogue
    /// </summary>
    /// <returns></returns>
    IEnumerator StartFirstPower()
    {
        ResetManager.Instance.TriggerTimeAnimation();
        yield return StartCoroutine(ResetManager.Instance.ChangeScene(2, GameManager.s_level));

        ResetManager.Instance.DisablePower(true);

        yield return new WaitForSeconds(1.5f);
        GameUIHandler.Instance.StartDialogue(dialogueAsset[3].dialogue);
        yield return null;
    }

    IEnumerator StartFreezeIntro()
    {
        s_disablePower = true;
        yield return new WaitForSeconds(1.5f);
        GameUIHandler.Instance.StartDialogue(dialogueAsset[4].dialogue);
        s_disablePower = false;
        yield return null;
    }

    IEnumerator StartFirstFreeze()
    {
        s_disablePower = true;
        yield return new WaitForSeconds(1.5f);
        GameUIHandler.Instance.StartDialogue(dialogueAsset[6].dialogue);
        s_disablePower = false;
        yield return null;
    }

    void JoinConversation()
    {
        ResetManager.Instance.DisableAll(true);
    }

    void LeaveConversation()
    {
        ResetManager.Instance.DisableFairyMovement(false);

        if (s_plantedSeed)
        {
            ResetManager.Instance.DisablePower(false);
        }
        ResetManager.Instance.DisableWizardInput(false);

        // Play cutscene
        if (ripeDiaStarted)
        {
            PlayCutScene();
        }
    }

    private void PlayCutScene()
    {
        // Get ready for cutscene
        ResetManager.Instance.DisableAll(true);
        GameUIHandler.Instance.TurnOffGameUI();

        // Add method to stop video when done
        vp.loopPointReached += StopVideo;

        // Play cutscene
        vp.Play();
    }

    private void StopVideo(VideoPlayer vp)
    {
        vp.Stop();

        // Delete later
        TutorialManager.levelOver = true;
    }

    /// <summary>
    /// Resets internal variables
    /// </summary>
    public static void ResetVariables()
    {
        s_hasRipeApple = false;
        s_hasUnripeApple = false;

        s_hasSeed = false;
        s_plantedSeed = false;

        s_disablePower = false;

        phase5FirstReached = false;
        phase4FirstBack = false;
        phase5SecondReached = false;

        firstTimeLordMeetingStarted = false;
        seedPickUpDiaStarted = false;
        seedPlantDiaStarted = false;

        firstPowerDiaStarted = false;
        firstFreeze = false;
        freezePowerTutStarted = false;
        s_firstPower = false;

        unripeDiaStarted = false;
        ripeDiaStarted = false;

        GameUIHandler.Instance.HideOrbDisplay();
        GameUIHandler.Instance.HideTimeBar();
        ResetManager.Instance.DisablePower(true);
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
