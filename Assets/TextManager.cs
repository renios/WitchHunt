using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TextManager : MonoBehaviour {

	public TextAsset prologueTextFile;
	public TextAsset dialogueBeforeStageTextFile;

	public Text prologueTextUI;
	public Image leftDialogueImage;
	public Image rightDialogueImage;

	void PrintPrologueText() {
		prologueTextUI.text = prologueTextFile.text;
	}

	List<string> SplitTextToLine(TextAsset ta) {
		string[] lines = ta.text.Split('\n');
		return lines.ToList();
	}

	int dialogueIndex = 0;

	void PrintDialogue(List<string> dialogueList) {
		leftDialogueImage.enabled = false;
		rightDialogueImage.enabled = false;
		leftDialogueImage.GetComponentInChildren<Text>().text = "";
		rightDialogueImage.GetComponentInChildren<Text>().text = "";

		if (!(dialogueIndex < dialogueList.Count)) return;

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
		// PrintPrologueText();
		dialogueList = SplitTextToLine(dialogueBeforeStageTextFile);
		PrintDialogue(dialogueList);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z)) {
			dialogueIndex += 1;
			PrintDialogue(dialogueList);
		}
	}
}
