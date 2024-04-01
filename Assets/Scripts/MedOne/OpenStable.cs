using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        if (!MedOneManager.playStableDia)
        {
            MedOneManager.playStableDia = true;
        }
    }

    public void RemoveInteractable()
    {
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        if (MedOneManager.noHorsesInFrontOfStable)
        {
            prompt.SetActive(true);
        }
    }
}
