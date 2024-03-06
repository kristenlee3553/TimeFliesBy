using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class SceneChangeEvent : UnityEvent<SceneChangeData>
{

}

public struct SceneChangeData
{
    public int curPhase;
    public int nextPhase;
    public string levelName;
    public bool[] orbsCollected;
    public GameObject preservedObject;
}
