using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

    public HexGrid grid;
    public Color[] color;
    private Color _currentColor;


    void Awake() {
        SelectColor(0);
    }

    public void SelectColor(int v) {
        _currentColor = color[v];
    }

    public void Update() {
        if (Input.GetMouseButton(0)&& !EventSystem.current.IsPointerOverGameObject())
            HandleInput();
    }

    private void HandleInput() {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
            grid.ColorCell(hit.point,_currentColor);
    }
}
