﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public bool trailActive;
	public bool shotActive;
	public bool moveActive;

	public bool canShot = true;

	public int maxHp = 5;
	public GameObject hpOrb;
	int currentHp;
	List<GameObject> hpOrbs;
	GameObject playerHpPanel;

	bool isDead = false;

	public int maxBomb = 3;
	public int initBomb = 2;
	public GameObject bomb;
	int currentBomb;
	List<GameObject> bombs;
	GameObject playerBombPanel;
	public int bombDamage;

	public float defaultSpeed;
	public float slowCoef;
	public int damageCoef;
	public float shotDelay;
	float lastShotTime;

	// 기록용
	int currentFrame = 0;

	public GameObject bullet;
	public GameObject bombObj;

	float noDamagedTime;
	float damagedDelay = 0.5f;

	SEManager SEPlayer;

	bool IsSlow() {
		return Input.GetKey(KeyCode.LeftShift);
	}

	void ShotBomb() {
		currentBomb -= 1;
		UpdatePlayerBombUI();
		SEPlayer.Play(SEManager.Sounds.Bomb);
		if (bombObj == null) {
			
			FindObjectsOfType<Bullet>().ToList().ForEach(bullet => {
				if (bullet.tag == "EnemyBullet") {
					bullet.DestroyBullet();
				}
			});
			FindObjectOfType<Boss>().Damaged(bombDamage);
		}
		else {
			Instantiate(bombObj, transform.position, Quaternion.identity);
		}
	}

	void ShotSector() {
		float sectorDelta = 15;

		GameObject upperBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		GameObject midBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		GameObject lowerBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;

		upperBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(sectorDelta);
		midBullet.GetComponent<Bullet>().direction = Vector3.right;
		lowerBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(-sectorDelta);

		upperBullet.transform.rotation *= Quaternion.Euler(0,0,sectorDelta);
		lowerBullet.transform.rotation *= Quaternion.Euler(0,0,-sectorDelta);

		upperBullet.tag = "PlayerBullet";
		midBullet.tag = "PlayerBullet";
		lowerBullet.tag = "PlayerBullet";

	}

	void ShotStraightStrong() {
		GameObject midBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		midBullet.GetComponent<Bullet>().direction = Vector3.right;
		midBullet.GetComponent<Bullet>().damage *= damageCoef;

		midBullet.transform.localScale *= 1.5f;

		midBullet.tag = "PlayerBullet";

	}

	void InitializePlayerHp() {
		hpOrbs = new List<GameObject>();
		currentHp = maxHp;
		for(int i = 0; i < maxHp; i++) {
			GameObject newHpOrb = Instantiate(hpOrb) as GameObject;
			newHpOrb.transform.SetParent(playerHpPanel.transform);
			newHpOrb.transform.localScale = new Vector3(1,1,1);
			hpOrbs.Add(newHpOrb);
		}
		UpdatePlayerHpUI();
	}

	void InitializePlayerBomb() {
		bombs = new List<GameObject>();
		currentBomb = initBomb;
		for(int i = 0; i < maxBomb; i++) {
			GameObject newBomb = Instantiate(bomb) as GameObject;
			newBomb.transform.SetParent(playerBombPanel.transform);
			newBomb.transform.localScale = new Vector3(1,1,1);
			bombs.Add(newBomb);
		}
		UpdatePlayerBombUI();
	}

	// Use this for initialization
	void Start () {
		lastShotTime = 0;
		noDamagedTime = 0;
		
		if (trailActive) {
			if (SceneManager.GetActiveScene().name == "Stage1-A") {
				InputTrailer.InitializeInputTrailer(1);
			}
			else if (SceneManager.GetActiveScene().name == "Stage2-A") {
				InputTrailer.InitializeInputTrailer(2);
			}
		}
			

		playerHpPanel = GameObject.Find("PlayerHpPanel");
		playerBombPanel = GameObject.Find("PlayerBombPanel");

		InitializePlayerHp();
		InitializePlayerBomb();

		SEPlayer = FindObjectOfType<SEManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		lastShotTime += Time.deltaTime;
		noDamagedTime += Time.deltaTime;

		currentFrame += 1;
		if (trailActive) {
			if (SceneManager.GetActiveScene().name == "Stage1-A") {
				InputTrailer.slowInput_1stage.Add(IsSlow());
			}
			else if (SceneManager.GetActiveScene().name == "Stage2-A") {
				InputTrailer.slowInput_2stage.Add(IsSlow());
			}
		}

		Move();
		Shot();
		Bomb();

		if (Input.GetKeyDown(KeyCode.B)) {
			currentBomb = maxBomb;
			UpdatePlayerBombUI();
		}

		if (Input.GetKeyDown(KeyCode.H)) {
			currentHp = maxHp;
			UpdatePlayerHpUI();
		}
	}

	void Bomb() {
		// 실제로 폭탄을 쐈을 때만 true로 입력
		if (trailActive) {
			if (SceneManager.GetActiveScene().name == "Stage1-A") {
				InputTrailer.bombInput_1stage.Add(Input.GetKeyDown(KeyCode.X) && currentBomb > 0);
			}
			else if (SceneManager.GetActiveScene().name == "Stage2-A") {
				InputTrailer.bombInput_2stage.Add(Input.GetKeyDown(KeyCode.X) && currentBomb > 0);
			}
		}

		if (!Input.GetKeyDown(KeyCode.X)) return;
		if (currentBomb <= 0) return; 

		if (!shotActive) return;

		ShotBomb();
	}

	void Shot() {
		if (trailActive) {
			if (SceneManager.GetActiveScene().name == "Stage1-A") {
				InputTrailer.shotInput_1stage.Add(Input.GetKey(KeyCode.Z));
			}
			else if (SceneManager.GetActiveScene().name == "Stage2-A") {
				InputTrailer.shotInput_2stage.Add(Input.GetKey(KeyCode.Z));
			}
		}
	
		if (!Input.GetKey(KeyCode.Z)) return;
		if (lastShotTime < shotDelay) return;

		if (!canShot) return;
		if (!shotActive) return;

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

		// 대화중에는 못움직임
		if (!moveActive)
			speed = 0;

		transform.position += Vector3.right * Time.deltaTime * speed;
		
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

		if (trailActive) {
			if (SceneManager.GetActiveScene().name == "Stage1-A") {
				InputTrailer.moveInput_1stage.Add(inputs);
			}
			else if (SceneManager.GetActiveScene().name == "Stage2-A") {
				InputTrailer.moveInput_2stage.Add(inputs);
			}
		}
	}

	public void Damaged() {
		if (noDamagedTime < damagedDelay) return;

		noDamagedTime = 0;

		Instantiate(FindObjectOfType<PlayerCollider>().hitParticle, transform.position, Quaternion.identity);

		currentHp -= 1;
	
		if (currentHp > 0) SEPlayer.Play(SEManager.Sounds.EnemyDeath);

		if (currentHp < 0) currentHp = 0;
		UpdatePlayerHpUI();

		if ((currentHp == 0) && (!isDead)) {
			SEPlayer.Play(SEManager.Sounds.Dead);
			TextManager textManager = FindObjectOfType<TextManager>();
			textManager.dialogueState = TextManager.DialogueState.Gameover;
			shotActive = false;
			GetComponent<Rigidbody2D>().gravityScale = 1;
			GetComponentsInChildren<Collider2D>().ToList().ForEach(coll => Destroy(coll));
			isDead = true;
		}
	}

	void UpdatePlayerHpUI() {
		hpOrbs.ForEach(orb => Destroy(orb));
		
		for (int i = 0; i < currentHp; i++) {
			GameObject newHpOrb = Instantiate(hpOrb);
			newHpOrb.transform.SetParent(playerHpPanel.transform);
			newHpOrb.transform.localScale = new Vector3(1,1,1);
			hpOrbs.Add(newHpOrb);
		}
	}

	void UpdatePlayerBombUI() {
		bombs.ForEach(bomb => Destroy(bomb));
		
		for (int i = 0; i < currentBomb; i++) {
			GameObject newBomb = Instantiate(bomb);
			newBomb.transform.SetParent(playerBombPanel.transform);
			newBomb.transform.localScale = new Vector3(1,1,1);
			bombs.Add(newBomb);
		}
	}
}
