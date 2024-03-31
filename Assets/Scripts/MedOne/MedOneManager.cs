using UnityEngine;

public class MedOneManager : MonoBehaviour
{
    [SerializeField] private GameObject lever;
    [SerializeField] private GameObject castle;
    [SerializeField] private GameObject brokenCastle;
    [SerializeField] private GameObject bridgeOpen;
    [SerializeField] private GameObject bridgeDown;
    [SerializeField] private GameObject bridgeTilt;
    [SerializeField] private GameObject gate;

    private SpriteRenderer leverSprite;
    private SpriteRenderer gateSprite;

    public static bool leverFlipped = false;

    // Start is called before the first frame update
    void Start()
    {
        leverSprite = lever.GetComponent<SpriteRenderer>();
        gateSprite = gate.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (leverFlipped)
        {
            leverFlipped = false;
            SetUpBridge(GameManager.s_curPhase);
        }
    }

    private void SetUpScene()
    {
        int phase = GameManager.s_curPhase;
        SetUpLever(phase);
        SetUpCastle(phase);
        SetUpGate(phase);
        SetUpBridge(phase);
    }

    public void SetUpBridge(int phase)
    {
        bool bridgeIsDown = leverSprite.flipX;
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
                leverSprite.flipX = true;
            }
            else
            {
                leverSprite.flipX = false;
            }
        } 
    }

    private void OnEnable()
    {
        ResetManager.AfterSceneLogic += SetUpScene;
    }

    private void OnDisable()
    {
        ResetManager.AfterSceneLogic -= SetUpScene;
    }
}
