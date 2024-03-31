using UnityEngine;

public class Alligator : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            ResetManager.Instance.StartDeathAnimation(false);
        }
    }
}
