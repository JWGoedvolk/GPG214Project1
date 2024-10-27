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

    [Header("Timer")]
    [SerializeField] private float curTime = 2f;
    [SerializeField] private float resetTime = .1f;

    [Header("Object Pool")]
    [SerializeField] private int poolSize = 10;
    [SerializeField] public GameObject[] pool;
    [SerializeField] private int poolIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        fullPath = Path.Combine(folderPath, fileName);

        LoadBundleData();

        pool = new GameObject[poolSize];
        if (bundlePrefab != null )
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = Instantiate(bundlePrefab);
                pool[i].transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1));
                pool[i].SetActive(false);
            }
        }
        SpawnPrefab();
    }

    private void Update()
    {
        curTime -= Time.deltaTime;
        if (curTime <= 0f)
        {
            curTime = resetTime;
            
            if(poolIndex >= poolSize)
            {
                poolIndex = 0;
                pool[poolIndex].SetActive(true); // Test for optimisation
                pool[poolIndex].transform.position = new Vector2(Random.Range(transform.position.x - 3, transform.position.x + 3), Random.Range(transform.position.y - 1, transform.position.y + 1));
            }
            else
            {
                pool[poolIndex].SetActive(true);
                pool[poolIndex].transform.position = new Vector2(Random.Range(transform.position.x - 3, transform.position.x + 3), Random.Range(transform.position.y - 1, transform.position.y + 1));
                poolIndex++;
            }
            
        }
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
            spawned.transform.position = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1));
        }
    }
}
