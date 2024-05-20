using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshMerger : MonoBehaviour
{
    public bool shouldSaveOnDisk;
    void Start()
    {
        //MeshMerger();
    }

    public void MeshMerge()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        Material material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        GetComponent<MeshRenderer>().material = material;
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (shouldSaveOnDisk)
        {
            CreateAndSaveFile();
        }
    }


    private void CreateAndSaveFile()
    {
        string mapName = Path.GetFileNameWithoutExtension(name);
        string localPath = Path.Combine("Assets", "Resources", "Prefabs", "Data", "Maps", mapName);
        Directory.CreateDirectory(localPath);

        var filters = GetComponentsInChildren<MeshFilter>();
        foreach (var filter in filters)
        {
            var id = filter.mesh.GetInstanceID();
            AssetDatabase.CreateAsset(filter.mesh, Path.Combine(localPath, id + ".asset"));
        }

        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath + ".prefab");

        PrefabUtility.SaveAsPrefabAssetAndConnect(this.gameObject, localPath, InteractionMode.UserAction);
        Debug.Log($"Saving to {localPath}");
    }
}
