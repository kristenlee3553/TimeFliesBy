using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    // Where we want the save file to be saved to
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string saveId)
    {
        string fullPath = Path.Combine(dataDirPath, saveId, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Load JSON data from file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Convert from JSON to game data
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string saveId)
    {
        string fullPath = Path.Combine(dataDirPath, saveId, dataFileName);
        try
        {
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Convert game data into JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write JSON data into file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllSaves()
    {
        Dictionary<string, GameData> saveDictionary = new Dictionary<string, GameData>();

        // Loop over all directory names 
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string saveId = dirInfo.Name;

            // Skip over non save data files
            string fullPath = Path.Combine(dataDirPath, saveId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all saves because it does not contain data: " + saveId);
                continue;
            }

            // Load game data for a saveId and add to dictionary
            GameData saveData = Load(saveId);

            // Check if data is null
            if (saveData != null)
            {
                saveDictionary.Add(saveId, saveData);
            }
            else
            {
                Debug.LogError("Tried to load save " + saveId + " but something went wrong.");
            }
        }

        return saveDictionary;
    }
}
