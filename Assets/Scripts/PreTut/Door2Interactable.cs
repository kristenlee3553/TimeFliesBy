using UnityEngine;

public class Door2Interactable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        GameManager.s_nextPhase = 2;
        GameManager.s_lastScene = "";
        GameManager.s_firstPhase = 1;
        GameManager.s_wizardResetX = -7.56f;
        GameManager.s_wizardResetY = -3.19f;
        GameManager.s_fairyResetX = -5.86f;
        GameManager.s_fairyResetY = -2.11f;
        ResetManager.Instance.ResizeWizard(0.46f, 0.43f, 1.0f);
        ResetManager.Instance.ResizeFairy(0.79f, 0.75f, 1.0f);
        ResetManager.Instance.RepositionFairy(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);
        ResetManager.Instance.RepositionWizard(GameManager.s_wizardResetX, GameManager.s_wizardResetY, 0);

        StartCoroutine(ResetManager.Instance.ChangeScene(1, "Tut"));

    }

    public void RemoveInteractable()
    {
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        prompt.SetActive(true);
    }
}
