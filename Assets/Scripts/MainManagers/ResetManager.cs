using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// HANDLES RESETTING OF LEVEL, CHANGING PHASES, DEATH ANIMATION
/// </summary>
public class ResetManager : MonoBehaviour
{
    [SerializeField] private GameObject wizard;
    [SerializeField] private GameObject fairy;

    private Rigidbody2D rb;
    private WizardMovement wizardMove;
    private FairyMovement fairyMove;
    private SpriteRenderer wizardSpriteRenderer;
    private SpriteRenderer fairySpriteRenderer;
    private CapsuleCollider2D col;

    public static ResetManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        rb = wizard.GetComponent<Rigidbody2D>();
        col = wizard.GetComponent<CapsuleCollider2D>();

        wizardMove = wizard.GetComponent<WizardMovement>();
        fairyMove = fairy.GetComponent<FairyMovement>();

        wizardSpriteRenderer = wizard.GetComponent<SpriteRenderer>();
        fairySpriteRenderer = fairy.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    /// <summary>
    /// After unloading the last phase. Includes checking for death
    /// </summary>
    /// <param name="scene"></param>
    private void OnSceneUnloaded(Scene scene)
    {
        // If not on UI screen
        if (GameManager.s_onGameLevel)
        {
            SetLevelRelatedObjects();
            CheckOnSceneCollision();
        }
    }

    /// <summary>
    /// Here lies logic for the preservation of objects
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.s_onGameLevel)
        {
            PreserveObject();
        }
    }

    /// <summary>
    /// Brings frozen object to current scene if needed. Hides objects with same tag.
    /// </summary>
    private void PreserveObject()
    {
        // If fairy is preserving an object that is not the wizard
        if (PreserveManager.Instance.GetPreservedObject() != null && !PreserveManager.Instance.IsPreservingWizard())
        {
            // Object to hide
            string tagToFind = PreserveManager.Instance.GetPreservedObject().tag;

            // Find objects to hide
            GameObject[] dups = GameObject.FindGameObjectsWithTag(tagToFind);
            foreach (GameObject dup in dups)
            {
                // If object is not the same as preserved object
                if (GameObject.ReferenceEquals(dup, PreserveManager.Instance.GetPreservedObject()) == false)
                {
                    // Hide object
                    dup.SetActive(false);
                }
            }

            // Move preserved object to scene
            SceneManager.MoveGameObjectToScene(PreserveManager.Instance.GetPreservedObject(), SceneManager.GetSceneByName(GameManager.s_curScene));
        }
    }

    /// <summary>
    /// Changes scene to level + phase. Updates UI bar and variables in GameManager
    /// </summary>
    /// <param name="nextPhase"></param>
    /// <param name="level"></param>
    public void ChangePhase(int nextPhase, string level)
    {
        StartCoroutine(ChangeScene(nextPhase, level));
    }

    private IEnumerator ChangeScene(int nextPhase, string level)
    {
        // Disable fairy powers 
        fairyMove.DisablePower(true);

        // Name of next scene
        string next_scene = level + nextPhase;
        GameManager.s_lastScene = GameManager.s_curScene;
        GameManager.s_curScene = next_scene;

        // Update variable
        GameManager.s_curPhase = nextPhase;
        
        // Reposition wizard if needed
        if (!PreserveManager.Instance.IsPreservingWizard())
        {
            yield return StartCoroutine(CheckForReposition());
        }

        // Load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(next_scene, LoadSceneMode.Additive);
        
        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        AsyncOperation asyncUnload =  SceneManager.UnloadSceneAsync(GameManager.s_lastScene);

        // Wait until scene is unloaded
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        // UI Bar
        GameUIHandler.Instance.SetPhase(GameManager.s_curPhase);

        // Enable fairy powers 
        fairyMove.DisablePower(false);

    }

    /// <summary>
    /// Checks if wizard needs to be repositioned during scene change 
    /// and sets new position if have to
    /// </summary>
    IEnumerator CheckForReposition()
    {
        if (GameManager.s_reposition[GameManager.s_curPhase] != null)
        {
            GameManager.s_reposition[GameManager.s_curPhase].Reposition(wizard);
        }

        yield return null;
    }

    /// <summary>
    /// Checks if wizard is smacked in the face when the scene changes. 
    /// Plays Death animation if smacked.
    /// </summary>
    private void CheckOnSceneCollision()
    {
        StartCoroutine(CheckDeathCollision());

    }

    IEnumerator CheckDeathCollision()
    {
        // Check wizard collision
        GameManager.s_sceneChange = true;

        // Delay -> allow objects to check their colliders
        yield return new WaitForSeconds(0.15f);

        // Game Over
        if (GameManager.s_onDeathObject)
        {
            StartDeathAnimation(false);
        }

        // Turn off wizard collision
        GameManager.s_sceneChange = false;


        yield return null;
    }

    public void SetLevelRelatedObjects()
    {
        // Level specific code
        if (GameManager.s_level == "Dino")
        {
            // Cave
            if (!DinoManager.s_inCave)
            {
                GameObject nest = GameObject.FindGameObjectWithTag("Nest");

                if (nest != null)
                {
                    nest.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Shows all orbs
    /// </summary>
    private void ResetOrbs()
    {
        CollectableOrbs[] orbs = FindObjectsByType<CollectableOrbs>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        // Show orbs
        for (int x = 0; x < orbs.Length; x++)
        {
            orbs[x].gameObject.SetActive(true);
            GameManager.s_curOrbs[x] = false;
        }
    }

    /// <summary>
    /// Send true if fully reseting the level
    /// False resets the level to the last checkpoint and plays death animation
    /// </summary>
    /// <param name="resetLevel"></param>
    public void ResetLevel(bool resetLevel)
    {
        if (resetLevel)
        {
            StartCoroutine(RespawnWizard(resetLevel));
        }
        else
        {
            StartCoroutine(DeathAnimation(resetLevel));
        }
    }

    private void StartDeathAnimation(bool resetLevel)
    {
        StartCoroutine(DeathAnimation(resetLevel));
    }

    /// <summary>
    ///  Show death animation. After calls a function to respawn Wizard to the checkpoint. 
    /// </summary>
    /// <returns></returns>
    IEnumerator DeathAnimation(bool resetLevel)
    {
        // Pause input and reset character's variables and velocity
        wizardMove.DisableInput(true);
        fairyMove.DisablePower(true);
        fairyMove.DisableMovement(true);
        wizardMove.ResetWizard();
        fairyMove.ResetFairy();

        // CHANGE TO DEATH FACE
        wizardSpriteRenderer.color = Color.red;

        // Pauses wizard
        rb.gravityScale = 0;

        // Wait
        yield return new WaitForSeconds(0.25f);

        // Allow wizard to fall through objects
        col.enabled = false;
        rb.gravityScale = 1;

        // Launches wizard up
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Wait for wizard to drop out of screen
        yield return new WaitForSeconds(2.5f);

        yield return StartCoroutine(RespawnWizard(resetLevel));
    }


    /// <summary>
    /// Resets level fully or to the nearest checkpoint depending on parameter. 
    /// True if fully reseting a level
    /// </summary>
    /// <returns></returns>
    IEnumerator RespawnWizard(bool resetLevel)
    {
        // Scene change
        if (resetLevel)
        {
            // Back to phase 1
            yield return StartCoroutine(ChangeScene(GameManager.s_firstPhase, GameManager.s_level));
        }
        else
        {
            // Load chckpoint scene
            yield return StartCoroutine(ChangeScene(GameManager.s_checkpointPhase, GameManager.s_level));
        }

        float wizardX = resetLevel? GameManager.s_wizardResetX : GameManager.s_wizardRespawnX;
        float wizardY = resetLevel? GameManager.s_wizardResetY : GameManager.s_wizardRespawnY;

        // Move wizard to respawn point
        wizardSpriteRenderer.color = Color.white; // FACE
        wizard.transform.position = new Vector3(wizardX, wizardY, wizard.transform.position.z);

        // Move fairy to respawn point
        fairySpriteRenderer.color = Color.white;
        fairy.transform.position = new Vector3(GameManager.s_fairyResetX, GameManager.s_fairyResetY, fairy.transform.position.z);

        // Show all orbs
        if (resetLevel)
        {
            ResetOrbs();
            GameUIHandler.Instance.SetOrbCounter();
        } 

        // Enable Input and Reset back to normal
        wizardMove.DisableInput(false);
        fairyMove.DisablePower(false);
        fairyMove.DisableMovement(false);
        col.enabled = true;
        wizardMove.ResetWizard();
        fairyMove.ResetFairy();

    }
}
