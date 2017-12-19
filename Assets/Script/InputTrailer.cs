using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputTrailer {

	public static bool Stage1ACleared = false;
	public static bool Stage2ACleared = false;

	public static List<List<KeyCode>> moveInput_1stage;
	public static List<bool> shotInput_1stage;
	public static List<bool> slowInput_1stage;
	public static List<bool> bombInput_1stage;

	public static List<List<KeyCode>> moveInput_2stage;
	public static List<bool> shotInput_2stage;
	public static List<bool> slowInput_2stage;
	public static List<bool> bombInput_2stage;

	public static void InitializeInputTrailer(int stage) {
		if (stage == 1) {
			moveInput_1stage = new List<List<KeyCode>>();
			shotInput_1stage = new List<bool>();
			slowInput_1stage = new List<bool>();
			bombInput_1stage = new List<bool>();
		}
		else if (stage == 2) {
			moveInput_2stage = new List<List<KeyCode>>();
			shotInput_2stage = new List<bool>();
			slowInput_2stage = new List<bool>();
			bombInput_2stage = new List<bool>();
		}
	}
}
