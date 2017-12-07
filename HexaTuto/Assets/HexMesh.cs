﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {
    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;
    private MeshCollider _collider;

    void Awake() {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        _collider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "hexMesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();

    }

    public void Triangulate(HexCell[] cells) {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();

        for (int i = 0; i < cells.Length; i++) {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        hexMesh.colors = colors.ToArray();
        _collider.sharedMesh = hexMesh;

    }

    private void Triangulate(HexCell hexCell) {
        Vector3 center = hexCell.transform.localPosition;
        for (int i = 0; i < 6; i++) {
            AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[i + 1]);
            AddColor(hexCell.color);
        }
    }

    private void AddColor(Color color) {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
}
