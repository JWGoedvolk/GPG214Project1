using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAssetBundleData : MonoBehaviour
{
    [Header("File")]
    [SerializeField] private string fileName = "obstacles";
    [SerializeField] private string folderPath = "Assets/AssetBundles";
    [SerializeField] private string fullPath;

    [Header("Asset Bundle")]
    [SerializeField] private AssetBundle bundle;
    [SerializeField] GameObject bundlePrefab;
    [SerializeField] Material bundleMaterial;
    [SerializeField] Texture2D bundleTexture;
    // Start is called before the first frame update
    void Start()
    {
        fullPath = Path.Combine(folderPath, fileName);

        LoadBundleData();
        SpawnPrefab();
    }

    public void LoadBundleData()
    {
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[ERROR] Could not fidn asset bundle at {fullPath}");
            return;
        }

        bundle = AssetBundle.LoadFromFile(fullPath);
        bundlePrefab = (GameObject)bundle.LoadAsset("Wooden_Crate");
        bundleMaterial = (Material)bundle.LoadAsset("ShaderMaterial");
        bundleTexture = (Texture2D)bundle.LoadAsset("Chest");
        Debug.Log($"Loaded AssetBundle");
    }

    public void SpawnPrefab()
    {
        if (bundlePrefab != null)
        {
            var spawned = Instantiate(bundlePrefab);
        }
    }
}
