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

    private void Start()
    {
        rb = wizard.GetComponent<Rigidbody2D>();
        col = wizard.GetComponent<CapsuleCollider2D>();

        wizardMove = wizard.GetComponent<WizardMovement>();
        fairyMove = fairy.GetComponent<FairyMovement>();

        wizardSpriteRenderer = wizard.GetComponent<SpriteRenderer>();
        fairySpriteRenderer = fairy.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Changes Phase based on phase set in game manager
    /// </summary>
    public void ChangePhase()
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        // Name of next scene
        string next_scene = GameManager.s_level + GameManager.s_phase.ToString();

        // Load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(next_scene, LoadSceneMode.Additive);

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // If fairy is preserving an object that is not the wizard
        if (PreserveManager.IsPreserving() && !PreserveManager.IsPreservingWizard())
        {
            // Object to hide
            string tagToFind = PreserveManager.GetPreservedObject().tag;

            // FInd objects to hide
            GameObject[] dups = GameObject.FindGameObjectsWithTag(tagToFind);
            foreach (GameObject dup in dups)
            {
                // If object is not the same as preserved object
                if (GameObject.ReferenceEquals(dup, PreserveManager.GetPreservedObject()) == false)
                {
                    // Hide object
                    dup.SetActive(false);
                }
            }

            // Move preserved object to scene
            SceneManager.MoveGameObjectToScene(PreserveManager.GetPreservedObject(), SceneManager.GetSceneByName(next_scene));

        }

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

        // Remove last scene
        SceneManager.UnloadSceneAsync(GameManager.s_currentScene);

        // Update variable
        GameManager.s_currentScene = next_scene;

        // UI Bar
        GameUIHandler.Instance.SetPhase(GameManager.s_phase);

        // Check wizard collision
        GameManager.s_sceneChange = true;

        // Delay -> allow objects to check their colliders
        yield return new WaitForSeconds(0.15f);

        // Game Over
        if (GameManager.s_onDeathObject)
        {
            ResetLevel(false);
        }

        // Turn off wizard collision
        GameManager.s_sceneChange = false;

        yield return null;
    }

    /// <summary>
    /// Send true if fully reseting the level
    /// False resets the level to the last checkpoint
    /// </summary>
    /// <param name="resetLevel"></param>
    public void ResetLevel(bool resetLevel)
    {
        if (!resetLevel)
        {
            StartCoroutine(DeathAnimation());
        }
        else
        {
            StartCoroutine(RespawnWizard(resetLevel));
        }
    }

    /// <summary>
    ///  Show death animation
    /// </summary>
    /// <returns></returns>
    IEnumerator DeathAnimation()
    {
        // Pause input and reset characters
        wizardMove.noInputWizard = true;
        fairyMove.noInputFairy = true;
        wizardMove.ResetWizard();
        fairyMove.resetFairy();

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

        StartCoroutine(RespawnWizard(false));
        yield return null;
    }


    /// <summary>
    /// Resets level fully or to the nearest checkpoint depending on parameter
    /// </summary>
    /// <returns></returns>
    IEnumerator RespawnWizard(bool resetLevel)
    {
        // Unload Scene
        SceneManager.UnloadSceneAsync(GameManager.s_currentScene);

        AsyncOperation asyncLoad;

        if (resetLevel)
        {
            // Back to phase 1
            asyncLoad = SceneManager.LoadSceneAsync(GameManager.s_level + 1, LoadSceneMode.Additive);
        }
        else
        {
            // Load new scene
            asyncLoad = SceneManager.LoadSceneAsync(GameManager.s_level + GameManager.s_checkpointPhase, LoadSceneMode.Additive);
        }

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        float wizardX = GameManager.s_wizardResetX;
        float wizardY = GameManager.s_wizardResetY;

        // Resume input
        if (!resetLevel)
        {
            wizardX = GameManager.s_wizardRespawnX;
            wizardY = GameManager.s_wizardRespawnY;

            wizardMove.ResetWizard();
            wizardMove.noInputWizard = false;
            fairyMove.noInputFairy = false;

            col.enabled = true;

            // REGULAR FACE
            wizardSpriteRenderer.color = Color.white;
        }

        // Move wizard to respawn point
        wizardSpriteRenderer.color = Color.white;
        wizard.transform.position = new Vector3(wizardX, wizardY, wizard.transform.position.z);

        // Move fairy to respawn point
        fairySpriteRenderer.color = Color.white;
        fairy.transform.position = new Vector3(GameManager.s_fairyResetX, GameManager.s_fairyResetY, fairy.transform.position.z);

        if (resetLevel)
        {
            // Orbs
            GameManager.s_curOrbs[0] = false;
            GameManager.s_curOrbs[1] = false;
            GameManager.s_curOrbs[2] = false;

            CollectableOrbs[] orbs = FindObjectsByType<CollectableOrbs>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            // Show orbs
            for (int x = 0; x < orbs.Length; x++)
            {
                orbs[x].gameObject.SetActive(true);
            }
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

        if (resetLevel)
        {
            GameManager.s_currentScene = GameManager.s_level + 1;
            GameManager.s_phase = 1;

            // UI
            GameUIHandler.Instance.SetPhase(1);
            GameUIHandler.Instance.SetOrbCounter();
        }
        else
        {
            // Update global variables
            GameManager.s_currentScene = GameManager.s_level + GameManager.s_checkpointPhase;
            GameManager.s_phase = GameManager.s_checkpointPhase;

            // UI
            GameUIHandler.Instance.SetPhase(GameManager.s_phase);
        }

    }
}
