using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int witdh = 6;
    public int height = 6;

    public HexCell cellprefab;
    public Text cellLabelPrefab; 

    public HexCell[] cells;

    private HexMesh _mesh;
    private Canvas _gridCanvas;

    void Awake()
    {
        _mesh = GetComponentInChildren<HexMesh>();
        _gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[witdh * height];

        for (int z = 0, i = 0; z < height; z++)
            for (int x = 0; x < witdh; x++)
                CreateCell(cellprefab, x, z, i++);

    }

    void Start() {
        _mesh.Triangulate(cells);
    }

    void CreateCell(HexCell cellPrefab,int x,int z,int i)
    {
        Vector3 position;
        position.x = (x+(0.5f*(z%2f))) * HexMetrics.innerRadius*2;
        position.y = 0f;
        position.z = z* HexMetrics.outerRadius*1.5f;
        HexCell cell = cells[i] = Instantiate<HexCell>(cellprefab);
        cell.transform.SetParent(this.transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        //label
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(_gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();

    }

}
