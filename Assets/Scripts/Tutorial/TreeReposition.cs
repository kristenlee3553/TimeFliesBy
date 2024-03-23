using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeReposition : MonoBehaviour, IReposition
{
    private IReposition iRep;
    private BoxCollider2D collider;

    private void Start()
    {
        iRep = this;
        collider = GetComponent<BoxCollider2D>();
    }

    public void Reposition(GameObject wizard)
    {
        Vector2 closestPoint = collider.ClosestPoint(wizard.transform.position);
        closestPoint.y += 0.5f;

        wizard.transform.position = closestPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            GameManager.s_reposition[3] = iRep;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            GameManager.s_reposition[3] = iRep;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard") && GameManager.wizardCollisions != 1)
        {
            GameManager.s_reposition[3] = null;
        }
    }
}
