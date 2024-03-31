using UnityEngine;

/// <summary>
/// WHERE USER COMPLETES THE LEVEL
/// FUTURE -> SAVE GAME DATA
/// </summary>
public class Portal : MonoBehaviour, IInteractable
{
    private Color startColor;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject prompt;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    // SAVE DATA CODE GOES HERE
    public void Interact()
    {
        Debug.Log("You Passed!");

        // Count orbs
        int numOrbs = 0;

        foreach (bool orb in GameManager.s_curOrbs) 
        {
            if (orb)
            {
                numOrbs++;
            }
        }

        Debug.Log("You Collected " + numOrbs + " orbs!");

        // Hide prompt
        prompt.SetActive(false);

        // Will delete this later
        DinoManager.levelOver = true;
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

}
