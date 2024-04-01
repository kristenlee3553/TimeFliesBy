using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavesMenu : MonoBehaviour
{
    [Header("Save Buttons")]
    [SerializeField] private Button save1Button;
    [SerializeField] private Button save2Button;
    [SerializeField] private Button save3Button;

    private SaveSlots[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlots>();
    }

    private void Start()
    {
        this.ActivateMenu();
    }

    public void ActivateMenu()
    {
        // Load all the saves that exist
        Dictionary<string, GameData> savesGameData = SaveDataManager.instance.GetAllSavesGameData();

        // Loop through each save slot and set UI accordingly
        foreach (SaveSlots saveSlot in saveSlots)
        {
            GameData saveData = null;
            savesGameData.TryGetValue(saveSlot.GetSaveId(), out saveData);
            saveSlot.SetData(saveData);
        }
    }

    public void OnSaveSlotClicked(SaveSlots saveSlot)
    {
        // Update selected save ID
        SaveDataManager.instance.ChangeSelectedSaveId(saveSlot.GetSaveId());

        // Create new game if the save slot has no data
        if (!SaveDataManager.instance.HasGameData())
        {
            SaveDataManager.instance.NewGame();
        }
        else
        {
            SaveDataManager.instance.LoadGame();
        }

        // Load level select scene
        SceneManager.LoadScene("LevelSelect");
    }
}
