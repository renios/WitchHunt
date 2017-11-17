using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPlayer : MonoBehaviour {

	Player player;

	float lastShotTime = 0;

	IEnumerator MoveByTrail() {
		for (int frame = 0; frame < InputTrailer.moveInput.Count; frame++) {
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

			// shot
			lastShotTime += Time.deltaTime;

			if ((InputTrailer.shotInput[frame]) && (lastShotTime > player.shotDelay)) {
				if (InputTrailer.slowInput[frame]) {
					ShotStraightStrong();
				}
				else {
					ShotSector();
				}
				lastShotTime = 0;
			}

			yield return null;
		}
	}

	void ShotSector() {
		float sectorDelta = 15;

		GameObject upperBullet = Instantiate(player.bullet, transform.position, Quaternion.identity) as GameObject;
		GameObject midBullet = Instantiate(player.bullet, transform.position, Quaternion.identity) as GameObject;
		GameObject lowerBullet = Instantiate(player.bullet, transform.position, Quaternion.identity) as GameObject;

		upperBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(sectorDelta);
		midBullet.GetComponent<Bullet>().direction = Vector3.right;
		lowerBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(-sectorDelta);

		upperBullet.tag = "EnemyBullet";
		midBullet.tag = "EnemyBullet";
		lowerBullet.tag = "EnemyBullet";
	}

	void ShotStraightStrong() {
		GameObject midBullet = Instantiate(player.bullet, transform.position, Quaternion.identity) as GameObject;
		midBullet.GetComponent<Bullet>().direction = Vector3.right;
		midBullet.GetComponent<Bullet>().damage *= player.damageCoef;

		midBullet.transform.localScale *= 2;

		midBullet.tag = "EnemyBullet";
	}

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player>();
		StartCoroutine (MoveByTrail());
	}
	
	// Update is called once per frame
	void Update () {
		// if (Input.GetKeyDown(KeyCode.P)) {
		// 	StartCoroutine (MoveByTrail());
		// }
	}
}
