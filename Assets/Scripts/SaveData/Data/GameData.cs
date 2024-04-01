using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    // We want to save the orbs
    public bool[] s_curOrbs = { false, false, false };

    // Default will be no orbs currently collected
    public GameData()
    {
        this.s_curOrbs[0] = false;
        this.s_curOrbs[1] = false;
        this.s_curOrbs[2] = false;
    }
}
