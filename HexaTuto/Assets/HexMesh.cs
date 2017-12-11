using System;
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
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
            Triangulate(d, hexCell);
        }
    }

    private void Triangulate(HexDirection d, HexCell hexCell) {
        Vector3 center = hexCell.transform.localPosition;
        Vector3 v1 = center + HexMetrics.GetFirstSolidCorner(d);
        Vector3 v2 = center + HexMetrics.GetSecondSolidCorner(d);
        AddTriangle(center, v1, v2);
        AddTriangleColor(hexCell.color, hexCell.color, hexCell.color);




     
        HexCell neighborNext = hexCell.GetNeighbor(d.Next()) ?? hexCell;
        HexCell neighborPrev = hexCell.GetNeighbor(d.Previous()) ?? hexCell;
        //Color colorNext = (hexCell.color + neighbor.color + neighborNext.color) / 3f;
        //Color colorPrev = (hexCell.color + neighbor.color + neighborPrev.color) / 3f;

        //Color colorBridge = (hexCell.color + neighbor.color) * 0.5f;
        if (d<= HexDirection.SE ) { // NE , E, SE
            TriangulateConnection(d, hexCell, v1, v2);
        }

        //// previous Corner

        //AddTriangle(v1, center + HexMetrics.GetFirstCorner(d), v3);
        //AddTriangleColor(hexCell.color, colorPrev, colorBridge);

        ////next corner
        //AddTriangle(v2, v4, center + HexMetrics.GetSecondCorner(d)); ;
        //AddTriangleColor(hexCell.color, colorBridge, colorNext);


    }
    

    private void TriangulateConnection( HexDirection d, HexCell cell, Vector3 v1 , Vector3 v2) {
        HexCell neighbor = cell.GetNeighbor(d);
        if (neighbor == null) {
            return;
        }
        Vector3 bridge = HexMetrics.GetBridge(d);
        Vector3 v3 = v1 + bridge;
        Vector3 v4 = v2 + bridge;
        AddQuad(v1, v2, v3, v4);
        AddQuadColor(cell.color, neighbor.color);

        //prev
        HexCell neighborNext = cell.GetNeighbor(d.Next());
        if (d <= HexDirection.E && neighborNext != null) {
            AddTriangle(v2, v4 , v2 + HexMetrics.GetBridge(d.Next()));
            AddTriangleColor(cell.color, neighbor.color, neighborNext.color);

        }

    }
    private void AddTriangleColor(Color c1, Color c2, Color c3) {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
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

    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);


    }
    void AddQuadColor(Color c1, Color c2) {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c2);

    }

}

