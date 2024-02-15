using UnityEngine;

/// <summary>
/// Script that allows the fairy to freeze an object
/// NEED TO ADD ANIMATION
/// </summary>
public class FreezeObject : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // If fairy is already preserving an object and wants to release object
        if (Input.GetKeyUp(KeyCode.Space) && PreserveManager.IsPreserving())
        {
            // Stop animation
            GetComponent<SpriteRenderer>().color = Color.white; //FILLER CODE

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
            GetComponent<SpriteRenderer>().color = Color.magenta; //FILLER CODE

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
