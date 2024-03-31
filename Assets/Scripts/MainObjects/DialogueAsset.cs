using UnityEngine;

/// <summary>
/// For dialogues. Create in Assets like any other GameObject.
/// Edit dialogues in the inspector.
/// </summary>
[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    public MyDialogue[] dialogue;
}

[System.Serializable]
public struct MyDialogue
{
    [TextArea]
    public string dialogue;

    /// <summary>
    /// Use Wizard, Fairy, TimeLord
    /// </summary>
    public string name;

    public bool checkpoint;
}
