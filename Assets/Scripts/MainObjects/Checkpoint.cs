using UnityEngine;

/// <summary>
/// Sets checkpoint when wizard enters
/// </summary>
public class Checkpoint : MonoBehaviour
{
    /// <summary>
    /// Empty Game Object that is the location where wizard will respawn
    /// </summary>
    [SerializeField] private Transform checkpoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Set checkpoint for wizard
        if (collision.gameObject.CompareTag("Wizard"))
        {
            GameManager.s_wizardRespawnX = checkpoint.position.x;
            GameManager.s_wizardRespawnY = checkpoint.position.y;
            GameManager.s_checkpointPhase = GameManager.s_curPhase;
        }
    }
}
