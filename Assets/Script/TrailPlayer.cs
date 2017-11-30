﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPlayer : MonoBehaviour {

	Player player;

	float lastShotTime = 0;

	int currentFrame = 0;

	public int CurrentFrame {
		get { return currentFrame; }
	}

	public bool IsSlow {
		get {
			if (InputTrailer.slowInput == null) return false; 
			return InputTrailer.slowInput[currentFrame]; 
		}
	}

	IEnumerator MoveByTrail() {
		for (int frame = 0; frame < InputTrailer.moveInput.Count; frame++) {
			currentFrame = frame;

			// move
			float speed = player.defaultSpeed;
			List<KeyCode> inputs = InputTrailer.moveInput[frame];

			// shift키 입력시 저속이동
			if (InputTrailer.slowInput[frame]) 
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
	IEnumerator Start () {
		player = FindObjectOfType<Player>();
		
		yield return new WaitForSeconds(1);

		if (InputTrailer.moveInput != null)
			StartCoroutine (MoveByTrail());
	}

	// Update is called once per frame
	void Update () {

	}
}
