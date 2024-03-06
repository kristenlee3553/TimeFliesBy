using UnityEngine;

/// <summary>
/// Script that allows the fairy to freeze an object
/// </summary>
public class FreezeObject : MonoBehaviour
{
    private FairyMovement fairyMove;
    private Animator animator;

    private void Start()
    {
        fairyMove = GetComponent<FairyMovement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Can use power as long as power is not disabled
        if (!fairyMove.IsPowerDisabled())
        {
            // If fairy is already preserving an object and wants to release object
            if (Input.GetKeyUp(KeyCode.Space) && PreserveManager.Instance.IsPreserving())
            {
                // Stop animation
                animator.SetBool("Holding", false);

                // Wizard specific
                if (PreserveManager.Instance.GetPreservedObject().CompareTag("Wizard"))
                {
                    PreserveManager.Instance.SetPreservingWizard(false);
                    PreserveManager.Instance.GetPreservedObject().GetComponent<Rigidbody2D>().gravityScale = 1;
                }

                // Highlight color
                PreserveManager.Instance.GetPreservedObject().GetComponent<SpriteRenderer>().color = PreserveManager.Instance.highlightColor;

                // Update objects
                PreserveManager.Instance.SetPreservableObject(PreserveManager.Instance.GetPreservedObject());
                PreserveManager.Instance.SetPreservedObject(null);

            }

            // If Fairy is near object that can be preserved and user chooses to preserve object
            else if (Input.GetKeyUp(KeyCode.Space) && PreserveManager.Instance.CanPreserve())
            {
                // Set hold animation
                animator.SetBool("Holding", true);

                // Wizard specific
                if (PreserveManager.Instance.GetPreservableObject().CompareTag("Wizard"))
                {
                    PreserveManager.Instance.SetPreservingWizard(true);
                    PreserveManager.Instance.GetPreservableObject().GetComponent<Rigidbody2D>().gravityScale = 0;
                }

                PreserveManager.Instance.SetPreservedObject(PreserveManager.Instance.GetPreservableObject());

                // Object no longer preservable
                PreserveManager.Instance.SetPreservableObject(null);

                // Remove highlight color
                PreserveManager.Instance.GetPreservedObject().GetComponent<SpriteRenderer>().color = PreserveManager.Instance.GetStartColor();

            }
        }

    }
}
