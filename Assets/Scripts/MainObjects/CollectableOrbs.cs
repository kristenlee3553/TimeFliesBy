using UnityEngine;
using System.Collections;

/// <summary>
/// Makes orbs collectible
/// </summary>
public class CollectableOrbs : MonoBehaviour
{
    [SerializeField] private AudioSource soundSource;

    private SpriteRenderer sprite;
    private bool isPlaying = false;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only wizard
        if (collision.CompareTag("Wizard") && !isPlaying)
        {
            isPlaying = true;
            StartCoroutine(PlaySound());
        }
    }

    private IEnumerator PlaySound()
    {
        // Get orb number
        string orbNum = transform.tag.Replace("Orb", "");
        int index = int.Parse(orbNum);

        // Obtained an orb
        GameManager.s_curOrbs[index] = true;

        sprite.color = new(255, 255, 255, 0);

        // UI
        GameUIHandler.Instance.SetOrbCounter();

        soundSource.Play();

        //Wait until it's done playing
        while (soundSource.isPlaying)
            yield return null;

        isPlaying = false;
        sprite.color = new(255, 255, 255, 255);
        // Hide from scene
        transform.gameObject.SetActive(false);
    }
}
