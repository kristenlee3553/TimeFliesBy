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
    private static GameObject preservableObject = null;

    /// <summary>
    /// Object fairy is preserving
    /// </summary>
    private static GameObject preservedObject = null;

    /// <summary>
    /// If wizard is frozen
    /// </summary>
    private static bool isPreservingWizard = false;

    /// <summary>
    /// Original color of sprite
    /// </summary>
    private static Color startcolor;

    /// <summary>
    /// Color when highlighting a sprite
    /// </summary>
    public static readonly Color highlightColor = Color.yellow;

    private void Awake()
    {
        // Script will last between scenes
        DontDestroyOnLoad(transform.gameObject);
    }

    /// <summary>
    /// Returns if the fairy can preserve an object
    /// </summary>
    /// <returns></returns>
    public static bool CanPreserve()
    {
        return preservableObject != null;
    }

    /// <summary>
    /// Returns if the fairy is preserving an object
    /// </summary>
    /// <returns></returns>
    public static bool IsPreserving()
    {
        return preservedObject != null;
    }

    /// <summary>
    /// Returns true if preserved object is wizard
    /// </summary>
    /// <returns></returns>
    public static bool IsPreservingWizard()
    {
        return isPreservingWizard;
    }

    /// <summary>
    /// Set if the wizard is being preserved
    /// </summary>
    /// <param name="isFrozen"></param>
    public static void SetPreservingWizard(bool isFrozen)
    {
        isPreservingWizard = isFrozen;
    }

    /// <summary>
    /// Set object fairy COULD preserve right now
    /// </summary>
    /// <param name="new_object"></param>
    public static void SetPreservableObject(GameObject newObject)
    {
        preservableObject = newObject;
    }

    /// <summary>
    /// Returns object fairy could be preserving
    /// </summary>
    /// <returns></returns>
    public static GameObject GetPreservableObject()
    {
        return preservableObject;
    }

    /// <summary>
    /// Sets object fairy is currently preserving
    /// </summary>
    /// <param name="new_object"></param>
    public static void SetPreservedObject(GameObject newobject)
    {
        preservedObject = newobject;
    }

    /// <summary>
    /// Returns object fairy is preserving
    /// </summary>
    /// <returns></returns>
    public static GameObject GetPreservedObject()
    {
        return preservedObject;
    }

    /// <summary>
    /// Stores previous color of sprite before highlighting
    /// </summary>
    /// <param name="newColor"></param>
    public static void SetStartColor(Color newColor)
    {
        startcolor = newColor;
    }

    /// <summary>
    /// Returns previous color of sprite before highlighting
    /// </summary>
    /// <returns></returns>
    public static Color GetStartColor()
    {
        return startcolor;
    }
}
