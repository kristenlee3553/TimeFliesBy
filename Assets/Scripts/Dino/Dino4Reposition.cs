using UnityEngine;

public class Dino4Reposition : MonoBehaviour, IReposition
{
    private IReposition iRep;
    private EdgeCollider2D collider;

    private bool reposition;

    private void Start()
    {
        iRep = this;
        collider = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        // If fairy is preserving
        if (PreserveManager.Instance.IsPreserving() && !PreserveManager.Instance.IsPreservingWizard())
        {
            // Check if object being preserved is the one we are currently on
            if (PreserveManager.Instance.GetPreservedObject().CompareTag("Dino"))
            {
                reposition = false;
            }
        }
        else
        {
            reposition = true;
        }
    }

    public void Reposition(GameObject wizard)
    {
        Vector2 closestPoint = collider.ClosestPoint(wizard.transform.position);
        closestPoint.y += 1.5f;

        wizard.transform.position = closestPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard") && reposition)
        {
            GameManager.s_reposition[4] = iRep;
        }

        // No need to reposition cuz object is being preserved
        if (collision.gameObject.CompareTag("Wizard") && !reposition)
        {
            GameManager.s_reposition[4] = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard") && reposition)
        {
            GameManager.s_reposition[4] = iRep;
        }

        // No need to reposition cuz object is being preserved
        if (collision.gameObject.CompareTag("Wizard") && !reposition)
        {
            GameManager.s_reposition[4] = null;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard") && GameManager.wizardCollisions != 1)
        {
            GameManager.s_reposition[4] = null;
        }
    }
}
