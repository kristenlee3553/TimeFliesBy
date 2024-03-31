using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// HANDLES RESETTING OF LEVEL, CHANGING PHASES, DEATH ANIMATION.
/// HAS METHODS TO RESET AND DISABLE INPUT OF PLAYERS
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
    private Animator fairyAnim;
    private Animator wizardAnim;

    /// <summary>
    /// Level setup after generic scene change
    /// If I knew about this I would have done this earlier
    /// </summary>
    public static event Action AfterSceneLogic;

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

        fairyAnim = fairy.GetComponent<Animator>();
        wizardAnim = wizard.GetComponent<Animator>();
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
        if (GameManager.s_onGameLevel && GameManager.s_level != "PreTut")
        {
            SetLevelRelatedObjects();
            CheckOnSceneCollision();
            AfterSceneLogic?.Invoke();
        }
    }

    /// <summary>
    /// Here lies logic for the preservation of objects
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.s_onGameLevel && GameManager.s_level != "PreTut")
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
    /// <returns></returns>
    public IEnumerator ChangeScene(int nextPhase, string level)
    {
        // Disable fairy powers 
        DisablePower(true);

        // Name of next scene
        string next_scene = level + nextPhase;
        GameManager.s_lastScene = GameManager.s_curScene;
        GameManager.s_curScene = next_scene;

        // Update variable
        GameManager.s_curPhase = nextPhase;
        
        // Reposition wizard if needed
        if (!PreserveManager.Instance.IsPreservingWizard() && level != "PreTut")
        {
            yield return StartCoroutine(CheckForReposition());
        }

        // If new level
        if (level != GameManager.s_level)
        {
            AsyncOperation asyncUnload2 = SceneManager.UnloadSceneAsync(GameManager.s_level + "Back");

            // Wait until scene is unloaded
            while (!asyncUnload2.isDone)
            {
                yield return null;
            }
            GameManager.s_level = level;

            // Load new scene
            AsyncOperation asyncLoad2 = SceneManager.LoadSceneAsync(GameManager.s_level + "Back", LoadSceneMode.Additive);

            // Wait until scene is loaded
            while (!asyncLoad2.isDone)
            {
                yield return null;
            }
        }

        // Load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(next_scene, LoadSceneMode.Additive);

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(GameManager.s_lastScene);

        // Wait until scene is unloaded
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        // UI Bar
        GameUIHandler.Instance.SetPhase(GameManager.s_curPhase);

        HandlePowerDisabling();

    }

    /// <summary>
    /// Whether to enable or disable fairy power
    /// </summary>
    public void HandlePowerDisabling()
    {
        // Tutorial code (bad code cuz can make it can be more concise)
        if (GameManager.s_level == "Tut" && TutorialManager.s_disablePower || GameManager.s_level == "PreTut")
        {
            DisablePower(true);
        }
        else
        {
            // Enable fairy powers 
            DisablePower(false);
        }
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

        ResetRepositionArray();

        yield return null;
    }

    private void ResetRepositionArray()
    {
        for (int i = 0; i < GameManager.s_reposition.Length; i++)
        {
            GameManager.s_reposition[i] = null;
        }
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
        else if (GameManager.s_level == "Tut")
        {
            // Wizard has seed -> forever hide the seed
            if (TutorialManager.s_hasSeed && GameManager.s_curPhase == 1)
            {
                GameObject seed = GameObject.FindGameObjectWithTag("Seed");

                if (seed != null)
                {
                    seed.SetActive(false);
                }
            }

            // Scene change, so no more apple 
            TutorialManager.s_hasRipeApple = false;
            TutorialManager.s_hasUnripeApple = false;
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

    public void StartDeathAnimation(bool resetLevel)
    {
        StartCoroutine(DeathAnimation(resetLevel));
    }

    /// <summary>
    ///  Show death animation. After calls a function to respawn Wizard to the checkpoint. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathAnimation(bool resetLevel)
    {
        // Pause input and reset character's variables and velocity
        DisableWizardInput(true);
        DisablePower(true);
        DisableFairyMovement(true);
        wizardMove.ResetWizard();
        fairyMove.ResetFairy();

        // Pauses wizard
        rb.gravityScale = 0;

        // Death face
        wizardSpriteRenderer.color = Color.white;
        wizardAnim.SetBool("Dead", true);

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
    private IEnumerator RespawnWizard(bool resetLevel)
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
        wizardAnim.SetBool("Dead", false);
        wizardSpriteRenderer.color = Color.white;
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
        HandlePowerDisabling();
        DisableFairyMovement(false);
        DisableWizardInput(false);
        col.enabled = true;
        wizardMove.ResetWizard();
        fairyMove.ResetFairy();
    }

    /// <summary>
    /// Disables time traversal and freezing mechanic
    /// </summary>
    /// <param name="disable"></param>
    public void DisablePower(bool disable)
    {
        fairyMove.DisablePower(disable);
    }

    /// <summary>
    /// Disables wizard's ability to jump and move
    /// </summary>
    /// <param name="disable"></param>
    public void DisableWizardInput(bool disable)
    {
        wizardMove.DisableInput(disable);
    }

    /// <summary>
    /// Disables fairy movement
    /// </summary>
    /// <param name="disable"></param>
    public void DisableFairyMovement(bool disable)
    {
        fairyMove.DisableMovement(disable);
    }

    /// <summary>
    /// Disables all player input
    /// </summary>
    /// <param name="disable"></param>
    public void DisableAll(bool disable)
    {
        fairyMove.DisableMovement(disable);
        wizardMove.DisableInput(disable);
        fairyMove.DisablePower(disable);
    }

    /// <summary>
    /// Returns if the fairy's power is disabled
    /// </summary>
    /// <returns></returns>
    public bool IsPowerDisabled()
    {
        return fairyMove.IsPowerDisabled();
    }

    /// <summary>
    /// Stops velocity of wizard and fairy.
    /// </summary>
    public void StopAllVelocity()
    {
        wizardMove.StopVelocity();
        fairyMove.StopFairyVelocity();
    }

    /// <summary>
    /// Triggers Fairy Power animation
    /// </summary>
    public void TriggerTimeAnimation()
    {
        fairyAnim.SetTrigger("Power");
    }

    /// <summary>
    /// Resizes wizard
    /// </summary>
    public void ResizeWizard(float x, float y, float z)
    {
        wizardMove.ResizeWizard(x, y, z);
    }

    public void RepositionWizard(float x, float y, float z)
    {
        wizardMove.RepositionWizard(x, y, z);
    }

    public void ResizeFairy(float x, float y, float z)
    {
        fairyMove.ResizeFairy(x, y, z);
    }

    public void RepositionFairy(float x, float y, float z)
    {
        fairyMove.RepositionFairy(x, y, z);
    }
    
    public void RotateWizard(float degrees)
    {
        wizard.transform.rotation = Quaternion.Euler(Vector3.forward * degrees);
    }

    public void TurnOffGravity(bool turnOff)
    {
        rb.gravityScale = turnOff ? 0 : 1;
    }

    public bool GetWizardFacingDirection()
    {
        return wizardSpriteRenderer.flipX;
    }

    public void FlipWizardSprite(bool flipX)
    {
        wizardSpriteRenderer.flipX = flipX;
    }
}
