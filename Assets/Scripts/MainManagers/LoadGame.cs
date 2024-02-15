using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script that runs when the game is loaded
/// <br></br>
/// FUTURE: LOAD KEY BINDINGS FROM FILE + SAVED DATA
/// THIS WILL BE IN THE FIRST SCENE
/// </summary>
public class LoadGame : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("DinoBack", LoadSceneMode.Additive); // Background
        //SceneManager.LoadScene("GameScene", LoadSceneMode.Additive); // Characters

        // Set up key bindings
        // ------------ FUTURE LOAD KEY BINDINGS FROM FILE ---------------
        GameManager.s_keyBinds = new();

        GameManager.s_keyBinds.Add(GameManager.KeyBind.WizardJump, KeyCode.UpArrow);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.WizardLeft, KeyCode.LeftArrow);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.WizardRight, KeyCode.RightArrow);

        GameManager.s_keyBinds.Add(GameManager.KeyBind.FairyLeft, KeyCode.A);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.FairyUp, KeyCode.W);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.FairyRight, KeyCode.D);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.FairyDown, KeyCode.S);

        GameManager.s_keyBinds.Add(GameManager.KeyBind.MoveTimeBack, KeyCode.Q);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.MoveTimeFor, KeyCode.E);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.Preserve, KeyCode.Space);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.Interact, KeyCode.F);

        StartCoroutine(SetUpLevel());
    }

    IEnumerator SetUpLevel()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameManager.s_level + GameManager.s_phase.ToString(), LoadSceneMode.Additive); // Phase 1

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (GameManager.s_level == "Dino")
        {
            GameObject nest = GameObject.FindGameObjectWithTag("Nest");

            if (nest != null)
            {
                nest.SetActive(false);
            }
        }
    }
}
