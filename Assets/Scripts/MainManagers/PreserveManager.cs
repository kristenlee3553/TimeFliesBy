using UnityEngine;

/// <summary>
/// This script is placed in a empty object in the GameScene.
/// <br></br>
/// It manages the preservation of objects
/// </summary>
public class PreserveManager : MonoBehaviour
{
    /// <summary>
    ///  Holds the object that is near the fairy that can be preserved
    /// </summary>
    private GameObject preservableObject = null;

    /// <summary>
    /// Object fairy is preserving
    /// </summary>
    private GameObject preservedObject = null;

    /// <summary>
    /// If wizard is frozen
    /// </summary>
    private bool isPreservingWizard = false;

    /// <summary>
    /// Original color of sprite
    /// </summary>
    private Color startcolor;

    /// <summary>
    /// Color when highlighting a sprite
    /// </summary>
    public readonly Color highlightColor = Color.yellow;

    public static PreserveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        // Script will last between scenes
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Returns if the fairy can preserve an object
    /// </summary>
    /// <returns></returns>
    public bool CanPreserve()
    {
        return preservableObject != null;
    }

    /// <summary>
    /// Returns if the fairy is preserving an object
    /// </summary>
    /// <returns></returns>
    public bool IsPreserving()
    {
        return preservedObject != null;
    }

    /// <summary>
    /// Returns true if preserved object is wizard
    /// </summary>
    /// <returns></returns>
    public bool IsPreservingWizard()
    {
        return isPreservingWizard;
    }

    /// <summary>
    /// Set if the wizard is being preserved
    /// </summary>
    /// <param name="isFrozen"></param>
    public void SetPreservingWizard(bool isFrozen)
    {
        isPreservingWizard = isFrozen;
    }

    /// <summary>
    /// Set object fairy COULD preserve right now
    /// </summary>
    /// <param name="new_object"></param>
    public void SetPreservableObject(GameObject newObject)
    {
        preservableObject = newObject;
    }

    /// <summary>
    /// Returns object fairy could be preserving
    /// </summary>
    /// <returns></returns>
    public GameObject GetPreservableObject()
    {
        return preservableObject;
    }

    /// <summary>
    /// Sets object fairy is currently preserving
    /// </summary>
    /// <param name="new_object"></param>
    public void SetPreservedObject(GameObject newobject)
    {
        preservedObject = newobject;
    }

    /// <summary>
    /// Returns object fairy is preserving
    /// </summary>
    /// <returns></returns>
    public GameObject GetPreservedObject()
    {
        return preservedObject;
    }

    /// <summary>
    /// Stores previous color of sprite before highlighting
    /// </summary>
    /// <param name="newColor"></param>
    public void SetStartColor(Color newColor)
    {
        startcolor = newColor;
    }

    /// <summary>
    /// Returns previous color of sprite before highlighting
    /// </summary>
    /// <returns></returns>
    public Color GetStartColor()
    {
        return startcolor;
    }
}
