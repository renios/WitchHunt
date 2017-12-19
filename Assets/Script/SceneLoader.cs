using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftBracket)) {
			SceneManager.LoadScene("Stage1-A");
		}
		if (Input.GetKeyDown(KeyCode.RightBracket)) {
			SceneManager.LoadScene("Stage1-B");
		}	

		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene("StageSelect");
		}	
	}
}
