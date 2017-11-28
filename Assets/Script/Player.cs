using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class Player : MonoBehaviour {

	public bool trailActive;

	public int maxHp = 3;
	public GameObject hpOrb;
	int currentHp;
	List<Image> hpOrbImages;

	public int maxBomb = 3;
	public int initBomb = 2;
	public GameObject bombOrb;
	int currentBomb;
	List<Image> bombImages;
	public int bombDamage;

	public float defaultSpeed;
	public float slowCoef;
	public int damageCoef;
	public float shotDelay;
	float lastShotTime;

	// 기록용
	int currentFrame = 0;

	public GameObject bullet;

	bool IsSlow() {
		return Input.GetKey(KeyCode.LeftShift);
	}

	void ShotBomb() {
		currentBomb -= 1;
		UpdatePlayerBombUI();
		FindObjectsOfType<Bullet>().ToList().ForEach(bullet => {
			if (bullet.tag == "EnemyBullet") {
				bullet.DestroyBullet();
			}
		});
		FindObjectOfType<Boss>().Damaged(bombDamage);
	}

	void ShotSector() {
		float sectorDelta = 15;

		GameObject upperBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		GameObject midBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		GameObject lowerBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;

		upperBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(sectorDelta);
		midBullet.GetComponent<Bullet>().direction = Vector3.right;
		lowerBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(-sectorDelta);

		upperBullet.tag = "PlayerBullet";
		midBullet.tag = "PlayerBullet";
		lowerBullet.tag = "PlayerBullet";
	}

	void ShotStraightStrong() {
		GameObject midBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		midBullet.GetComponent<Bullet>().direction = Vector3.right;
		midBullet.GetComponent<Bullet>().damage *= damageCoef;

		midBullet.transform.localScale *= 2;

		midBullet.tag = "PlayerBullet";
	}

	void InitializePlayerHp() {
		hpOrbImages = new List<Image>();
		GameObject playerHpPanel = GameObject.Find("PlayerHpPanel");
		currentHp = maxHp;
		for(int i = 0; i < maxHp; i++) {
			GameObject newHpOrb = Instantiate(hpOrb) as GameObject;
			newHpOrb.transform.SetParent(playerHpPanel.transform);
			newHpOrb.transform.localScale = new Vector3(1,1,1);
			hpOrbImages.Insert(i, newHpOrb.GetComponent<Image>());
		}
	}

	void InitializePlayerBomb() {
		bombImages = new List<Image>();
		GameObject playerBombPanel = GameObject.Find("PlayerBombPanel");
		currentBomb = initBomb;
		for(int i = 0; i < maxBomb; i++) {
			GameObject newBombOrb = Instantiate(bombOrb) as GameObject;
			newBombOrb.transform.SetParent(playerBombPanel.transform);
			newBombOrb.transform.localScale = new Vector3(1,1,1);
			bombImages.Insert(i, newBombOrb.GetComponent<Image>());
		}
		UpdatePlayerBombUI();
	}

	// Use this for initialization
	void Start () {
		lastShotTime = 0;
		if (trailActive) 
			InputTrailer.InitializeInputTrailer();

		InitializePlayerHp();
		InitializePlayerBomb();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		currentFrame += 1;
		if (trailActive)
			InputTrailer.slowInput.Add(IsSlow());

		Move();
		Shot();
		Bomb();
	}

	void Bomb() {
		// 실제로 폭탄을 쐈을 때만 true로 입력
		if (trailActive)
			InputTrailer.bombInput.Add(Input.GetKeyDown(KeyCode.X) && currentBomb > 0);

		if (!Input.GetKeyDown(KeyCode.X)) return;
		if (currentBomb <= 0) return; 

		ShotBomb();
	}

	void Shot() {
		lastShotTime += Time.deltaTime;

		if (trailActive)
			InputTrailer.shotInput.Add(Input.GetKey(KeyCode.Z));

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

		if (trailActive)
			InputTrailer.moveInput.Add(inputs);
	}

	public void Damaged() {
		currentHp -= 1;
		if (currentHp < 0) currentHp = 0;
		UpdatePlayerHpUI();
	}

	void UpdatePlayerHpUI() {
		for (int i = 0; i < currentHp; i++) {
			hpOrbImages[i].enabled = true;
		}
		for (int i = currentHp; i < maxHp; i++) {
			hpOrbImages[i].enabled = false;
		}
	}

	void UpdatePlayerBombUI() {
		for (int i = 0; i < currentBomb; i++) {
			bombImages[i].enabled = true;
		}
		for (int i = currentBomb; i < maxBomb; i++) {
			bombImages[i].enabled = false;
		}
	}
}
