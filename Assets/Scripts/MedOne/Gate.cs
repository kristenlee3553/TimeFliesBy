using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        if (GameManager.s_curPhase == 5)
        {
            MedOneManager.tempGateDia = true;
        }
    }

    public void RemoveInteractable()
    {
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        if (GameManager.s_curPhase == 5)
        {
            prompt.SetActive(true);
        }
    }
}
