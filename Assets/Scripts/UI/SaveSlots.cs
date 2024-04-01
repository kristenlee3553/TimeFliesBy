using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlots : MonoBehaviour
{
    [Header("Save")]
    [SerializeField] private string saveId = "";

    [Header("Content")]
    [SerializeField] private GameObject newGameContent;
    [SerializeField] private GameObject hasGameContent;
    [SerializeField] private TextMeshProUGUI levelNameText;

    public void SetData(GameData data)
    {
        // Display New Game button if there is no data for the save slot
        if (data == null)
        {
            newGameContent.SetActive(true);
            hasGameContent.SetActive(false);
        }
        // Display the saved level is there is data for the save slot
        else
        {
            newGameContent.SetActive(false);
            hasGameContent.SetActive(true);

            levelNameText.text = "Tutorial"; // PLACEHOLDER
        }
    }

    public string GetSaveId()
    {
        return this.saveId;
    }
}
