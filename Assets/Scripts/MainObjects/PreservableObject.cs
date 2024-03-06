using UnityEngine;

/// <summary>
/// Place this script on objects that can be preserved
/// <br></br>
/// Objects must have collider
/// </summary>
public class Preserve : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fairy"))
        {
            // Change color
            PreserveManager.Instance.SetStartColor(this.GetComponent<SpriteRenderer>().color);
            this.GetComponent<SpriteRenderer>().color = PreserveManager.Instance.highlightColor;

            // Set object fairy can preserve
            PreserveManager.Instance.SetPreservableObject(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Fairy"))
        {
            // Revert to original color
            this.GetComponent<SpriteRenderer>().color = PreserveManager.Instance.GetStartColor();

            // No longer able to preserve an object
            PreserveManager.Instance.SetPreservableObject(null);
        }
    }
}
