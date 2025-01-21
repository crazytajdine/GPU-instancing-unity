using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Make sure to import DoTween

public class Instancer : MonoBehaviour {
    public int instance = 1000;
    public int batchSize = 1000;

    public Mesh mesh;
    public Material material;

    private List<List<Matrix4x4>> batches = new List<List<Matrix4x4>>();
    private List<Vector3> initialPositions = new List<Vector3>(); // To store the initial positions for animation

    private void RenderBatches() {
        foreach (var batch in batches) {
            for (int i = 0; i < mesh.subMeshCount; i++) {
                Graphics.DrawMeshInstanced(mesh, i, material, batch);
            }
        }
    }

    private void Update() {
        RenderBatches();
    }

    private void Start() {
        int BatchesCount = Mathf.CeilToInt((float)(instance) / batchSize);
        for (int i = 0; i < BatchesCount; i++) {
            List<Matrix4x4> batch = new List<Matrix4x4>();
            for (int j = 0; j < batchSize; j++) {
                var position = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
                var rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
                var scale = new Vector3(UnityEngine.Random.Range(0.5f, 1), UnityEngine.Random.Range(0.5f, 1), UnityEngine.Random.Range(0.5f, 1));
                var matrix = Matrix4x4.TRS(position, rotation, scale);
                batch.Add(matrix);
                initialPositions.Add(position); // Store initial positions for animations
            }
            batches.Add(batch);
        }

        // Start animating the positions of the instances
        AnimateInstances();
    }

    private void AnimateInstances() {
        // You can use DoTween to animate the instances' positions or other properties.
        for (int i = 0; i < initialPositions.Count; i++) {
            int index = i; // Capture the index for the DoTween closure
            Vector3 targetPosition = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
            
            // Animate each instance position using DoTween
            DOTween.To(() => initialPositions[index], x => initialPositions[index] = x, targetPosition, 2f).OnUpdate(() => {
                // Update the matrix with the new position
                var matrix = Matrix4x4.TRS(initialPositions[index], Quaternion.identity, Vector3.one);
                batches[index / batchSize][index % batchSize] = matrix;
            });
        }
    }
}
