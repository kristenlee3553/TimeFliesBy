using UnityEngine;

public class TreeReposition3 : MonoBehaviour, IReposition
{
    private IReposition iRep;

    private void Start()
    {
        iRep = this;
    }

    public void Reposition(GameObject wizard)
    {
        Vector2 curPos = wizard.transform.position;
        Vector3 newPosition = new(1.777f, -3.319f, 0);
        if (curPos.x < 0)
        {
            newPosition.x = -1.79f;
            newPosition.y = -3.3f;
        }

        wizard.transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            GameManager.s_reposition[4] = iRep;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            GameManager.s_reposition[4] = iRep;
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
