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
        if (!fairyMove.noInputFairy)
        {
            // If fairy is already preserving an object and wants to release object
            if (Input.GetKeyUp(KeyCode.Space) && PreserveManager.IsPreserving())
            {
                // Stop animation
                animator.SetBool("Holding", false);

                // Wizard specific
                if (PreserveManager.GetPreservedObject().CompareTag("Wizard"))
                {
                    PreserveManager.SetPreservingWizard(false);
                    PreserveManager.GetPreservedObject().GetComponent<Rigidbody2D>().gravityScale = 1;
                }

                // Highlight color
                PreserveManager.GetPreservedObject().GetComponent<SpriteRenderer>().color = PreserveManager.highlightColor;

                // Update objects
                PreserveManager.SetPreservableObject(PreserveManager.GetPreservedObject());
                PreserveManager.SetPreservedObject(null);

            }

            // If Fairy is near object that can be preserved and user chooses to preserve object
            else if (Input.GetKeyUp(KeyCode.Space) && PreserveManager.CanPreserve())
            {
                // Set hold animation
                animator.SetBool("Holding", true);

                // Wizard specific
                if (PreserveManager.GetPreservableObject().CompareTag("Wizard"))
                {
                    PreserveManager.SetPreservingWizard(true);
                    PreserveManager.GetPreservableObject().GetComponent<Rigidbody2D>().gravityScale = 0;
                }

                // Set object fairy is preserving
                PreserveManager.SetPreservedObject(PreserveManager.GetPreservableObject());

                // Object no longer preservable
                PreserveManager.SetPreservableObject(null);

                // Remove highlight color
                PreserveManager.GetPreservedObject().GetComponent<SpriteRenderer>().color = PreserveManager.GetStartColor();

            }
        }

    }
}
