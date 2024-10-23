using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public class SaveData
    {
        public Vector2 playerPosition;
        public string SceneTag;

        public SaveData()
        {

        }

        public SaveData(Vector2 playerPosition, string sceneTag)
        {
            this.playerPosition = playerPosition;
            SceneTag = sceneTag;
        }
    }

    

    [Header("Object References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private string sceneTag;
    [SerializeField] private SceneTransitionDestination.DestinationTag currentDestinationTag;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(sceneController.initialSceneTransitionDestination.destinationTag);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SaveXML();
        }

        if (SceneController.Transitioning)
        {
            do
            {
                SaveXML();
            } while (!isSaved);
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
        playerTransform = FindObjectOfType(typeof(PlayerCharacter)).GetComponent<Transform>();
        sceneController = FindObjectOfType(typeof(SceneController)) as SceneController;
    }

    public void SaveXML()
    {
        Debug.Log("Saving XML");
        GetReferences();
        isSaved = true;
        saveData.playerPosition = playerTransform.position;
        saveData.SceneTag = GetSceneStringFromTag(sceneController.CurrentTransitionDestination);

        XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

        using (StreamWriter writer = new StreamWriter(fullPath))
        {
            serializer.Serialize(writer, saveData);
        }
    }

    public void LoadXML()
    {
        Debug.Log("NYI");
    }
}
