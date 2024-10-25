using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadTexture : MonoBehaviour
{
    [Header("File")]
    [SerializeField] private string fileName;
    [SerializeField] private string folderPath = Application.streamingAssetsPath;
    [SerializeField] private string fullPath;

    [Header("Sprite")]
    [SerializeField] private Image toChange;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        fullPath = Path.Combine(folderPath, fileName);
        LoadTextureFromFile();
    }

    public void LoadTextureFromFile()
    {
        if (Directory.Exists(folderPath) && File.Exists(fullPath))
        {
            Debug.Log("Loading texture");
            // Read in all the bytes from the file
            byte[] bytes = File.ReadAllBytes(fullPath);

            // Create the temporary texture to put our data in
            Texture2D texture = new Texture2D(200, 200);
            texture.LoadImage(bytes);

            // Set the sprite to be the new sprite shown
            toChange.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        else
        {
            Debug.LogError($"[ERROR] File not found at {fullPath}");
        }
    }
}
