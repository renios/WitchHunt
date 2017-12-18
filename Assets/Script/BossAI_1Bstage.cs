using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_1Bstage : MonoBehaviour {

	public float preDelay = 1;

	public GameObject spreadBullet;
	public GameObject slowSpreadBullet;
	public float spreadBulletSpeed = 10;
	public int spreadBulletCount = 12;
	public float spreadBulletRotateAngle = 10;
	public float spreadBulletDelay = 0.1f;	
	float lastDelta; 

	public GameObject crossbowBulletObj;
	public float defaultShot1Speed = 10;
	public float defaultShot1Delay = 2;
	public float defaultShot1SectorDelta = 15;

	public GameObject wallBullet;
	public float wallBulletSpeed = 2;
	public float wallBulletHorizSpeed = 0.1f;
	public int wallBulletCount = 15;
	public float wallBulletPreDelay = 1;
	public float wallBulletDelay = 2;
	
	public GameObject followBullet;
	public float followBulletSpeed = 10;
	public float followBulletDelay = 0.25f;

	public GameObject specialLaserBullet;
	public float specialLaserBulletSpeed = 2;
	public int specialLaserCount = 2;
	public float specialLaserAngle = 20;
	public float specialLaserRemainTime = 45;

	public GameObject specialWallBullet;
	public float specialWallBulletSpeed = 1;
	public float specialWallBulletHorizSpeed = 0.1f;
	public int specialWallBulletCount1 = 7;
	public int specialWallBulletCount2 = 10;
	public float specialWallBulletPreDelay = 0;	
	public float specialWallBulletDelay = 1;

	public GameObject specialBullet1Obj;
	public float specialPattern1Delay = 10;

	public GameObject specialBullet2Obj;
	public float specialBullet2Speed = 0.5f;
	public float specialPattern2ShotDelay = 1;
	public float specialBullet2SelfDestroyDelay = 1;

	public GameObject specialBullet2SubObj;
	public float specialBullet2SubSpeed = 2;
	public int specialBullet2SubCount = 60;

	Player player;
	TrailPlayer trailPlayer;

	int pattern = 1;
	public float pattern1Time = 10;
	public float pattern2Time = 10;
	public float pattern3Time = 10;
	public float pattern4Time = 10;

	IEnumerator ChangePattern() {

		List<Coroutine> currentPatterns = new List<Coroutine>();
		Coroutine currentPattern1;
		Coroutine currentPattern2;

		while (true) {
			pattern = 1;
			currentPattern1 = StartCoroutine(Pattern1_1());
			currentPatterns.Add(currentPattern1);
			yield return new WaitForSeconds(pattern1Time);
			currentPatterns.ForEach(pattern => StopCoroutine(pattern));

			pattern = 2;
			currentPattern1 = StartCoroutine(Pattern2_1());
			currentPattern2 = StartCoroutine(Pattern2_2());
			currentPatterns.Add(currentPattern1);
			currentPatterns.Add(currentPattern2);
			yield return new WaitForSeconds(pattern2Time);
			currentPatterns.ForEach(pattern => StopCoroutine(pattern));

			pattern = 3;
			currentPattern1 = StartCoroutine(Pattern4_1());
			currentPattern2 = StartCoroutine(Pattern3_2());
			currentPatterns.Add(currentPattern1);
			currentPatterns.Add(currentPattern2);
			yield return new WaitForSeconds(pattern3Time);
			currentPatterns.ForEach(pattern => StopCoroutine(pattern));

			pattern = 4;
			currentPattern1 = StartCoroutine(Pattern1_1());
			currentPattern2 = StartCoroutine(Pattern2_2());
			currentPatterns.Add(currentPattern1);
			currentPatterns.Add(currentPattern2);
			yield return new WaitForSeconds(pattern4Time);
			currentPatterns.ForEach(pattern => StopCoroutine(pattern));
		}
	}

	void ShotSpread() {
		List<GameObject> bullets = new List<GameObject>();

		int bulletCount = spreadBulletCount;
		if (trailPlayer.IsSlow) bulletCount *= 2;

		float delta = 360 / (float) bulletCount;
		for (int i = 0; i < bulletCount; i++) {
			GameObject bullet;
			if (trailPlayer.IsSlow)
				bullet = Instantiate(slowSpreadBullet, transform.position, Quaternion.identity);
			else 
				bullet = Instantiate(spreadBullet, transform.position, Quaternion.identity);
			Vector3 direction = Utility.GetUnitVector(lastDelta + i * delta);
			bullet.GetComponent<Bullet>().direction = direction;
			bullets.Add(bullet);
		}

		float bulletSpeed = spreadBulletSpeed;
		if (trailPlayer.IsSlow) bulletSpeed /= 2f;
		bullets.ForEach(bullet => bullet.GetComponent<Bullet>().speed = bulletSpeed);

		float bulletRotateAngle = spreadBulletRotateAngle;
		if (trailPlayer.IsSlow) bulletRotateAngle /= 2f;
			lastDelta += bulletRotateAngle;
	}

	void ShotLaser() {
		GameObject upperBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;
		GameObject midBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;
		GameObject lowerBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;

		Vector3 deltaVector = (FindObjectOfType<Player>().transform.position - transform.position).normalized;
		float delta = Mathf.Asin(deltaVector.y) * Mathf.Rad2Deg;

		upperBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(delta - defaultShot1SectorDelta);
		midBullet.GetComponent<Bullet>().direction = deltaVector;
		lowerBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(delta + defaultShot1SectorDelta);

		upperBullet.GetComponent<Bullet>().speed = defaultShot1Speed;
		midBullet.GetComponent<Bullet>().speed = defaultShot1Speed;
		lowerBullet.GetComponent<Bullet>().speed = defaultShot1Speed;

		upperBullet.transform.rotation *= Quaternion.Euler(0,0,delta - defaultShot1SectorDelta);
		midBullet.transform.rotation *= Quaternion.Euler(0,0,delta);
		lowerBullet.transform.rotation *= Quaternion.Euler(0,0,delta + defaultShot1SectorDelta);
	}

	void ShotWall() {
		StartCoroutine(ShotWallCoroutine());
	}

	IEnumerator ShotWallCoroutine() {
		List<GameObject> newWallBullets = new List<GameObject>();
		
		float slit = 10.2f / (float)(wallBulletCount - 1);

		for (int i = 0; i < wallBulletCount; i++) {
			GameObject newWallBullet = Instantiate(wallBullet, new Vector3(0, 5.1f - slit * i, 0), Quaternion.identity) as GameObject;
			newWallBullet.GetComponent<Bullet>().speed = 0;
			newWallBullets.Add(newWallBullet);
		}

		yield return new WaitForSeconds(wallBulletPreDelay);

		float speed = wallBulletSpeed;
		float horizSpeed = wallBulletHorizSpeed*Random.Range(-1.0f, 1.0f);
		if (trailPlayer.IsSlow) speed *= 0.5f;
		int playerDirection = (int)Mathf.Sign(player.transform.position.x);
		foreach (var newWallBullet in newWallBullets) {
			if (newWallBullet != null) {
				Bullet bullet = newWallBullet.GetComponent<Bullet>();
				bullet.GetComponent<Bullet>().direction = new Vector3(1, horizSpeed, 0) * playerDirection;
				bullet.GetComponent<Bullet>().speed = speed;
			}
		}
	}

	void ShotFollow() {
		GameObject newBullet = Instantiate(followBullet, transform.position, Quaternion.identity) as GameObject;
		
		Vector3 deltaVector = (FindObjectOfType<Player>().transform.position - transform.position).normalized;
		float delta = Mathf.Asin(deltaVector.y) * Mathf.Rad2Deg;

		newBullet.GetComponent<Bullet>().direction = deltaVector;
		newBullet.GetComponent<Bullet>().speed = followBulletSpeed;
		newBullet.transform.rotation *= Quaternion.Euler(0,0,delta);
	}

	// Currently not in use
	void ShotSpecialLaser() {
		List<GameObject> bullets = new List<GameObject>();

		GameObject upperBullet = Instantiate(specialLaserBullet, transform.position, Quaternion.identity) as GameObject;
		GameObject lowerBullet = Instantiate(specialLaserBullet, transform.position, Quaternion.identity) as GameObject;
		upperBullet.transform.rotation *= Quaternion.Euler(0,0,specialLaserAngle);
		lowerBullet.transform.rotation *= Quaternion.Euler(0,0,-specialLaserAngle);
		bullets.Add(upperBullet);
		bullets.Add(lowerBullet);

		bullets.ForEach(bullet => {
			bullet.transform.parent = transform;
			LaserBullet lb = bullet.GetComponent<LaserBullet>();
			lb.speed = specialLaserBulletSpeed;
			lb.remainTime = specialLaserRemainTime;
		});
	}

	void ShotSpecialWall() {
		StartCoroutine(ShotSpecialWallCoroutine());
	}

	IEnumerator ShotSpecialWallCoroutine() {
		List<GameObject> newWallBullets = new List<GameObject>();
	
		int count = specialWallBulletCount1;
		if (trailPlayer.IsSlow) count = specialWallBulletCount2;
		
		float slit = 10.2f / (float)(count - 1);
		float horizSpeed = specialWallBulletHorizSpeed*Random.Range(-1.0f, 1.0f);

		for (int i = 0; i < count; i++) {
			GameObject newWallBullet = Instantiate(specialWallBullet, new Vector3(9.5f, 5.1f - slit * i, 0), Quaternion.identity) as GameObject;
			newWallBullet.GetComponent<Bullet>().direction = new Vector3(-1, horizSpeed, 0);
			newWallBullet.GetComponent<Bullet>().speed = 0;
			newWallBullets.Add(newWallBullet);
		}

		yield return new WaitForSeconds(specialWallBulletPreDelay);

		float speed = specialWallBulletSpeed;
		if (trailPlayer.IsSlow) speed *= 0.5f;
		foreach (var newWallBullet in newWallBullets) {
			if (newWallBullet != null) {
				newWallBullet.GetComponent<Bullet>().speed = speed;
			}
		}
	}

	void ShotMushroom(){
		StartCoroutine(ShotMushroomCoroutine());
	}

	IEnumerator ShotMushroomCoroutine(){
		float deltaTime = specialPattern2ShotDelay;

		// 버섯탄은 화면 좌측에서 생성되어 일정 거리를 날아간 후 폭발한다
		
		Vector3 mPosition = new Vector3(-7.5f + Random.Range(-0.5f, 0.5f), Random.Range(-5.0f,5.0f), 0);
		GameObject newBullet = Instantiate(specialBullet2Obj, mPosition, Quaternion.identity) as GameObject;
		Vector3 direction = FindObjectOfType<Player>().transform.position - transform.position;
		direction.y += Random.Range(-0.5f, 0.5f);
		Bullet bullet = newBullet.GetComponent<Bullet>();
		bullet.direction = direction;
		bullet.speed = specialBullet2Speed;
		MushroomBullet mBullet = newBullet.GetComponent<MushroomBullet>();
		mBullet.mushroomSubBulletObj = specialBullet2SubObj;
		mBullet.subBulletCount = specialBullet2SubCount;
		mBullet.subBulletSpeed = specialBullet2SubSpeed;
		mBullet.selfDestroy = true;
		mBullet.selfDestroyDelay = specialBullet2SelfDestroyDelay + Random.Range(0.0f, 3.0f);
		yield return new WaitForSeconds(deltaTime);
	}

	IEnumerator ShotTrapCoroutine() {
		// 격자탄막은 부모오브젝트째로 (0,0)에 생성
		GameObject specialBullet1 = Instantiate(specialBullet1Obj) as GameObject;
		yield return new WaitForSeconds(specialPattern1Delay);
		Destroy(specialBullet1);
	}

	// Use this for initialization
	IEnumerator StartPattern () {
		yield return new WaitForSeconds(preDelay);
		StartCoroutine(DefaultPattern());
		StartCoroutine(ChangePattern());
	}

	void StopPattern() {
		StopAllCoroutines();
	}
	
	bool isStart;
	bool isEnd;
	TextManager textManager;
	Boss boss;

	void Start () {
		isStart = false;
		isEnd = false;
		textManager = FindObjectOfType<TextManager>();
		boss = GetComponent<Boss>();

		player = FindObjectOfType<Player>();
		trailPlayer = FindObjectOfType<TrailPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((!isStart) && (textManager.dialogueState == TextManager.DialogueState.Ingame)) {
			GetComponent<TrailPlayer>().StartPattern();
			GetComponent<Boss>().patternCoroutine = StartCoroutine(StartPattern());
			isStart = true;
		}

		if ((!isEnd) && (textManager.dialogueState == TextManager.DialogueState.After)) {
			GetComponent<TrailPlayer>().StopPattern();
			StopPattern();
			boss.DestroyAllBullets();
			isEnd = true;
		}
	}

	// 기본공격: 유도 레이저탄(고속) / 유도 원형탄(저속)
	IEnumerator DefaultPattern() {
		while (true){	
			if (trailPlayer.IsSlow){
				ShotFollow();
				yield return new WaitForSeconds(followBulletDelay);
			}
			else {
				ShotLaser();
				yield return new WaitForSeconds(defaultShot1Delay);
			}
		}
	}

	// 전방위 원형탄
	IEnumerator Pattern1_1 () {
		while(true) {
			ShotSpread();	
			yield return new WaitForSeconds(spreadBulletDelay);
		}
	}

	// 원형탄 벽
	IEnumerator Pattern2_1 () {
		while (true) {
			ShotWall();
			yield return new WaitForSeconds(wallBulletDelay);
		}
	}

	// 랜덤 버섯탄
	IEnumerator Pattern2_2 () {
		while (true) {
			ShotMushroom();
			yield return new WaitForSeconds(specialPattern2ShotDelay);
		}
	}

	// 특수 패턴 - 레이저
	IEnumerator Pattern3_1 () {
		ShotSpecialLaser();
		yield return null;
	}

	// 특수 패턴 - 이동하는 벽
	IEnumerator Pattern3_2 () {
		while (true) {
			ShotSpecialWall();
			yield return new WaitForSeconds(specialWallBulletDelay);
		}
	}

	IEnumerator Pattern4_1 () {
		while (true){
			yield return StartCoroutine(ShotTrapCoroutine());
		}
	}
}
