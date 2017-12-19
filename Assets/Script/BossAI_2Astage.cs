using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_2Astage : MonoBehaviour {

	public float preDelay = 1;

	public GameObject spreadBullet;
	public float spreadBulletSpeed = 3;
	public int spreadBulletCount = 20;
	public float spreadBulletRotateAngle = 30;
	public float spreadBulletDelay = 0.1f;	
	public int spreadBulletShotCount = 9;
	public float spreadBulletPatternDelay = 3;
	float lastDelta; 

	public GameObject rockBullet;
	public float rockBulletSpeed = 12;
	public int rockBulletCount = 10;
	public float rockBulletMaxPreDelay = 2;
	public float rockBulletPatternDelay = 5;

	public GameObject fenceBullet;
	public float fenceBulletInitSpeed = 5;
	public float fenceBulletTangent = 40;
	public float fenceBulletDelay = 1;

	// 사운드
	SEManager SEPlayer;

	// 1패턴 - 칼바람
	void ShotSpread(Vector3 centerPos, bool isRotateClockwise) {
		List<GameObject> bullets = new List<GameObject>();
		
		float delta = 360 / (float)spreadBulletCount;
		for (int i = 0; i < spreadBulletCount; i++) {
			GameObject bullet = Instantiate(spreadBullet, centerPos, Quaternion.identity) as GameObject;
			Vector3 direction = Utility.GetUnitVector(lastDelta + i * delta);
			bullet.GetComponent<Bullet>().direction = direction;
			
			Vector3 deltaVector = direction;
			float deltaDeg = -1 * Mathf.Asin(deltaVector.y) * Mathf.Rad2Deg;
			if (isRotateClockwise)
				bullet.transform.rotation *= Quaternion.Euler(0,0,deltaDeg + 45);
			else
				bullet.transform.rotation *= Quaternion.Euler(0,0,deltaDeg - 45);

			bullets.Add(bullet);
		}

		bullets.ForEach(bullet => bullet.GetComponent<Bullet>().speed = spreadBulletSpeed);
	}

	IEnumerator ShotTornado(Vector3 centerPos) {
		for (int i = 0; i < spreadBulletShotCount; i++) {
			if (i >= spreadBulletShotCount/3 && i < spreadBulletShotCount*2/3) {
				ShotSpread(centerPos, true);
				lastDelta += spreadBulletRotateAngle;
			}
			else {
				ShotSpread(centerPos, false);
				lastDelta -= spreadBulletRotateAngle;
			}
			yield return new WaitForSeconds(spreadBulletDelay);
		}
	}

	IEnumerator Pattern1() {
		while (true) {
			float xPos = Random.Range(transform.position.x - 3, transform.position.x + 3);
			float yPos = Random.Range(transform.position.y - 4, transform.position.y + 4);
			StartCoroutine(ShotTornado(new Vector3(xPos, yPos, 0)));
			yield return new WaitForSeconds(spreadBulletPatternDelay);
		}
	}

	// 2패턴 - 낙석
	void ShotRock() {
		List<GameObject> bullets = new List<GameObject>();

		for (int i = 0; i < rockBulletCount; i++) {
			Vector3 initPos = new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f),
										  transform.position.y + Random.Range(-2.5f, 2.5f),
										  0);
			GameObject bullet = Instantiate(rockBullet, initPos, Quaternion.identity) as GameObject;
			float scale = Random.Range(0.5f, 1.5f);
			bullet.transform.localScale = new Vector3(scale, scale, 1);
			
			RockBullet rb = bullet.GetComponent<RockBullet>();
			rb.delay = Random.Range(0, rockBulletMaxPreDelay) + 0.5f;
			rb.speed = rockBulletSpeed;

			bullets.Add(bullet);
		}
	}

	IEnumerator Pattern2() {
		while (true) {
			ShotRock();
			yield return new WaitForSeconds(rockBulletPatternDelay);
		}
	}

	// 3패턴 - 펜스
	// 홀수번째에는 4개 짝수번째에는 3개
	void ShotFence(bool isEven) {
		List<GameObject> bullets = new List<GameObject>();

		if (isEven) {
			float interval = 10 / 8f;
			for (int i = 0; i < 4; i++) {
				Vector3 initPos = new Vector3(7f, -5 + interval*((i+1)*2 -1), 0);
				GameObject bullet = Instantiate(fenceBullet, initPos, Quaternion.identity) as GameObject;
				bullet.GetComponent<Bullet>().direction = Vector3.left;
				bullet.GetComponent<FenceBullet>().initSpeed = fenceBulletInitSpeed;
				
				bullets.Add(bullet);
			}
			bullets.ForEach(bullet => bullet.transform.rotation *= Quaternion.Euler(0,0,fenceBulletTangent));
		}
		else {
			float interval = 10 / 6f;
			for (int i = 0; i < 3; i++) {
				Vector3 initPos = new Vector3(7f, -5 + interval*((i+1)*2 -1), 0);
				GameObject bullet = Instantiate(fenceBullet, initPos, Quaternion.identity) as GameObject;
				bullet.GetComponent<Bullet>().direction = Vector3.left;
				bullet.GetComponent<FenceBullet>().initSpeed = fenceBulletInitSpeed;
				
				bullets.Add(bullet);
			}
			bullets.ForEach(bullet => bullet.transform.rotation *= Quaternion.Euler(0,0,-fenceBulletTangent));
		}
	}

	IEnumerator Pattern3() {
		bool isEven = false;
		while (true) {
			ShotFence(isEven);
			if (!isEven) {				
				isEven = true;
			}
			else {
				isEven = false;
			}
			yield return new WaitForSeconds(fenceBulletDelay);
		}
	}

	public bool pattern1;
	public bool pattern2;
	public bool pattern3;

	IEnumerator StartPattern() {
		yield return new WaitForSeconds(preDelay);

		if (pattern1) StartCoroutine(Pattern1());
		if (pattern2) StartCoroutine(Pattern2());
		if (pattern3) StartCoroutine(Pattern3());
	}

	void StopPattern() {
		StopAllCoroutines();
	}
	
	bool isStart;
	TextManager textManager;
	Boss boss;

	void Start () {
		isStart = false;
		textManager = FindObjectOfType<TextManager>();
		boss = GetComponent<Boss>();

		SEPlayer = FindObjectOfType<SEManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((!isStart) && (textManager.dialogueState == TextManager.DialogueState.Ingame)) {
			GetComponent<Boss>().patternCoroutine = StartCoroutine(StartPattern());
			isStart = true;
		}

		if ((boss.currentHp < boss.maxHp/10f) && (textManager.dialogueState == TextManager.DialogueState.Ingame)) {
			StopPattern();
			boss.DestroyAllBullets();

			SEPlayer.Play(SEManager.Sounds.EnemyDeath);
			textManager.dialogueState = TextManager.DialogueState.After;
		}
	}
}
