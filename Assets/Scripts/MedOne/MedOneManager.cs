using UnityEngine;
using System.Collections;

public class MedOneManager : MonoBehaviour
{
    [SerializeField] private GameObject castle;
    [SerializeField] private GameObject brokenCastle;
    [SerializeField] private GameObject bridgeOpen;
    [SerializeField] private GameObject bridgeDown;
    [SerializeField] private GameObject bridgeTilt;
    [SerializeField] private GameObject gate;
    [SerializeField] private GameObject horsesWalking;

    public DialogueAsset[] dialogueAsset;

    private SpriteRenderer gateSprite;

    public static bool leverFlipped = false;
    public static bool noHorsesInFrontOfStable = false;
    public static bool playStableDia = false;
    public static bool tempGateDia = false;

    private bool bridgeIsDown;
    private bool playHorseWalk;
    private bool horseDiaStarted;
    private bool stableOrbObtained;
    private bool startingDiaPlayed;

    // Start is called before the first frame update
    void Start()
    {
        gateSprite = gate.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startingDiaPlayed)
        {
            startingDiaPlayed = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[0].dialogue);
        }

        if (leverFlipped)
        {
            leverFlipped = false;
            bridgeIsDown = !bridgeIsDown;
            SetUpBridge(GameManager.s_curPhase);
        }
        if (playStableDia)
        {
            if (stableOrbObtained)
            {
                GameUIHandler.Instance.StartDialogue(dialogueAsset[3].dialogue);
            }
            else
            {
                stableOrbObtained = true;
                GameManager.s_curOrbs[1] = true;
                GameUIHandler.Instance.SetOrbCounter();
                GameUIHandler.Instance.StartDialogue(dialogueAsset[2].dialogue);
            }
            playStableDia = false;
        }

        if (tempGateDia)
        {
            tempGateDia = false;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[4].dialogue);
        }
    }

    private void SetUpScene()
    {
        int phase = GameManager.s_curPhase;
        SetUpLever(phase);
        SetUpCastle(phase);
        SetUpGate(phase);
        SetUpBridge(phase);
        noHorsesInFrontOfStable = false;
    }

    public void SetUpBridge(int phase)
    {
        bridgeDown.SetActive(false);
        bridgeOpen.SetActive(false);
        bridgeTilt.SetActive(false);

        if (phase == 4 && bridgeIsDown)
        {
            bridgeTilt.SetActive(true);
        }
        else if (bridgeIsDown)
        {
            bridgeDown.SetActive(true);
        }
        else
        {
            bridgeOpen.SetActive(true);
        }

        CheckHorseFood();
    }

    private void SetUpGate(int phase)
    {
        if (phase == 5)
        {
            gateSprite.color = Color.clear;
        }
        else
        {
            gateSprite.color = Color.white;
        }
    }

    private void SetUpCastle(int phase)
    {
        // Crumbled castle
        if (phase == 5)
        {
            castle.SetActive(false);
            brokenCastle.SetActive(true);
        }
        else
        {
            castle.SetActive(true);
            brokenCastle.SetActive(false);
        }
    }

    private void SetUpLever(int phase)
    {
        // If not preserving the lever, set to default
        if (!PreserveManager.Instance.IsPreserving() ||
            !PreserveManager.Instance.GetPreservedObject().CompareTag("Lever"))
        {
            if (phase == 2)
            {
                bridgeIsDown = true;
            }
            else
            {
                bridgeIsDown = false;
            }
        }
    }

    private void CheckHorseFood()
    {
        int phase = GameManager.s_curPhase;

        // Horses brought to food in phase 1 or food brought to horses in phase 2 + bridge is down
        if ((phase == 1 || phase == 2) && bridgeIsDown)
        {
            GameObject obj;

            // See if correct horse was brought
            if (phase == 1)
            {
                obj = GameObject.FindGameObjectWithTag("Horse");

                if (obj != null)
                {
                    if (obj.name != "HorseOut")
                    {
                        obj = null;
                    }
                    else
                    {
                        ResetManager.Instance.StopFairyHold();
                    }
                }
            }
            else
            {
                obj = GameObject.FindGameObjectWithTag("Cart");
            }

            // Correct object brought to approiate phase
            if (obj != null)
            {

                // Play Dialogue
                if (!horseDiaStarted)
                {
                    playHorseWalk = true;
                    horseDiaStarted = true;
                    GameUIHandler.Instance.StartDialogue(dialogueAsset[1].dialogue);
                }
                else
                {
                    StartCoroutine(StartHorseWalk());
                }
            }
        }
    }

    private IEnumerator StartHorseWalk()
    {
        ResetManager.Instance.DisableAll(true);
        ResetManager.Instance.StopAllVelocity();

        // Hide horses
        GameObject horse = GameObject.FindGameObjectWithTag("Horse");
        if (horse != null)
        {
            horse.SetActive(false);
        }
        float startX = 2.38f;
        float startY = -4.14f;
        horsesWalking.transform.position = new(startX, startY, 0);
        horsesWalking.SetActive(true);

        for (int i = 0; i < 100; i++)
        {
            startX -= 0.0426f;
            horsesWalking.transform.position = new(startX, startY, 0);
            yield return new WaitForSeconds(.01f);
        }

        horsesWalking.SetActive(false);

        noHorsesInFrontOfStable = true;
        ResetManager.Instance.DisableAll(false);
    }

    void JoinConversation()
    {
        ResetManager.Instance.DisableAll(true);
    }

    void LeaveConversation()
    {
        ResetManager.Instance.DisableAll(false);
        GameUIHandler.Instance.ShowOrbDisplay();

        if (playHorseWalk)
        {
            playHorseWalk = false;
            StartCoroutine(StartHorseWalk());
        }
    }

    private void ResetVariables()
    {
        noHorsesInFrontOfStable = false;
        playStableDia = false;
        playHorseWalk = false;
        horseDiaStarted = false;
        stableOrbObtained = false;
        startingDiaPlayed = false;
        tempGateDia = false;
        GameManager.s_curOrbs[1] = false;
        GameUIHandler.Instance.SetOrbCounter();
}

    private void OnEnable()
    {
        ResetManager.AfterSceneLogic += SetUpScene;
        GameUIHandler.OnDialogueStarted += JoinConversation;
        GameUIHandler.OnDialogueEnded += LeaveConversation;
        GameUIHandler.OnResetEvent += ResetVariables;
    }

    private void OnDisable()
    {
        ResetManager.AfterSceneLogic -= SetUpScene;
        GameUIHandler.OnDialogueStarted -= JoinConversation;
        GameUIHandler.OnDialogueEnded -= LeaveConversation;
        GameUIHandler.OnResetEvent -= ResetVariables;
    }

}
