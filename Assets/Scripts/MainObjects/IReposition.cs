using UnityEngine;
/// <summary>
/// Interface for objects that cause the wizard to reposition rather than die.
/// Need to create new script that implemets this interface 
/// and attach script to the object that repositions the wizard rather than kills them.
/// See Dino2Reposition for coding template.
/// </summary>
public interface IReposition
{
    void Reposition(GameObject wizard);
}
