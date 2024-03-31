using System;
using UnityEngine;

/// <summary>
/// Script that allows the fairy to freeze an object
/// </summary>
public class FreezeObject : MonoBehaviour
{
    private FairyMovement fairyMove;

    public static Action OnObjectRelease;
    public static Action OnObjectFreeze;

    private void Start()
    {
        fairyMove = GetComponent<FairyMovement>();
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
                PreserveManager.Instance.SetHoldingAnimation(false);

                GameObject preservedObject = PreserveManager.Instance.GetPreservedObject();

                // Wizard specific
                if (preservedObject.CompareTag("Wizard"))
                {
                    PreserveManager.Instance.SetPreservingWizard(false);
                    preservedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    preservedObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                }

                // Update objects
                PreserveManager.Instance.SetPreservableObject(preservedObject);
                PreserveManager.Instance.SetPreservedObject(null);


                // Show prompt
                preservedObject.GetComponent<PreservableObject>().ShowPrompt(true);

            }

            // If Fairy is near object that can be preserved and user chooses to preserve object
            else if (Input.GetKeyUp(KeyCode.Space) && PreserveManager.Instance.CanPreserve())
            {
                // Set hold animation
                PreserveManager.Instance.SetHoldingAnimation(true);

                GameObject preservable = PreserveManager.Instance.GetPreservableObject();

                // Wizard specific
                if (preservable.CompareTag("Wizard"))
                {
                    PreserveManager.Instance.SetPreservingWizard(true);
                    preservable.GetComponent<Rigidbody2D>().gravityScale = 0;
                    preservable.GetComponent<SpriteRenderer>().color = Color.white;
                }

                PreserveManager.Instance.SetPreservedObject(preservable);

                // Object no longer preservable
                PreserveManager.Instance.SetPreservableObject(null);

                // Hide prompt
                preservable.GetComponent<PreservableObject>().ShowPrompt(false);

            }
        }

    }
}
