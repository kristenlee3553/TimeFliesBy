using UnityEngine;

/// <summary>
/// Makes orbs collectible
/// </summary>
public class CollectableOrbs : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only wizard
        if (collision.CompareTag("Wizard"))
        {
            // Get orb number
            string orbNum = transform.tag.Replace("Orb", "");
            int index = int.Parse(orbNum);

            // Obtained an orb
            GameManager.s_curOrbs[index] = true;

            // Hide from scene
            transform.gameObject.SetActive(false);
        }
    }
}
