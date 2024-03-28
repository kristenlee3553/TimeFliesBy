using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameUIHandler : MonoBehaviour
{
    /// <summary>
    /// UI that tells player what phase.
    /// Not sure why there is an m.
    /// Unity tutorial had that there.
    /// </summary>
    private VisualElement m_Timebar;

    /// <summary>
    /// Holds all 3 orbs
    /// </summary>
    private VisualElement m_OrbContainer;

    private VisualElement m_resetButton;

    private VisualElement m_menuButton;

    [SerializeField] GameObject wizard;

    [SerializeField] GameObject pauseMenuObject;

    private ResetManager resetManager;

    private PauseMenu pauseMenuManager;

    /// <summary>
    /// So that other classes can call methods here using the class name
    /// </summary>
    public static GameUIHandler Instance { get; private set; }

    // Awake is called when the script instance is being loaded (in this situation, when the game scene loads)
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Timebar = uiDocument.rootVisualElement.Q<VisualElement>("TimeBar");
        m_OrbContainer = uiDocument.rootVisualElement.Q<VisualElement>("OrbContainer");
        m_menuButton = uiDocument.rootVisualElement.Q<VisualElement>("MenuButton");
        m_resetButton = uiDocument.rootVisualElement.Q<VisualElement>("ResetButton");

        resetManager = wizard.GetComponent<ResetManager>();
        pauseMenuManager = pauseMenuObject.GetComponent<PauseMenu>();

        m_resetButton.RegisterCallback<ClickEvent>(ResetEvent);
        m_menuButton.RegisterCallback<ClickEvent>(MenuEvent);

        SetPhase(1);
        SetOrbCounter();
    }

    private void ResetEvent(ClickEvent evt)
    {
        resetManager.ResetLevel(true);
    }

    private void MenuEvent(ClickEvent evt)
    {
        pauseMenuManager.Pause();
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
}
