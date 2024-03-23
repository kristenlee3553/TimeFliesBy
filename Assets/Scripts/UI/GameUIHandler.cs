using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System;

/// <summary>
/// Handles functionality of GameUI
/// </summary>
public class GameUIHandler : MonoBehaviour
{
    /// <summary>
    /// UI that tells player what phase.
    /// Not sure why there is an m.
    /// Unity tutorial had that there.
    /// </summary>
    private VisualElement m_Timebar;

    /// <summary>
    /// The whole time contanier
    /// </summary>
    private VisualElement m_TimeContainer;

    /// <summary>
    /// Holds all 3 orbs
    /// </summary>
    private VisualElement m_OrbContainer;

    private VisualElement m_resetButton;
    private VisualElement m_menuButton;
    private VisualElement m_hintButton;

    /// <summary>
    /// Contains 3 buttons
    /// </summary>
    private VisualElement m_menuButtonContanier;
    
    private TextElement m_hintText;
    private VisualElement m_hintCloseButton;

    private VisualElement m_dialogue;
    private VisualElement m_wizardProfile;
    private VisualElement m_timeLordProfile;
    private VisualElement m_fairyProfile;
    private VisualElement m_wizardTag;
    private VisualElement m_timeLordTag;
    private VisualElement m_fairyTag;

    private TextElement m_dialogueText;
    private VisualElement m_nextDialogueButton;
    private VisualElement m_skipDialogueButton;

    private UIDocument uiDocument;
    private VisualElement hintUI;

    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;

    bool skipLineTriggered;
    bool skipDialogueTriggered;

    /// <summary>
    /// Typewriter speed
    /// </summary>
    readonly float charactersPerSecond = 30;

    /// <summary>
    /// So that other classes can call methods here using the class name
    /// </summary>
    public static GameUIHandler Instance { get; private set; }

    // Awake is called when the script instance is being loaded (in this situation, when the game scene loads)
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        hintUI = uiDocument.rootVisualElement.Q<VisualElement>("HintUI");
        m_Timebar = uiDocument.rootVisualElement.Q<VisualElement>("TimeBar");
        m_TimeContainer = uiDocument.rootVisualElement.Q<VisualElement>("TimeBarBackground");
        m_OrbContainer = uiDocument.rootVisualElement.Q<VisualElement>("OrbContainer");
        m_menuButton = uiDocument.rootVisualElement.Q<VisualElement>("MenuButton");
        m_resetButton = uiDocument.rootVisualElement.Q<VisualElement>("ResetButton");
        m_hintButton = uiDocument.rootVisualElement.Q<VisualElement>("HintButton");
        m_menuButtonContanier = uiDocument.rootVisualElement.Q<VisualElement>("MenuButtonContainer");

        m_dialogue = uiDocument.rootVisualElement.Q<VisualElement>("DialogueContainer");
        m_dialogueText = uiDocument.rootVisualElement.Q<TextElement>("DialogueText");
        m_wizardProfile = uiDocument.rootVisualElement.Q<VisualElement>("WizardProfile");
        m_wizardTag = uiDocument.rootVisualElement.Q<VisualElement>("WizardTag");
        m_fairyProfile = uiDocument.rootVisualElement.Q<VisualElement>("FairyProfile");
        m_fairyTag = uiDocument.rootVisualElement.Q<VisualElement>("FairyTag");
        m_timeLordProfile = uiDocument.rootVisualElement.Q<VisualElement>("TimeLordProfile");
        m_timeLordTag = uiDocument.rootVisualElement.Q<VisualElement>("TimeLordTag");
        m_nextDialogueButton = uiDocument.rootVisualElement.Q<VisualElement>("NextDiaButton");
        m_skipDialogueButton = uiDocument.rootVisualElement.Q<VisualElement>("SkipDiaButton");

        m_hintCloseButton = uiDocument.rootVisualElement.Q<VisualElement>("HintCloseButton");
        m_hintText = uiDocument.rootVisualElement.Q<TextElement>("HintText");

        m_hintCloseButton.RegisterCallback<ClickEvent>(CloseHintMenu);
        m_hintButton.RegisterCallback<ClickEvent>(ShowHintMenu);
        m_skipDialogueButton.RegisterCallback<ClickEvent>(SkipDialogue);
        m_nextDialogueButton.RegisterCallback<ClickEvent>(SkipLine);
        m_resetButton.RegisterCallback<ClickEvent>(ResetEvent);
        m_menuButton.RegisterCallback<ClickEvent>(MenuEvent);

        SetPhase(GameManager.s_firstPhase);
        SetOrbCounter();
    }

    /// <summary>
    /// Shows hint menu and disables input
    /// </summary>
    /// <param name="evt"></param>
    private void ShowHintMenu(ClickEvent evt)
    {
        ResetManager.Instance.StopAllVelocity();
        hintUI.style.display = DisplayStyle.Flex;
        StopAllCoroutines();
        ResetManager.Instance.DisableAll(true);
    }

    private void CloseHintMenu(ClickEvent evt)
    {
        hintUI.style.display = DisplayStyle.None;
        ResetManager.Instance.HandlePowerDisabling();
        ResetManager.Instance.DisableWizardInput(false);
        ResetManager.Instance.DisableFairyMovement(false);
    }

    /// <summary>
    /// Sets the hint text
    /// </summary>
    /// <param name="hint"></param>
    public void SetHintText(string hint)
    {
        m_hintText.text = hint;
    }

    /// <summary>
    /// Resets level. Respawns wizard and fairy
    /// </summary>
    /// <param name="evt"></param>
    private void ResetEvent(ClickEvent evt)
    {
        ResetManager.Instance.ResetLevel(true);

        if (GameManager.s_level == "Tut")
        {
            TutorialManager.ResetVariables();
        }
        else if (GameManager.s_level == "Dino")
        {
            DinoManager.ResetVariables();
        }
        else if (GameManager.s_level == "PreTut")
        {
            if (GameManager.s_curPhase == 1)
            {
                PreTutManager.ResetVariables(1);
            }
            else
            {
                PreTutManager.ResetVariables(2);
            }
        }
    }

    private void MenuEvent(ClickEvent evt)
    {
        Debug.Log("Menu Clicked");
        Application.Quit();
    }

    /// <summary>
    /// Updates Time Tracker Bar
    /// </summary>
    /// <param name="phase"></param>
    public void SetPhase(int phase)
    {
        float percent = (phase - 1) / 4.0f;
        m_Timebar.style.width = Length.Percent(100 * percent);
    }

    /// <summary>
    /// Updates orb UI
    /// </summary>
    public void SetOrbCounter()
    {
        for (int x = 0; x < GameManager.s_curOrbs.Length; x++)
        {
            string containerName = "Orb" + x + "Container";

            VisualElement container = m_OrbContainer.Q<VisualElement>(containerName);
            VisualElement orb = container.Q<VisualElement>("Orb");
            orb.SetEnabled(GameManager.s_curOrbs[x]);
        }

    }

    /// <summary>
    /// Starts dialogue. Send dialogue to play
    /// </summary>
    /// <param name="dialogue"></param>
    /// <param name="startPosition"></param>
    /// <param name="name">Wizard, Fairy, TimeLord </param>
    public void StartDialogue(MyDialogue[] dialogue)
    {
        // Stop movement
        ResetManager.Instance.StopAllVelocity();

        // Hide other UI
        HideOrbDisplay();
        m_menuButtonContanier.style.display = DisplayStyle.None;
        
        // Show Dialogue
        m_dialogue.style.display = DisplayStyle.Flex;

        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue));
    }

    private IEnumerator RunDialogue(MyDialogue[] dialogue)
    {
        skipLineTriggered = false;
        skipDialogueTriggered = false;
        OnDialogueStarted?.Invoke();

        for (int i = 0; i < dialogue.Length; i++)
        {
            // Skip dialogue
            if (skipDialogueTriggered && !dialogue[i].checkpoint)
            {
                continue;
            }
            // Skip to checkpoint
            else if (skipDialogueTriggered && dialogue[i].checkpoint)
            {
                skipDialogueTriggered = false;
                skipLineTriggered = false;
            }
            if (!skipLineTriggered)
            {
                // Textwriter effect
                StartCoroutine(TypeTextUncapped(dialogue[i].dialogue));
            }

            // Show appropiate person
            DisplayTagAndProfile(dialogue[i].name);
            while (skipLineTriggered == false && skipDialogueTriggered == false)
            {
                yield return null;
            }
            skipLineTriggered = false;
            HideTagAndProfile(dialogue[i].name);
        }

        OnDialogueEnded?.Invoke();

        // UI
        m_dialogue.style.display = DisplayStyle.None;
        m_menuButtonContanier.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Typewriter effect
    /// Code from here: https://gamedevbeginner.com/dialogue-systems-in-unity/
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private IEnumerator TypeTextUncapped(string line)
    {
        float timer = 0;
        float interval = 1 / charactersPerSecond;
        string textBuffer = null;
        char[] chars = line.ToCharArray();
        int i = 0;

        while (i < chars.Length)
        {
            if (skipLineTriggered || skipDialogueTriggered)
            {
                yield break;
            }
            if (timer < Time.deltaTime)
            {
                textBuffer += chars[i];
                m_dialogueText.text = textBuffer;
                timer += interval;
                i++;
            }
            else
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
    }

    /// <summary>
    /// Skips one line of dialogue
    /// </summary>
    /// <param name="evt"></param>
    public void SkipLine(ClickEvent evt)
    {
        skipLineTriggered = true;
    }

    /// <summary>
    /// Skip to nearest breakpoint
    /// </summary>
    /// <param name="evt"></param>
    public void SkipDialogue(ClickEvent evt)
    {
        skipDialogueTriggered = true;
    }

    /// <summary>
    /// Displays who is speaking
    /// </summary>
    /// <param name="name"></param>
    private void DisplayTagAndProfile(string name)
    {
        // Show appropiate person
        if (name.Equals("Wizard"))
        {
            m_wizardTag.style.display = DisplayStyle.Flex;
            m_wizardProfile.style.display = DisplayStyle.Flex;
        }
        else if (name.Equals("Fairy"))
        {
            m_fairyProfile.style.display = DisplayStyle.Flex;
            m_fairyTag.style.display = DisplayStyle.Flex;
        }
        else
        {
            m_timeLordProfile.style.display = DisplayStyle.Flex;
            m_timeLordTag.style.display = DisplayStyle.Flex;
        }
    }

    /// <summary>
    /// Hides who is speaking
    /// </summary>
    /// <param name="name"></param>
    private void HideTagAndProfile(string name)
    {
        if (name.Equals("Wizard"))
        {
            m_wizardTag.style.display = DisplayStyle.None;
            m_wizardProfile.style.display = DisplayStyle.None;
        }
        else if (name.Equals("Fairy"))
        {
            m_fairyProfile.style.display = DisplayStyle.None;
            m_fairyTag.style.display = DisplayStyle.None;
        }
        else
        {
            m_timeLordProfile.style.display = DisplayStyle.None;
            m_timeLordTag.style.display = DisplayStyle.None;
        }
    }

    /// <summary>
    /// Shows whole game UI
    /// </summary>
    public void TurnOnGameUI()
    {
        uiDocument.enabled = true;
    }

    /// <summary>
    /// Hides whole game UI
    /// </summary>
    public void TurnOffGameUI()
    {
        uiDocument.enabled = false;
    }

    /// <summary>
    /// Hides all orbs
    /// </summary>
    public void HideOrbDisplay()
    {
        m_OrbContainer.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Shows all orbs
    /// </summary>
    public void ShowOrbDisplay()
    {
        m_OrbContainer.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Hides time bar
    /// </summary>
    public void HideTimeBar()
    {
        m_TimeContainer.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Shows time bar
    /// </summary>
    public void ShowTimeBar()
    {
        m_TimeContainer.style.display = DisplayStyle.Flex;
    }
}
