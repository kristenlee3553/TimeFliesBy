using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script that controls when wizard wants to move time back and forth
/// FUTURE: Lock powers when transitioning between UI
/// </summary>
public class ChangeTime : MonoBehaviour
{
    // Script that manages death
    private Death deathScript;

    private void Start()
    {
        deathScript = GetComponent<Death>();
    }

    // Update is called once per frame
    void Update()
    {
        // Go back in time
        if (Input.GetKeyUp(KeyCode.Q))
        {

            // If not at first phase
            if (!MinPhase())
            {
                GameManager.s_phase--;
                StartCoroutine(ChangeScene());
            }

        }

        // Go Forward in Time
        if (Input.GetKeyUp(KeyCode.E))
        {
            // If not at last phase
            if (!MaxPhase())
            {
                GameManager.s_phase++;
                StartCoroutine(ChangeScene());
            }
        }

    }

    /// <summary>
    /// Returns true if on the last phase
    /// </summary>
    /// <returns></returns>
    private bool MaxPhase()
    {
        return GameManager.s_phase == 5;
    }

    /// <summary>
    ///  Returns true if on the first phase
    /// </summary>
    /// <returns></returns>
    private bool MinPhase()
    {
        return GameManager.s_phase == 1;
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

        GameUIHandler.Instance.SetPhase(GameManager.s_phase);

        // Game Over
        if (GameManager.s_onMoveableObject)
        {
            deathScript.Die();
        }
    }
}
