using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics  {

	public const float outerRadius  = 10f;
	public const float innerRadius = outerRadius *  0.866025404f; // phytagore

	public static Vector3[] corners = {
		new Vector3 (0, 0, outerRadius),
		new Vector3(innerRadius,0, outerRadius*0.5f),
		new Vector3(innerRadius,0, outerRadius*-0.5f),
		new Vector3 (0, 0, -outerRadius),
		new Vector3 (-innerRadius, 0, outerRadius*-0.5f),
		new Vector3 (-innerRadius, 0, outerRadius*0.5f),
        new Vector3 (0, 0, outerRadius),
    };
		
}
