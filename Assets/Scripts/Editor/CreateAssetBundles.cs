using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAssetBundles()
    {
        string folderPath = "Assets/AssetBundles";
        if(!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        BuildPipeline.BuildAssetBundles(folderPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
