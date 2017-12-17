using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void MoveScene (string dest) {
		if (dest == "start")
			SceneManager.LoadScene("Stage1-A");
		else if (dest == "continue")
			SceneManager.LoadScene("StageSelect");
		else if (dest == "exit")
			Application.Quit();
	}
}
