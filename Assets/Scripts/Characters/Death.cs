using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public void Die()
    {
        // Death animation
        Debug.Log("You died");

        StartCoroutine(UnloadScene());
        StartCoroutine(ResetGame());
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


    /// <summary>
    /// Resets game to wizard's respawn point
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetGame()
    {
        // Load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameManager.s_level + GameManager.s_checkpointPhase, LoadSceneMode.Additive);

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move wizard to respawn point
        GameObject wizard = GameObject.FindWithTag("Wizard");

        if (wizard != null)
        {
            wizard.GetComponent<SpriteRenderer>().color = Color.white;
            wizard.transform.position = new Vector3(GameManager.s_wizardRespawnX, GameManager.s_wizardRespawnY, wizard.transform.position.z);

        }

        // Move fairy to respawn point
        GameObject fairy = GameObject.FindWithTag("Fairy");

        if (fairy != null)
        {
            fairy.GetComponent<SpriteRenderer>().color = Color.white;
            fairy.transform.position = new Vector3(GameManager.s_fairyResetX, GameManager.s_fairyResetY, fairy.transform.position.z);
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

        GameManager.s_currentScene = GameManager.s_level + GameManager.s_checkpointPhase;
        GameManager.s_phase = GameManager.s_checkpointPhase;

        // UI
        GameUIHandler.Instance.SetPhase(GameManager.s_phase);

    }
}
