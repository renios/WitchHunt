using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TextManager : MonoBehaviour {

	public enum DialogueState {
		Prologue,
		AfterPrologue,
		Before,
		Ingame,
		After,
		Epilogue
	}

	public DialogueState dialogueState;
	int dialogueIndex = 0;

	public TextAsset prologueTextFile;
	public TextAsset dialogueBeforeStageTextFile;

	public Canvas prologueCanvas;
	public Text prologueTextUI;
	public Image leftDialogueImage;
	public Image rightDialogueImage;

	void PrintPrologueText() {
		prologueCanvas.gameObject.SetActive(true);
		prologueTextUI.text = prologueTextFile.text;
	}

	void HidePrologueText() {
		prologueTextUI.text = "";
		prologueCanvas.gameObject.SetActive(false);
	}

	List<string> SplitTextToLine(TextAsset ta) {
		string[] lines = ta.text.Split('\n');
		return lines.ToList();
	}

	void PrintDialogue(List<string> dialogueList) {
		leftDialogueImage.enabled = false;
		rightDialogueImage.enabled = false;
		leftDialogueImage.GetComponentInChildren<Text>().text = "";
		rightDialogueImage.GetComponentInChildren<Text>().text = "";

		if (!(dialogueIndex < dialogueList.Count)) {
			dialogueState = DialogueState.Ingame;
			FindObjectOfType<Player>().shotActive = true;
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
	} 

	List<string> dialogueList;

	// Use this for initialization
	void Start () {
		dialogueState = DialogueState.Prologue;
		FindObjectOfType<Player>().shotActive = false;
		PrintPrologueText();
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogueState == DialogueState.Prologue) {
			if (Input.GetKeyDown(KeyCode.Z)) {
				HidePrologueText();

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

		// else if ()
	}
}
