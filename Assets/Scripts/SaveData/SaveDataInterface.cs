using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script allows other scripts to be able to implement save data
public interface SaveDataInterface
{    
    void LoadData(GameData data);

    void SaveData(ref GameData data);
}
