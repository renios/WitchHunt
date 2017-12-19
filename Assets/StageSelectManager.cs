using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour {

	public Text stage1Text;
	public Image stage1BImage;
	public Text stage2Text;
	public Image stage2BImage;
	public Text stage3Text;
	public Image stage3BImage;

	// Use this for initialization
	void Start () {
		stage1BImage.enabled = false;
		stage2BImage.enabled = false;
		stage3BImage.enabled = false;
		if (InputTrailer.Stage1ACleared)
			stage1BImage.enabled = true;
		if (InputTrailer.Stage2ACleared)
			stage2BImage.enabled = true;
	}

	bool hiddenStageSelected = false;

	// Update is called once per frame
	void Update () {
		if (EventSystem.current.currentSelectedGameObject.name == "Button_Stage1") {
			if (Input.GetKeyDown(KeyCode.DownArrow) && (InputTrailer.Stage1ACleared)) {
				stage1Text.text = "아이를 치료해주었다가" + '\n' + "마녀로 몰린" + '\n' + "사냥꾼";
				hiddenStageSelected = true;
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow)) {
				stage1Text.text = "아이를 현혹하고" + '\n' + "가뭄을 일으킨" + '\n' + "숲의 마녀";
				hiddenStageSelected = false;
			}
		}
		else if (EventSystem.current.currentSelectedGameObject.name == "Button_Stage2") {
			// if (Input.GetKeyDown(KeyCode.DownArrow) && (InputTrailer.Stage1ACleared)) {
			// 	stage2Text.text = "남편의 돌연사로" + '\n' + "엄청난 유산을 상속받은" + '\n' + "과부";
			// 	hiddenStageSelected = true;
			// }
			// else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			// 	stage2Text.text = "악마의 꾐에 빠져" + '\n' + "남편을 저주로 죽인" + '\n' + "바람의 마녀";
			// 	hiddenStageSelected = false;
			// }
		}


		if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene("Intro");
		}
	}

	public void StartScene(string sceneName) {
		if (sceneName == "Stage1") {
			if (!hiddenStageSelected) {
				SceneManager.LoadScene(sceneName + "-A");
			}
			else {
				SceneManager.LoadScene(sceneName + "-B");
			}
		}
		else if (sceneName == "Stage2") {
			if (!hiddenStageSelected) {
				SceneManager.LoadScene(sceneName + "-A");
			}
			else {
				SceneManager.LoadScene(sceneName + "-B");
			}
		}
	}
}
