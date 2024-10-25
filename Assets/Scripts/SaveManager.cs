using Gamekit2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [Serializable]
    public class SaveData
    {
        // Position data
        public Vector2 PlayerPosition;
        public string SceneName;

        // Inventory data
        public bool MeleeUnlocked = false;
        public bool RangeUnlocked = false;
        public bool HasKey1 = false;
        public bool HasKey2 = false;
        public bool HasKey3 = false;

        // Constructors
        public SaveData()
        {

        }
    }

    

    [Header("Object References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] public string sceneName;
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public InventoryController inventoryController;

    [Header("Save Data")]
    [SerializeField] private SaveData saveData = new SaveData();
    [SerializeField] public bool isSaved = false;

    [Header("Save File")]
    [SerializeField] private string playerSaveName = "PlayerSave.xml";
    [SerializeField] private string folderPath = Application.streamingAssetsPath;
    [SerializeField] private string fullPath = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        fullPath = Path.Combine(folderPath, playerSaveName);

        DontDestroyOnLoad(gameObject);

        // TODO: Load save data if file exists
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Debug.Log(sceneController.initialSceneTransitionDestination.destinationTag);
            LoadXML();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SaveXML();
        }

        
    }

    public string GetSceneStringFromTag(SceneTransitionDestination.DestinationTag destination)
    {
        switch (destination)
        {
            case SceneTransitionDestination.DestinationTag.A:
                return "A";
            case SceneTransitionDestination.DestinationTag.B:
                return "B";
            case SceneTransitionDestination.DestinationTag.C:
                return "C";
            case SceneTransitionDestination.DestinationTag.D:
                return "D";
            case SceneTransitionDestination.DestinationTag.E:
                return "E";
            case SceneTransitionDestination.DestinationTag.F:
                return "F";
            case SceneTransitionDestination.DestinationTag.G:
                return "G";
            default:
                return "B";
        }
    }

    public SceneTransitionDestination.DestinationTag GetsceneTagFromString(string tag)
    {
        switch(tag)
        {
            case "A": return SceneTransitionDestination.DestinationTag.A;
            case "B": return SceneTransitionDestination.DestinationTag.B;
            case "C": return SceneTransitionDestination.DestinationTag.C;
            case "D": return SceneTransitionDestination.DestinationTag.D;
            case "E": return SceneTransitionDestination.DestinationTag.E;
            case "F": return SceneTransitionDestination.DestinationTag.F;
            case "G": return SceneTransitionDestination.DestinationTag.G;
            default: return SceneTransitionDestination.DestinationTag.B;
        }
    }

    public void GetReferences()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        playerTransform = playerInput.transform;
        inventoryController = playerInput.gameObject.GetComponent<InventoryController>();
    }

    public void SaveXML()
    {
        Debug.Log("Saving XML");

        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"[SAVE][ERROR] Folder does not exist at {folderPath}");
            return;
        }

        GetReferences();
        isSaved = true;
        saveData = new SaveData();
        saveData.PlayerPosition = playerTransform.position;
        saveData.SceneName = sceneName;
        saveData.MeleeUnlocked = playerInput.MeleeAttack.Enabled;
        saveData.RangeUnlocked = playerInput.RangedAttack.Enabled;
        saveData.HasKey1 = inventoryController.HasItem("Key1");
        saveData.HasKey2 = inventoryController.HasItem("Key2");
        saveData.HasKey3 = inventoryController.HasItem("Key3");

        Debug.Log($"[SAVE][INFO] PlayerPosition: {playerTransform.position}");
        Debug.Log($"[SAVE][INFO] SceneName: {sceneName}");

        Debug.Log($"[SAVE][INFO] Melee Unlocked: {playerInput.MeleeAttack.Enabled}");
        Debug.Log($"[SAVE][INFO] Ranged Unlocked: {playerInput.MeleeAttack.Enabled}");

        Debug.Log($"[SAVE][INFO] Key1 Unlocked: {inventoryController.HasItem("Key1")}");
        Debug.Log($"[SAVE][INFO] Key2 Unlocked: {inventoryController.HasItem("Key2")}");
        Debug.Log($"[SAVE][INFO] Key3 Unlocked: {inventoryController.HasItem("Key3")}");


        XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

        using (StreamWriter writer = new StreamWriter(fullPath))
        {
            serializer.Serialize(writer, saveData);
        }
    }

    public void LoadXML()
    {
        Debug.Log("Loading Save Data");

        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"[LOAD][ERROR] Folder does not exist at {folderPath}");
            return;
        }
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[LOAD][ERROR] Save file does not exist at {fullPath}");
            return;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

        using (StreamReader reader = new StreamReader(fullPath))
        {
            saveData = (SaveData)serializer.Deserialize(reader);
        }

        Debug.Log("Loaded: " + saveData.PlayerPosition);
        Debug.Log("Loaded: " + saveData.SceneName);

        StartCoroutine(LoadXMLScene());
    }

    public IEnumerator LoadXMLScene()
    {
        yield return StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.Loading));
        yield return SceneManager.LoadSceneAsync(saveData.SceneName);
        yield return StartCoroutine(ScreenFader.FadeSceneIn());
        playerInput = FindAnyObjectByType(typeof(PlayerInput)).GetComponent<PlayerInput>();
        playerInput.transform.position = saveData.PlayerPosition;
        inventoryController = playerInput.gameObject.GetComponent<InventoryController>();
        if (saveData.HasKey1) inventoryController.AddItem("Key1");
        if (saveData.HasKey2) inventoryController.AddItem("Key2");
        if (saveData.HasKey3) inventoryController.AddItem("Key3");
        if (saveData.MeleeUnlocked) inventoryController.AddItem("Gun");

        yield return null;
    }
}
