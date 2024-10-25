using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAudio : MonoBehaviour
{
    [Header("File")]
    [SerializeField] private string fileName;
    [SerializeField] private string folderPath = Application.streamingAssetsPath;
    [SerializeField] private string fullPath;

    [Header("Audio")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        fullPath = Path.Combine(folderPath, fileName);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"[ERROR] Directory for audio file does not exist at {folderPath}");
            return;
        }
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[ERROR] Audio file does not exist at {fullPath}");
            return;
        }

        if (source == null)
        {
            Debug.LogError($"[ERROR] No AudioSource was given to load the audio file into");
            return;
        }

        LoadAudioFromFile();
    }

    public void LoadAudioFromFile()
    {
        Debug.Log("Loading audio file");
        // Get all the bytes from the audio file
        byte[] bytes = File.ReadAllBytes(fullPath);

        // Put the byte array into a float array to get the vallues
        float[] floatArray = new float[bytes.Length/2];
        for (int i = 0; i < floatArray.Length; i++)
        {
            short bitValue = System.BitConverter.ToInt16(bytes, i * 2);
            floatArray[i] = bitValue / 32768.0f; // Normalize value and put into the float array
        }

        // Make the AudioClip
        clip = AudioClip.Create("LoadedClip", floatArray.Length, 1, 44100, false);
        clip.SetData(floatArray, 0);
        
        // Set the AudioClip in the given AudioSource
        source.clip = clip;
        source.Play();
    }
}
