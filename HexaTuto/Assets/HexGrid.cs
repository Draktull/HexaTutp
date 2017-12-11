using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
    public int width = 6;
    public int height = 6;

    public HexCell cellprefab;
    public Text cellLabelPrefab;

    public HexCell[] cells;

    private HexMesh _mesh;
    private Canvas _gridCanvas;

    public Color defaultColor = Color.white;


    void Awake() {
        _mesh = GetComponentInChildren<HexMesh>();
        _gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[width * height];

        for (int z = 0, i = 0; z < height; z++)
            for (int x = 0; x < width; x++)
                CreateCell(cellprefab, x, z, i++);

    }

    void Start() {
        _mesh.Triangulate(cells);
    }

    void CreateCell(HexCell cellPrefab, int x, int z, int i) {
        Vector3 position;
        position.x = (x + (0.5f * (z % 2f))) * HexMetrics.innerRadius * 2;
        position.y = 0f;
        position.z = z * HexMetrics.outerRadius * 1.5f;
        HexCell cell = cells[i] = Instantiate<HexCell>(cellprefab);
        cell.transform.SetParent(this.transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        if (x > 0)
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        if (z > 0) {
            if ((z & 1) == 0) {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
            }
            else {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1) {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }


        //label
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(_gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();

    }

    public void ColorCell(Vector3 point, Color color) {
        point = point - this.transform.position;
        HexCoordinates coordinates = HexCoordinates.FromPosition(point);
        int index = coordinates.Z * this.width + coordinates.Z / 2 + coordinates.X;


        //int index = coordinates.X + coordinates.Z * this.witdh + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        this._mesh.Triangulate(cells);
    }
}
