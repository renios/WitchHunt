using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrailPlayer : MonoBehaviour {

	Player player;

	float lastShotTime = 0;

	List<List<KeyCode>> moveInput;
	List<bool> shotInput;
	List<bool> slowInput;
	List<bool> bombInput;

	int currentFrame = 0;

	public int CurrentFrame {
		get { return currentFrame; }
	}

	public bool IsSlow {
		get {
			if (InputTrailer.slowInput_1stage == null) return false; 
			return InputTrailer.slowInput_1stage[currentFrame]; 
		}
	}

	IEnumerator MoveByTrail() {
		for (int frame = 0; frame < InputTrailer.moveInput_1stage.Count; frame++) {
			currentFrame = frame;

			// move
			float speed = player.defaultSpeed;
			List<KeyCode> inputs = InputTrailer.moveInput_1stage[frame];

			// shift키 입력시 저속이동
			if (InputTrailer.slowInput_1stage[frame]) 
				speed *= player.slowCoef;

			// 대각선으로 이동할 경우 속도 일정하게 유지
			if ((inputs.Contains(KeyCode.LeftArrow) && inputs.Contains(KeyCode.UpArrow)) ||
				(inputs.Contains(KeyCode.LeftArrow) && inputs.Contains(KeyCode.DownArrow)) ||
				(inputs.Contains(KeyCode.RightArrow) && inputs.Contains(KeyCode.UpArrow)) ||
				(inputs.Contains(KeyCode.RightArrow) && inputs.Contains(KeyCode.DownArrow))) {
				speed *= 1/Mathf.Sqrt(2);
			}

			if (inputs.Contains(KeyCode.LeftArrow)) {
				transform.position += Vector3.left * Time.deltaTime * speed;
			}
			if (inputs.Contains(KeyCode.RightArrow)) {
				transform.position += Vector3.right * Time.deltaTime * speed;
			}
			if (inputs.Contains(KeyCode.UpArrow)) {
				transform.position += Vector3.up * Time.deltaTime * speed;
			}
			if (inputs.Contains(KeyCode.DownArrow)) {
				transform.position += Vector3.down * Time.deltaTime * speed;
			}

			// shot - 별도 패턴
			yield return null;
		}
	}

	// Use this for initialization
	public IEnumerator StartTrail () {
		player = FindObjectOfType<Player>();

		if (SceneManager.GetActiveScene().name == "Stage1-A") {
			moveInput = InputTrailer.moveInput_1stage;
			shotInput = InputTrailer.shotInput_1stage;
			slowInput = InputTrailer.slowInput_1stage;
			bombInput = InputTrailer.bombInput_1stage;
		}
		else if (SceneManager.GetActiveScene().name == "Stage2-A") {
			moveInput = InputTrailer.moveInput_2stage;
			shotInput = InputTrailer.shotInput_2stage;
			slowInput = InputTrailer.slowInput_2stage;
			bombInput = InputTrailer.bombInput_2stage;
		}
		
		yield return new WaitForSeconds(1);

		if (InputTrailer.moveInput_1stage != null)
			StartCoroutine (MoveByTrail());
	}

	public void StartPattern() {
		StartCoroutine(StartTrail());
	}

	public void StopPattern() {
		StopAllCoroutines();
	}

	// Update is called once per frame
	void Update () {

	}
}
