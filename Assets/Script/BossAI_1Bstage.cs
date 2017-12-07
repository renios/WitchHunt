using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_1Bstage : MonoBehaviour {

	public float preDelay = 1;

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
	TrailPlayer trailPlayer;

	int pattern = 1;
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

	void ShotSpread() {
		List<GameObject> bullets = new List<GameObject>();
		
		float delta = 360 / (float)spreadBulletCount;
		for (int i = 0; i < spreadBulletCount; i++) {
			GameObject bullet = Instantiate(spreadBullet, transform.position, Quaternion.identity) as GameObject;
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

		float bulletSpeed = laserBulletSpeed;
		if (trailPlayer.IsSlow) bulletSpeed *= 1.5f;

		bullets.ForEach(bullet => {
			bullet.transform.parent = transform;
			LaserBullet lb = bullet.GetComponent<LaserBullet>();
			lb.speed = laserBulletSpeed;
			lb.remainTime = laserRemainTime;
		});
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
		if (trailPlayer.IsSlow) speed *= 0.5f;
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

	void ShotSpecialWall() {
		StartCoroutine(ShotSpecialWallCoroutine());
	}

	IEnumerator ShotSpecialWallCoroutine() {
		List<GameObject> newWallBullets = new List<GameObject>();
	
		int count = specialWallBulletCount1;
		if (trailPlayer.IsSlow) count = specialWallBulletCount2;
		
		float slit = 10.2f / (float)(count - 1);

		for (int i = 0; i < count; i++) {
			GameObject newWallBullet = Instantiate(specialWallBullet, new Vector3(9.5f, 5.1f - slit * i, 0), Quaternion.identity) as GameObject;
			newWallBullet.GetComponent<Bullet>().direction = Vector3.left;
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

	// Use this for initialization
	IEnumerator Start () {
		player = FindObjectOfType<Player>();
		trailPlayer = FindObjectOfType<TrailPlayer>();
		
		yield return new WaitForSeconds(preDelay);

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
			ShotWall();
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
			ShotSpecialWall();
			yield return new WaitForSeconds(specialWallBulletDelay);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
