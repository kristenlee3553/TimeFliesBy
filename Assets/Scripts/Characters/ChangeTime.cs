using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script that controls when wizard wants to move time back and forth
/// FUTURE: Lock powers when transitioning between UI
/// </summary>
public class ChangeTime : MonoBehaviour
{
    // Script that manages death
    private ResetManager resetManager;
    private FairyMovement fairyMove;

    private void Start()
    {
        fairyMove = GetComponent<FairyMovement>();
        resetManager = GetComponent<ResetManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fairyMove.noInputFairy)
        {
            // Go back in time
            if (Input.GetKeyUp(KeyCode.Q))
            {

                // If not at first phase
                if (!MinPhase())
                {
                    GameManager.s_phase--;
                    resetManager.ChangePhase();
                }

            }

            // Go Forward in Time
            if (Input.GetKeyUp(KeyCode.E))
            {
                // If not at last phase
                if (!MaxPhase())
                {
                    GameManager.s_phase++;
                    resetManager.ChangePhase();
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
        return GameManager.s_phase == 5;
    }

    /// <summary>
    ///  Returns true if on the first phase
    /// </summary>
    /// <returns></returns>
    private bool MinPhase()
    {
        return GameManager.s_phase == 1;
    }

 
}
