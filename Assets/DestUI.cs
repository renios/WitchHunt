using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DestUI : MonoBehaviour {

	public float maxDest;
	float currentDest = 0;
	public Slider slider;
	bool isStart = false;
	bool isEnd = false;
	TextManager textManager;

	// Use this for initialization
	void Start () {
		textManager = FindObjectOfType<TextManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((!isStart) && (textManager.dialogueState == TextManager.DialogueState.Ingame))
			isStart = true;

		if ((isStart) && (!isEnd)) {
			currentDest += Time.deltaTime;
			slider.value = currentDest / maxDest;
		}

		if ((!isEnd) && (currentDest >= maxDest)) {
			textManager.dialogueState = TextManager.DialogueState.After;
			isEnd = true;
		}
	}
}
