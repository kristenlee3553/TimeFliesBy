using UnityEngine;

/// <summary>
/// Attach to Moveable Objects. If wizard is on a moveable object when time changes -> death
/// </summary>
public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            // Fairy is preserving an object that is not the wizard
            if (PreserveManager.IsPreserving() && !PreserveManager.IsPreservingWizard())
            {
                // If object fairy is preserving is not the object the wizard is standing on
                if (!ReferenceEquals(transform.gameObject, PreserveManager.GetPreservedObject()))
                {
                    GameManager.s_onMoveableObject = true;
                }
                else
                {
                    // Wizard will not die because fairy is preserving object
                    GameManager.s_onMoveableObject = false;
                }
            }
            // Wizard will not die if being preserved by fairy
            else if (PreserveManager.IsPreservingWizard())
            {
                GameManager.s_onMoveableObject = false;
            }

            // If on movemable object
            else
            {
                GameManager.s_onMoveableObject = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard") && GameManager.s_onMoveableObject == false)
        {   
            // Fairy is preserving an object that is not the wizard
            if (PreserveManager.IsPreserving() && !PreserveManager.IsPreservingWizard())
            {
                // If object fairy is preserving is not the object the wizard is standing on
                if (!ReferenceEquals(transform.gameObject, PreserveManager.GetPreservedObject()))
                {
                    GameManager.s_onMoveableObject = true;
                }
                else
                {
                    // Wizard will not die because fairy is preserving object
                    GameManager.s_onMoveableObject = false;
                }
            }
            // Wizard will not die if being preserved by fairy
            else if (PreserveManager.IsPreservingWizard())
            {
                GameManager.s_onMoveableObject = false;
            }

            // If on movemable object
            else
            {
                GameManager.s_onMoveableObject = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            GameManager.s_onMoveableObject = false;
        }
    }
}
