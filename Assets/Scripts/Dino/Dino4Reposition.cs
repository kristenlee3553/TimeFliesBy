using UnityEngine;

public class Dino4Reposition : MonoBehaviour, IReposition
{
    private IReposition iRep;

    private bool reposition;

    private void Start()
    {
        iRep = this;
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
        Vector2 curPos = wizard.transform.position;
        Vector3 newPos = curPos;

        if (curPos.x < 0.77f && curPos.y <= 1.65f)
        {
            newPos.x = -2.83f;
            newPos.y = -3.51f;
        }
        else if (curPos.x >= 0.77f && curPos.y <= 1.65f)
        {
            newPos.x = 4.96f;
            newPos.y = -3.52f;
        }
        else if (curPos.y >= -0.49f)
        {
            newPos.x = -1.79f;
            newPos.y = 0.84f;
        }

        wizard.transform.position = newPos;
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
