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
        // Load backgrounds according to which level it is
        if (GameManager.s_level == "PreTut")
        {
            SceneManager.LoadScene("PreTutBack", LoadSceneMode.Additive);
        }
        else if (GameManager.s_level == "Dino")
        {
            SceneManager.LoadScene("DinoBack", LoadSceneMode.Additive);
        }
        else if (GameManager.s_level == "MedOne")
        {
            SceneManager.LoadScene("MedOneBack", LoadSceneMode.Additive);
        }

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
        GameManager.s_keyBinds.Add(GameManager.KeyBind.Interact, KeyCode.P);

        StartCoroutine(SetUpLevel());
    }

    IEnumerator SetUpLevel()
    {
        // Configure scene according to which level it is
        if (GameManager.s_level == "PreTut")
        {
            GameManager.s_level = "PreTut";
            GameManager.s_curScene = "PreTut1";
            GameManager.s_firstPhase = 1;
            GameManager.s_curPhase = 1;
            GameManager.s_wizardResetX = -7.7f;
            GameManager.s_wizardResetY = -3.1f;
            GameManager.s_fairyResetX = -6.08f;
            GameManager.s_fairyResetY = -2.63f;
        }
        else if (GameManager.s_level == "Dino")
        {
            GameManager.s_level = "Dino";
            GameManager.s_curScene = "Dino1";
            GameManager.s_firstPhase = 1;
            GameManager.s_curPhase = 1;
            GameManager.s_wizardResetX = -7.53f;
            GameManager.s_wizardResetY = 1.07f;
            GameManager.s_wizardRespawnX = -7.53f;
            GameManager.s_wizardRespawnY = 1.07f;
            GameManager.s_fairyResetX = -4.29f;
            GameManager.s_fairyResetY = -2.56f;
        }
        else if (GameManager.s_level == "MedOne")
        {
            GameManager.s_level = "MedOne";
            GameManager.s_curScene = "MedOne1";
            GameManager.s_firstPhase = 1;
            GameManager.s_curPhase = 1;
            GameManager.s_wizardResetX = -3.78f;
            GameManager.s_wizardResetY = -2.6f;
            GameManager.s_wizardRespawnX = -3.78f;
            GameManager.s_wizardRespawnY = -2.6f;
            GameManager.s_fairyResetX = -2.83f;
            GameManager.s_fairyResetY = -1.22f;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameManager.s_level + GameManager.s_curPhase.ToString(), LoadSceneMode.Additive); // Phase 1

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Configure scene according to which level it is
        if (GameManager.s_level == "PreTut")
        {
            ResetManager.Instance.SetLevelRelatedObjects();
            ResetManager.Instance.ResizeWizard(1.15f, 1.08f, 1);
            ResetManager.Instance.ResizeFairy(1.35f, 1.35f, 1);
            ResetManager.Instance.RepositionFairy(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);
            ResetManager.Instance.RepositionWizard(GameManager.s_wizardResetX, GameManager.s_wizardResetY, 0);

            GameUIHandler.Instance.HideOrbDisplay();
            GameUIHandler.Instance.HideTimeBar();
        }
        else if (GameManager.s_level == "Dino")
        {
            ResetManager.Instance.SetLevelRelatedObjects();
            ResetManager.Instance.ResizeWizard(0.54f, 0.48f, 1.0f);
            ResetManager.Instance.ResizeFairy(0.87f, 0.74f, 1.0f);
            ResetManager.Instance.RepositionFairy(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);
            ResetManager.Instance.RepositionWizard(GameManager.s_wizardResetX, GameManager.s_wizardResetY, 0);
        }
        else if (GameManager.s_level == "MedOne")
        {
            ResetManager.Instance.SetLevelRelatedObjects();
            ResetManager.Instance.ResizeWizard(0.54f, 0.48f, 1.0f);
            ResetManager.Instance.ResizeFairy(0.87f, 0.74f, 1.0f);
            ResetManager.Instance.RepositionFairy(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);
            ResetManager.Instance.RepositionWizard(GameManager.s_wizardResetX, GameManager.s_wizardResetY, 0);
        }
    }
}
