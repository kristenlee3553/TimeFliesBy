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
        StartCoroutine(StartShoot());
    }

    private IEnumerator StartShoot()
    {
        ResetManager.Instance.DisableAll(true);
        ResetManager.Instance.StopAllVelocity();
        float startAngle = 65.0f;

        ResetManager.Instance.RotateWizard(startAngle);
        ResetManager.Instance.TurnOffGravity(true);
        ResetManager.Instance.FlipWizardSprite(false);

        float newX = 5f;
        float newY = 2.8f;

        for (int i = 1; i <= 50; i++)
        {
            newX -= 0.1078f;
            newY += 0.037f;
            ResetManager.Instance.RepositionWizard(newX, newY, 0);
            ResetManager.Instance.FlipWizardSprite(false);
            yield return new WaitForSeconds(.01f);
        }

        for (int i = 1; i <= 20; i++)
        {
            startAngle -= 3.5f;
            newX -= 0.0855f;
            newY -= 0.0105f;
            ResetManager.Instance.FlipWizardSprite(false);
            ResetManager.Instance.RepositionWizard(newX, newY, 0);
            ResetManager.Instance.RotateWizard(startAngle);
            yield return new WaitForSeconds(.01f);
        }

        ResetManager.Instance.TurnOffGravity(false);
        ResetManager.Instance.DisableAll(false);

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
