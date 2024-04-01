using UnityEngine;

public class TowerReposition : MonoBehaviour, IReposition
{
    private IReposition iRep;

    private void Start()
    {
        iRep = this;
    }

    public void Reposition(GameObject wizard)
    {
        wizard.transform.position = new(5.08f, 2.92f, 0);
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
