using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

	public static Vector3 GetUnitVector(float degree) {
		float x = 1.0f * Mathf.Cos(degree * Mathf.Deg2Rad);
		float y = 1.0f * Mathf.Sin(degree * Mathf.Deg2Rad);

		Vector3 newUnitVector = new Vector3(x, y, 0);

		return newUnitVector;
	}
}
