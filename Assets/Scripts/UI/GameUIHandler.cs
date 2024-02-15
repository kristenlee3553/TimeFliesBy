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

        m_resetButton.RegisterCallback<ClickEvent>(ResetEvent);
        m_menuButton.RegisterCallback<ClickEvent>(MenuEvent);

        SetPhase(1);
        SetOrbCounter();
    }

    private void ResetEvent(ClickEvent evt)
    {
        StartCoroutine(UnloadScene());
        StartCoroutine(ResetGame());
    }

    private void MenuEvent(ClickEvent evt)
    {
        Debug.Log("Menu Clicked");
    }

    IEnumerator UnloadScene()
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(GameManager.s_currentScene); ;

        // Wait until scene is loaded
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }

    IEnumerator ResetGame()
    {
        // Load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameManager.s_level + 1, LoadSceneMode.Additive);

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move wizard to start point
        GameObject wizard = GameObject.FindWithTag("Wizard");

        if (wizard != null)
        {
            wizard.GetComponent<SpriteRenderer>().color = Color.white;
            wizard.transform.position = new Vector3(GameManager.s_wizardResetX, GameManager.s_wizardResetY, wizard.transform.position.z);

        }

        // Move fairy to respawn point
        GameObject fairy = GameObject.FindWithTag("Fairy");

        if (fairy != null)
        {
            fairy.GetComponent<SpriteRenderer>().color = Color.white;
            fairy.transform.position = new Vector3(GameManager.s_fairyResetX, GameManager.s_fairyResetY, fairy.transform.position.z);
        }

        // Orbs
        GameManager.s_curOrbs[0] = false;
        GameManager.s_curOrbs[1] = false;
        GameManager.s_curOrbs[2] = false;

        CollectableOrbs[] orbs = FindObjectsByType<CollectableOrbs>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int x = 0; x < orbs.Length; x++)
        {
            orbs[x].gameObject.SetActive(true);
        }


        // Reset all preserving variables
        PreserveManager.SetPreservableObject(null);
        PreserveManager.SetPreservedObject(null);
        PreserveManager.SetPreservingWizard(false);

        // Level specific code
        if (GameManager.s_level == "Dino")
        {
            GameObject nest = GameObject.FindGameObjectWithTag("Nest");

            if (nest != null)
            {
                nest.SetActive(false);
            }
        }

        GameManager.s_currentScene = GameManager.s_level + 1;
        GameManager.s_phase = 1;

        // UI
        SetPhase(1);
        SetOrbCounter();

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
