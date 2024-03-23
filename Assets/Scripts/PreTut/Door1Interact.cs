using UnityEngine;

public class Door1Interact : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        GameManager.s_firstPhase = 2;
        GameManager.s_wizardResetX = -7.59f;
        GameManager.s_wizardResetY = -1.91f;
        GameManager.s_fairyResetX = -6.2034f;
        GameManager.s_fairyResetY = -1.0967f;
        ResetManager.Instance.RepositionFairy(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);
        ResetManager.Instance.RepositionWizard(GameManager.s_wizardResetX, GameManager.s_wizardResetY, 0);

        StartCoroutine(ResetManager.Instance.ChangeScene(2, "PreTut"));
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
