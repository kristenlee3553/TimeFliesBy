using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SaveDataManager : MonoBehaviour
{
    [Header("Debugging")]
    // SET THIS TO TRUE IF YOU DON'T WANT TO GO THROUGH MAIN MENU ALL THE TIME
    [SerializeField] private bool initalizeDataIfNull = false;

    [Header("File Storage Config")]

    // Name of file we want to store data to
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<SaveDataInterface> saveDataObjects;
    private FileDataHandler dataHandler;
    private string selectedSaveId = "";

    // So that other classes can call methods here using the class name
    public static SaveDataManager instance { get; private set; }

    // Called when game scene loads
    private void Awake()
    {
        // Only want one SaveDataManager
        if (instance != null)
        {
            Debug.Log("Found more than one Save Data Manager in the scene. Removing the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // This gets the directory name from player's OS
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // Get saved data when loaded level scenes
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.saveDataObjects = FindAllSaveDataObjects();
        LoadGame();
    }

    // Save data when level is complete
    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void ChangeSelectedSaveId(string newSaveId)
    {
        // Update save ID for saving and loading
        this.selectedSaveId = newSaveId;

        // Load the game using the appropriate save ID
        LoadGame();
    }

    public void NewGame()
    {
        // Initialize with default game data
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load game data from file
        this.gameData = dataHandler.Load(selectedSaveId);

        // Creates a new game if you want to test scenes without going through main menu
        if (this.gameData == null && initalizeDataIfNull)
        {
            NewGame();
        }

        // If game data is empty, don't continue
        if (this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started.");
            return;
        }

        // Loads data from each script that uses SaveDataInterface
        foreach (SaveDataInterface saveDataObj in saveDataObjects)
        {
            saveDataObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // Check if game data is empty
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started.");
            return;
        }

        // Saves data using each script that uses SaveDataInterface
        foreach (SaveDataInterface saveDataObj in saveDataObjects)
        {
            saveDataObj.SaveData(ref gameData);
        }

        // Saves data to file
        dataHandler.Save(gameData, selectedSaveId);
    }

    // Saves data when quitting game
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // Finds all scripts that use the SaveDataInterface
    private List<SaveDataInterface> FindAllSaveDataObjects()
    {
        IEnumerable<SaveDataInterface> saveDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<SaveDataInterface>();

        return new List<SaveDataInterface>(saveDataObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllSavesGameData()
    {
        return dataHandler.LoadAllSaves();
    }
}
