using UnityEngine;
using UnityEngine.UIElements;

public class GameUIHandler : MonoBehaviour
{
    /// <summary>
    /// UI that tells player what phase.
    /// Not sure why there is an m.
    /// Unity tutorial had that there.
    /// </summary>
    private VisualElement m_Timebar;
    
    /// <summary>
    /// So that other classes can call methods here using the class name
    /// </summary>
    public static GameUIHandler Instance { get; private set; }

    // Awake is called when the script instance is being loaded (in this situation, when the game scene loads)
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Timebar = uiDocument.rootVisualElement.Q<VisualElement>("TimeBar");
        SetPhase(1);
    }

    /// <summary>
    /// Updates Time Tracker Bar
    /// </summary>
    /// <param name="phase"></param>
    public void SetPhase(int phase)
    {
        float percent = (phase - 1) / 4.0f;
        m_Timebar.style.width = Length.Percent(100 * percent);

    }
}
