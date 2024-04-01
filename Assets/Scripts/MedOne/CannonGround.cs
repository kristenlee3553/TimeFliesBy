using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonGround : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;
    private SpriteRenderer spriteRenderer;
    private Color startColor;

    public void Interact()
    {
        prompt.SetActive(false);

        if (GameManager.s_curPhase == 3)
        {
            StartCoroutine(StartDeathShoot());
        } 
        else
        {
            StartCoroutine(StartShoot());
        }
    }

    /// <summary>
    /// Wizard dies to cannon ball
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDeathShoot()
    {
        ResetManager.Instance.DisableAll(true);
        ResetManager.Instance.StopAllVelocity();

        float startAngle = -40.0f;

        ResetManager.Instance.RotateWizard(startAngle);
        ResetManager.Instance.TurnOffGravity(true);

        float newX = -1.56f;
        float newY = -2.81f;

        for (int i = 1; i <= 25; i++)
        {
            newX += 0.1044f;
            newY += 0.1188f;
            ResetManager.Instance.RepositionWizard(newX, newY, 0);
            yield return new WaitForSeconds(.01f);
        }

        // Die to cannon ball
        ResetManager.Instance.StartDeathAnimation(false);
    }
    
    /// <summary>
    /// Wizard lives. Ending spot is castle top
    /// </summary>
    /// <returns></returns>
    IEnumerator StartShoot()
    {
        ResetManager.Instance.DisableAll(true);
        ResetManager.Instance.StopAllVelocity();

        float startAngle = -40.0f;

        ResetManager.Instance.RotateWizard(startAngle);
        ResetManager.Instance.TurnOffGravity(true);

        float newX = -1.56f;
        float newY = -2.81f;

        for (int i = 1; i <= 100; i ++)
        {
            newX += 0.0568f;
            newY += 0.0635f;
            ResetManager.Instance.RepositionWizard(newX, newY, 0);
            yield return new WaitForSeconds(.01f);
        }

        for (int i = 1; i <= 20; i++)
        {
            startAngle += 2f;
            newX += 0.047f;
            newY -= 0.033f;
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
