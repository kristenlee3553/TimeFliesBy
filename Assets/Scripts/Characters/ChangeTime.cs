using UnityEngine;

/// <summary>
/// Script that controls when wizard wants to move time back and forth
/// FUTURE: Lock powers when transitioning between UI
/// </summary>
public class ChangeTime : MonoBehaviour
{
    // Script that manages reseting of level
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
        // Wacky code in the second half.
        // There was a bug that left me scratching my head for a few hours
        // This was the solution and I'm too tired to find a more elegant way
        if (!fairyMove.IsPowerDisabled() && GameManager.s_level != "Tut" || TutorialManager.s_firstPower)
        {
            // Go back in time
            if (Input.GetKeyUp(KeyCode.Q))
            {
                // If not at first phase
                if (!MinPhase())
                {

                    if (!PreserveManager.Instance.IsPreserving())
                    {
                        animator.SetTrigger("Power");
                    }

                    // Change scene
                    StartCoroutine(ResetManager.Instance.ChangeScene(GameManager.s_curPhase - 1, GameManager.s_level));
                }

            }

            // Go Forward in Time
            if (Input.GetKeyUp(KeyCode.E))
            {
                // If not at last phase
                if (!MaxPhase())
                {
                    StartCoroutine(ResetManager.Instance.ChangeScene(GameManager.s_curPhase + 1, GameManager.s_level));

                    if (!PreserveManager.Instance.IsPreserving())
                    {
                        animator.SetTrigger("Power");
                    }
                }
            }
        }

    }

    /// <summary>
    /// Returns true if on the last phase
    /// </summary>
    /// <returns></returns>
    private bool MaxPhase()
    {
        return GameManager.s_curPhase == 5;
    }

    /// <summary>
    ///  Returns true if on the first phase
    /// </summary>
    /// <returns></returns>
    private bool MinPhase()
    {
        return GameManager.s_curPhase == 1;
    }

 
}
