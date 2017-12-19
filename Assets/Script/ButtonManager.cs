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
		else if (dest == "Stage1A")
			SceneManager.LoadScene("Stage1-A");
		else if (dest == "Stage2A")
			SceneManager.LoadScene("Stage2-A");
		else if (dest == "Stage1B")
			SceneManager.LoadScene("Stage1-B");
		else if (dest == "Stage2B")
			SceneManager.LoadScene("Stage2-B");
		else if (dest == "exit")
			Application.Quit();
	}
}
