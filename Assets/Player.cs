using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour {

	public float defaultSpeed;
	public float slowCoef;
	public float shotDelay;
	float lastShotTime;
	float lastCheckTrailTime;
	public float trailInterval;
	float currentTime = 0;

	// 기록 방지를 위한 임시 변수.
	bool isTrailOn = true;

	Dictionary<float, Vector3> moveTrails;

	public GameObject bullet;

	bool isSlow() {
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

		lastCheckTrailTime = 0;
		moveTrails = new Dictionary<float, Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		if (isTrailOn)
			CheckTrail();
		Move();
		Shot();

		if (Input.GetKeyDown(KeyCode.P)) {
			StartCoroutine (MoveByTrail());
		}
	}

	// 슬로우 여부와 공격 여부도 추후 추가할 것.
	void CheckTrail() {
		lastCheckTrailTime += Time.deltaTime;

		if (lastCheckTrailTime < trailInterval) return;

		moveTrails.Add(currentTime, transform.position);
		// Debug.Log("Time : " + currentTime + ", Pos : " + (Vector2)transform.position);
		lastCheckTrailTime = 0;
	}

	IEnumerator MoveByTrail() {
		isTrailOn = false;

		foreach (var trail in moveTrails) {
			if (trail.Key == 0) {
				transform.position = trail.Value;
			}
			else {
				transform.DOMove(trail.Value, trailInterval);
			}
			yield return new WaitForSeconds(trailInterval);
		}
	}

	void Shot() {
		lastShotTime += Time.deltaTime;

		if (!Input.GetKey(KeyCode.Z)) return;
		if (lastShotTime < shotDelay) return;

		if (isSlow()) {
			ShotStraightStrong();
		}
		else {
			ShotSector();
		}
		lastShotTime = 0;
	}

	void Move() {
		float speed = defaultSpeed;
		
		if (isSlow()) 
			speed *= slowCoef;
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += Vector3.left * Time.deltaTime * speed;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position += Vector3.right * Time.deltaTime * speed;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.position += Vector3.up * Time.deltaTime * speed;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.position += Vector3.down * Time.deltaTime * speed;
		}
	}
}
