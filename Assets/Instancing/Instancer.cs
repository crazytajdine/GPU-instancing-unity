using System;
using System.Collections.Generic;
using UnityEngine;

public class Instancer : MonoBehaviour
{
    public int instance = 1000;
    public int batchSize = 1000;

    public Mesh mesh;
    public Material material;

    private List<List<Matrix4x4>> batches = new List<List<Matrix4x4>>();

    private void RenderBatches()
    {
        foreach (var batch in batches)
        {
        for (int i = 0; i < mesh.subMeshCount; i += 1) {
                Graphics.DrawMeshInstanced(mesh, i, material, batch);
        }
            
        }
    }

    private void Update()
    {
        RenderBatches();
    }

    private void Start()
    {
        int BatchesCount = Mathf.CeilToInt((float)(instance) / batchSize);
        for (int i = 0; i < BatchesCount; i++) {
            List<Matrix4x4> batch = new List<Matrix4x4>();
            for (int j= 0; j < batchSize; j++) {

                var position = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
                var rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
                var scale = new Vector3(UnityEngine.Random.Range(0.5f, 1), UnityEngine.Random.Range(0.5f, 1), UnityEngine.Random.Range(0.5f, 1));
                var matrix = Matrix4x4.TRS(position, rotation, scale);
                batch.Add(matrix);
            }
            batches.Add(batch);
        }
        print(batches.Count);
    }
}
