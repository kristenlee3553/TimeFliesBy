using UnityEngine;

/// <summary>
/// WHERE USER COMPLETS THE LEVEL
/// FUTURE -> SAVE GAME DATA
/// </summary>
public class Portal : MonoBehaviour, IInteractable
{
    private Color startColor;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    public void Interact()
    {
        Debug.Log("You Passed!");

        int numOrbs = 0;

        foreach (bool orb in GameManager.s_curOrbs) 
        {
            if (orb)
            {
                numOrbs++;
            }
        }

        Debug.Log("You Collected " + numOrbs + " orbs!");
    }

    public void RemoveInteractable()
    {
        spriteRenderer.color = startColor;
    }

    public void ShowInteractable()
    {
        spriteRenderer.color = GameManager.s_interactColor;
    }

}
