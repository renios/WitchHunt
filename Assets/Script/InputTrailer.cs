using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputTrailer {
	public static List<List<KeyCode>> moveInput;
	public static List<bool> shotInput;
	public static List<bool> slowInput;
	public static List<bool> bombInput;

	public static void InitializeInputTrailer() {
		moveInput = new List<List<KeyCode>>();
		shotInput = new List<bool>();
		slowInput = new List<bool>();
		bombInput = new List<bool>();
	}
}
