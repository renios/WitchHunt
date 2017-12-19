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
			if (SceneManager.GetActiveScene().name == "Stage1-B")
				SceneManager.LoadScene("Stage1-A");
			else if (SceneManager.GetActiveScene().name == "Stage2-B")
				SceneManager.LoadScene("Stage2-A");
		}
		if (Input.GetKeyDown(KeyCode.RightBracket)) {
			if (SceneManager.GetActiveScene().name == "Stage1-A")
				SceneManager.LoadScene("Stage1-B");
			else if (SceneManager.GetActiveScene().name == "Stage2-A")
				SceneManager.LoadScene("Stage2-B");
		}	

		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene("StageSelect");
		}	
	}
}
