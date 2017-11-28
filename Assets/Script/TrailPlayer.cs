using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPlayer : MonoBehaviour {

	public GameObject spreadBullet;
	public float spreadBulletSpeed = 10;
	public int spreadBulletCount = 12;
	public float spreadBulletRotateAngle = 10;
	public float spreadBulletDelay = 0.1f;	
	float lastDelta; 

	public GameObject laserBullet;
	public float laserBulletSpeed = 1;
	public int laserCount = 5;
	public float laserAngle = 15;
	public float laserRemainTime = 2f;
	public float laserBulletDelay = 0.5f;

	public GameObject wallBullet;
	public float wallBulletSpeed = 2;
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
	public int specialWallBulletCount1 = 7;
	public int specialWallBulletCount2 = 10;
	public float specialWallBulletPreDelay = 0;	
	public float specialWallBulletDelay = 1;

	Player player;

	float lastShotTime = 0;

	public int pattern = 1;
	public float pattern1Time = 10;
	public float pattern2Time = 10;
	public float pattern3Time = 45;

	IEnumerator ChangePattern() {

		List<Coroutine> currentPatterns = new List<Coroutine>();
		Coroutine currentPattern1;
		Coroutine currentPattern2;

		while (true) {
			pattern = 1;
			currentPattern1 = StartCoroutine(Pattern1_1());
			currentPattern2 = StartCoroutine(Pattern1_2());
			currentPatterns.Add(currentPattern1);
			currentPatterns.Add(currentPattern2);
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
			currentPattern1 = StartCoroutine(Pattern3_1());
			currentPattern2 = StartCoroutine(Pattern3_2());
			currentPatterns.Add(currentPattern1);
			currentPatterns.Add(currentPattern2);
			yield return new WaitForSeconds(pattern3Time);
			currentPatterns.ForEach(pattern => StopCoroutine(pattern));
		}
	}

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
					// ShotStraightStrong();
				}
				else {
					// ShotSector();
				}
				lastShotTime = 0;
			}

			yield return null;
		}
	}

	void ShotSpread() {
		List<GameObject> bullets = new List<GameObject>();
		
		float delta = 360 / (float)spreadBulletCount;
		for (int i = 0; i < spreadBulletCount; i++) {
			GameObject bullet = Instantiate(spreadBullet, transform.position, Quaternion.identity) as GameObject;
			Vector3 direction = Utility.GetUnitVector(lastDelta + i * delta);
			bullet.GetComponent<Bullet>().direction = direction;
			bullets.Add(bullet);
		}

		bullets.ForEach(bullet => bullet.GetComponent<Bullet>().speed = spreadBulletSpeed);

		lastDelta += spreadBulletRotateAngle;
	}

	void ShotLaser() {
		List<GameObject> bullets = new List<GameObject>();

		Vector3 deltaVector = (FindObjectOfType<Player>().transform.position - transform.position).normalized;
		float delta = Mathf.Asin(deltaVector.y) * Mathf.Rad2Deg;

		if (laserCount % 2 != 0) {
			GameObject midBullet = Instantiate(laserBullet, transform.position, Quaternion.identity) as GameObject;
			midBullet.transform.rotation *= Quaternion.Euler(0,0,delta);
			bullets.Add(midBullet);
			
			for (int i = 0; i < laserCount/2; i++) {
				GameObject upperBullet = Instantiate(laserBullet, transform.position, Quaternion.identity) as GameObject;
				GameObject lowerBullet = Instantiate(laserBullet, transform.position, Quaternion.identity) as GameObject;
				upperBullet.transform.rotation *= Quaternion.Euler(0,0,delta - laserAngle * (i + 1));
				lowerBullet.transform.rotation *= Quaternion.Euler(0,0,delta + laserAngle * (i + 1));
				bullets.Add(upperBullet);
				bullets.Add(lowerBullet);
			}
		}
		else {
			GameObject upperBullet = Instantiate(laserBullet, transform.position, Quaternion.identity) as GameObject;
			GameObject lowerBullet = Instantiate(laserBullet, transform.position, Quaternion.identity) as GameObject;
			upperBullet.transform.rotation *= Quaternion.Euler(0,0,delta - laserAngle/2f);
			lowerBullet.transform.rotation *= Quaternion.Euler(0,0,delta + laserAngle/2f);
			bullets.Add(upperBullet);
			bullets.Add(lowerBullet);
			
			for (int i = 0; i < laserCount/2; i++) {
				upperBullet = Instantiate(laserBullet, transform.position, Quaternion.identity) as GameObject;
				lowerBullet = Instantiate(laserBullet, transform.position, Quaternion.identity) as GameObject;
				upperBullet.transform.rotation *= Quaternion.Euler(0,0,delta - laserAngle * (i + 0.5f));
				lowerBullet.transform.rotation *= Quaternion.Euler(0,0,delta + laserAngle * (i + 0.5f));
				bullets.Add(upperBullet);
				bullets.Add(lowerBullet);
			}
		}

		bullets.ForEach(bullet => {
			LaserBullet lb = bullet.GetComponent<LaserBullet>();
			lb.speed = laserBulletSpeed;
			lb.remainTime = laserRemainTime;
		});
	}

	void ShotWall(bool isSlow) {
		StartCoroutine(ShotWallCoroutine(isSlow));
	}

	IEnumerator ShotWallCoroutine(bool isSlow) {
		List<GameObject> newWallBullets = new List<GameObject>();
		
		float slit = 10.2f / (float)(wallBulletCount - 1);

		for (int i = 0; i < wallBulletCount; i++) {
			GameObject newWallBullet = Instantiate(wallBullet, new Vector3(0, 5.1f - slit * i, 0), Quaternion.identity) as GameObject;
			newWallBullet.GetComponent<Bullet>().speed = 0;
			newWallBullets.Add(newWallBullet);
		}

		yield return new WaitForSeconds(wallBulletPreDelay);

		float speed = wallBulletSpeed;
		if (isSlow) speed *= 0.5f;
		int playerDirection = (int)Mathf.Sign(player.transform.position.x);
		foreach (var newWallBullet in newWallBullets) {
			if (newWallBullet != null) {
				Bullet bullet = newWallBullet.GetComponent<Bullet>();
				bullet.GetComponent<Bullet>().direction = Vector3.right * playerDirection;
				bullet.GetComponent<Bullet>().speed = speed;
			}
		}
	}

	void ShotFollow() {
		GameObject newBullet = Instantiate(followBullet, transform.position, Quaternion.identity) as GameObject;
		
		Vector3 deltaVector = (FindObjectOfType<Player>().transform.position - transform.position).normalized;
		float delta = -1 * Mathf.Asin(deltaVector.y) * Mathf.Rad2Deg;

		newBullet.GetComponent<Bullet>().direction = deltaVector;
		newBullet.transform.rotation *= Quaternion.Euler(0,0,delta);
	}

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

	void ShotSpecialWall(bool isSlow) {
		StartCoroutine(ShotSpecialWallCoroutine(isSlow));
	}

	IEnumerator ShotSpecialWallCoroutine(bool isSlow) {
		List<GameObject> newWallBullets = new List<GameObject>();
	
		int count = specialWallBulletCount1;
		if (isSlow) count = specialWallBulletCount2;
		
		float slit = 10.2f / (float)(count - 1);

		for (int i = 0; i < count; i++) {
			GameObject newWallBullet = Instantiate(specialWallBullet, new Vector3(9.5f, 5.1f - slit * i, 0), Quaternion.identity) as GameObject;
			newWallBullet.GetComponent<Bullet>().direction = Vector3.left;
			newWallBullet.GetComponent<Bullet>().speed = 0;
			newWallBullets.Add(newWallBullet);
		}

		yield return new WaitForSeconds(specialWallBulletPreDelay);

		float speed = specialWallBulletSpeed;
		if (isSlow) speed *= 0.5f;
		foreach (var newWallBullet in newWallBullets) {
			if (newWallBullet != null) {
				newWallBullet.GetComponent<Bullet>().speed = speed;
			}
		}
	}

	// Use this for initialization
	IEnumerator Start () {
		player = FindObjectOfType<Player>();
		
		yield return new WaitForSeconds(1);

		if (InputTrailer.moveInput != null)
			StartCoroutine (MoveByTrail());
		StartCoroutine(ChangePattern());
	}

	// 전방위 원형탄
	IEnumerator Pattern1_1 () {
		while(true) {
			ShotSpread();	
			yield return new WaitForSeconds(spreadBulletDelay);
		}
	}

	// 유도 레이저탄
	IEnumerator Pattern1_2 () {
		while(true) {
			ShotLaser();	
			yield return new WaitForSeconds(laserBulletDelay);
		}
	}

	// 원형탄 벽
	IEnumerator Pattern2_1 () {
		while (true) {
			ShotWall(false);
			yield return new WaitForSeconds(wallBulletDelay);
		}
	}

	// 유도 원형탄
	IEnumerator Pattern2_2 () {
		while (true) {
			ShotFollow();
			yield return new WaitForSeconds(followBulletDelay);
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
			ShotSpecialWall(false);
			yield return new WaitForSeconds(specialWallBulletDelay);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
