using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using static SaveManager;

public class KeySetter : MonoBehaviour
{
    [Header("Save File")]
    [SerializeField] private string fileName;
    [SerializeField] private string folderPath = Application.streamingAssetsPath;
    [SerializeField] private string fullPath;
    [SerializeField] private SaveManager.SaveData saveData;

    [Header("Key Setting")]
    [SerializeField] private GameObject keyObject;
    [SerializeField] private int keyNumber;

    private void Start()
    {
        fullPath = Path.Combine(folderPath, fileName);
        GetSaveData();

        if (saveData != null)
        {
            switch (keyNumber)
            {
                case 1:
                    keyObject.SetActive(!saveData.HasKey1);
                    break;
                case 2:
                    keyObject.SetActive(!saveData.HasKey2);
                    break;
                case 3:
                    keyObject.SetActive(!saveData.HasKey3);
                    break;
            }
        }
    }

    public void GetSaveData()
    {
        if (Directory.Exists(folderPath))
        {
            if (File.Exists(fullPath)) 
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

                using (StreamReader reader = new StreamReader(fullPath))
                {
                    saveData = (SaveData)serializer.Deserialize(reader);
                }
            }
        }
    }
}
