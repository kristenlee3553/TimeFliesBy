using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            ResetManager.Instance.StartDeathAnimation(false);
        }
    }
}
