using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;
    private SpriteRenderer spriteRenderer;
    private Color startColor;

    public void Interact()
    {
        prompt.SetActive(false);
    }

    public void RemoveInteractable()
    {
        spriteRenderer.color = startColor;
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        spriteRenderer.color = GameManager.s_interactColor;
        prompt.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }
}
