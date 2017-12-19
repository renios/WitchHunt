using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour {

	public enum DialogueState {
		Prologue,
		Before,
		Ingame,
		After,
		Epilogue,
		Gameover
	}

	public DialogueState dialogueState;
	int dialogueIndex = 0;

	public TextAsset prologueTextFile;
	public TextAsset dialogueBeforeStageTextFile;
	public TextAsset dialogueAfterStageTextFile;
	public TextAsset epilogueTextFile;

	public Canvas prologueCanvas;
	public Text prologueTextUI;
	public Image leftDialogueImage;
	public Image rightDialogueImage;
	public Image midNarrationImage;

	public Canvas gameoverCanvas;

	void ShowCanvas(string text) {
		prologueCanvas.gameObject.SetActive(true);
		prologueTextUI.text = text;
	}

	void HideCanvas() {
		prologueTextUI.text = "";
		prologueCanvas.gameObject.SetActive(false);
	}

	void ShowGameoverCanvas() {
		gameoverCanvas.gameObject.SetActive(true);
	}

	List<string> SplitTextToLine(TextAsset ta) {
		string[] lines = ta.text.Split('\n');
		return lines.ToList();
	}

	void PrintDialogue(List<string> dialogueList) {
		leftDialogueImage.enabled = false;
		rightDialogueImage.enabled = false;
		midNarrationImage.enabled = false;
		leftDialogueImage.GetComponentInChildren<Text>().text = "";
		rightDialogueImage.GetComponentInChildren<Text>().text = "";
		midNarrationImage.GetComponentInChildren<Text>().text = "";
		Debug.Log("Dialogue list import: length "+dialogueList.Count);

		if (!(dialogueIndex < dialogueList.Count)) {
			if (dialogueState == DialogueState.Before) {
				dialogueIndex = -1;
				dialogueState = DialogueState.Ingame;
				Player player = FindObjectOfType<Player>();
				player.shotActive = true;
				player.moveActive = true;

				if ((SceneManager.GetActiveScene().name == "Stage1-A") ||
					(SceneManager.GetActiveScene().name == "Stage2-A")) 
				{
					Boss boss = FindObjectOfType<Boss>();
					boss.StartPattern();
				}
			}
			else if (dialogueState == DialogueState.After) {
				dialogueState = DialogueState.Epilogue;
				ShowCanvas(epilogueTextFile.text);
			}
			return;
		}

		string[] splitedLine = dialogueList[dialogueIndex].Split('_');
		if (splitedLine.Length < 2) return;
		else if (splitedLine[0] == "L") {
			leftDialogueImage.enabled = true;
			leftDialogueImage.GetComponentInChildren<Text>().text = splitedLine[1];
		}
		else if (splitedLine[0] == "R") {
			rightDialogueImage.enabled = true;
			rightDialogueImage.GetComponentInChildren<Text>().text = splitedLine[1];
		}
		else if (splitedLine[0] == "M") {
			midNarrationImage.enabled = true;
			midNarrationImage.GetComponentInChildren<Text>().text = splitedLine[1];
		}
	} 

	List<string> dialogueList;

	// Use this for initialization
	void Start () {
		dialogueState = DialogueState.Prologue;
		Player player = FindObjectOfType<Player>();
		player.shotActive = false;
		player.moveActive = false;
		ShowCanvas(prologueTextFile.text);
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogueState == DialogueState.Prologue) {
			if (Input.GetKeyDown(KeyCode.Z)) {
				HideCanvas();

				dialogueState = DialogueState.Before;
				dialogueList = SplitTextToLine(dialogueBeforeStageTextFile);
				PrintDialogue(dialogueList);
			}
		}
		
		else if (dialogueState == DialogueState.Before) {
			if (Input.GetKeyDown(KeyCode.Z)) {
				dialogueIndex += 1;
				PrintDialogue(dialogueList);
			}
		}

		else if (dialogueState == DialogueState.After) {
			if (dialogueIndex == -1) {
				Player player = FindObjectOfType<Player>();
				player.shotActive = false;
				player.moveActive = false;

				if ((SceneManager.GetActiveScene().name == "Stage1-A") ||
					(SceneManager.GetActiveScene().name == "Stage2-A")) 
				{
					Boss boss = FindObjectOfType<Boss>();
					boss.StopPattern();
				}

				dialogueList = SplitTextToLine(dialogueAfterStageTextFile);
				dialogueIndex = 0;
				PrintDialogue(dialogueList);

				if (SceneManager.GetActiveScene().name == "Stage1-A") {
					InputTrailer.Stage1ACleared = true;
				}
				else if (SceneManager.GetActiveScene().name == "Stage2-A") {
					InputTrailer.Stage2ACleared = true;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Z)) {
				dialogueIndex += 1;
				PrintDialogue(dialogueList);
			}
		}

		else if ((dialogueState == DialogueState.Gameover) && !gameoverCanvas.gameObject.activeInHierarchy) {	
			Invoke("ShowGameoverCanvas", 1f);
		}

		else if (dialogueState == DialogueState.Epilogue) {
			if (Input.GetKeyDown(KeyCode.Z)) {
				SceneManager.LoadScene("StageSelect");
			}
		}
	}
}
