using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour {

	public float defaultSpeed;
	public float slowCoef;
	public float shotDelay;
	float lastShotTime;

	// 기록용
	// float lastCheckTrailTime;
	// public float trailInterval;
	// float currentTime = 0;
	int currentFrame = 0;

	// 기록 방지를 위한 임시 변수.
	bool isTrailOn = true;

	// Dictionary<float, Vector3> moveTrails;
	List<List<KeyCode>> moveInput;
	List<bool> shotInput;
	List<bool> slowInput;

	public GameObject bullet;

	bool IsSlow() {
		return Input.GetKey(KeyCode.LeftShift);
	}

	void ShotSector() {
		float sectorDelta = 15;

		GameObject upperBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		GameObject midBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		GameObject lowerBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;

		upperBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(sectorDelta);
		midBullet.GetComponent<Bullet>().direction = Vector3.right;
		lowerBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(-sectorDelta);
	}

	void ShotStraightStrong() {
		GameObject midBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		midBullet.GetComponent<Bullet>().direction = Vector3.right;

		midBullet.transform.localScale *= 2;
	}

	// Use this for initialization
	void Start () {
		lastShotTime = 0;

		// lastCheckTrailTime = 0;
		// moveTrails = new Dictionary<float, Vector3>();

		moveInput = new List<List<KeyCode>>();
		shotInput = new List<bool>();
		slowInput = new List<bool>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// currentTime += Time.deltaTime;
		currentFrame += 1;
		if (isTrailOn)
			CheckTrail();

		slowInput.Add(IsSlow());

		Move();
		Shot();

		if (Input.GetKeyDown(KeyCode.P)) {
			StartCoroutine (MoveByTrail());
		}
	}

	// 슬로우 여부와 공격 여부도 추후 추가할 것.
	void CheckTrail() {
		// lastCheckTrailTime += Time.deltaTime;

		// if (lastCheckTrailTime < trailInterval) return;

		// moveTrails.Add(currentTime, transform.position);
		// // Debug.Log("Time : " + currentTime + ", Pos : " + (Vector2)transform.position);
		// lastCheckTrailTime = 0;
	}

	IEnumerator MoveByTrail() {
		isTrailOn = false;

		// foreach (var trail in moveTrails) {
		// 	if (trail.Key == 0) {
		// 		transform.position = trail.Value;
		// 	}
		// 	else {
		// 		transform.DOMove(trail.Value, trailInterval).SetEase(Ease.Linear);
		// 	}
		// 	yield return new WaitForSeconds(trailInterval);
		// }

		for (int frame = 0; frame < moveInput.Count; frame++) {
			// move
			float speed = defaultSpeed;
			List<KeyCode> inputs = moveInput[frame];

			// shift키 입력시 저속이동
			if (slowInput[frame]) 
				speed *= slowCoef;

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

			if ((shotInput[frame]) && (lastShotTime > shotDelay)) {
				if (slowInput[frame]) {
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

	void Shot() {
		lastShotTime += Time.deltaTime;

		shotInput.Add(Input.GetKey(KeyCode.Z));

		if (!Input.GetKey(KeyCode.Z)) return;
		if (lastShotTime < shotDelay) return;

		if (IsSlow()) {
			ShotStraightStrong();
		}
		else {
			ShotSector();
		}
		lastShotTime = 0;
	}

	void Move() {
		float speed = defaultSpeed;
		
		// shift키 입력시 저속이동
		if (IsSlow()) 
			speed *= slowCoef;

		// 대각선으로 이동할 경우 속도 일정하게 유지
		if ((Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)) ||
			(Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow)) ||
			(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow)) ||
			(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow))) {
			speed *= 1/Mathf.Sqrt(2);
		}

		List<KeyCode> inputs = new List<KeyCode>();

		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += Vector3.left * Time.deltaTime * speed;
			inputs.Add(KeyCode.LeftArrow);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position += Vector3.right * Time.deltaTime * speed;
			inputs.Add(KeyCode.RightArrow);
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.position += Vector3.up * Time.deltaTime * speed;
			inputs.Add(KeyCode.UpArrow);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.position += Vector3.down * Time.deltaTime * speed;
			inputs.Add(KeyCode.DownArrow);
		}

		moveInput.Add(inputs);
	}
}
