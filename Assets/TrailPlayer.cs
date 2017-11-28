using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPlayer : MonoBehaviour {

	public GameObject spreadBullet;
	public int spreadBulletCount;
	public float spreadBulletRotateAngle;
	public float spreadBulletDelay;	
	float lastDelta; 

	public GameObject wallBullet;
	public int wallBulletCount;
	public int preDelayWallBullet;
	public int wallBulletSpeed;

	public GameObject followBullet;

	public GameObject specialWallBullet;
	public int specialWallBulletCount1;
	public int specialWallBulletCount2;
	public int preDelaySpecialWallBullet;
	public int specialWallBulletSpeed;

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
		}

		lastDelta += spreadBulletRotateAngle;
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

		yield return new WaitForSeconds(preDelayWallBullet);

		float speed = wallBulletSpeed;
		if (isSlow) speed *= 0.5f;
		int playerDirection = (int)Mathf.Sign(player.transform.position.x);
		foreach (var newWallBullet in newWallBullets) {
			if (newWallBullet != null) {
				newWallBullet.GetComponent<Bullet>().direction = Vector3.right * playerDirection;
				newWallBullet.GetComponent<Bullet>().speed = speed;
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

		yield return new WaitForSeconds(preDelaySpecialWallBullet);

		float speed = specialWallBulletSpeed;
		if (isSlow) speed *= 0.5f;
		foreach (var newWallBullet in newWallBullets) {
			if (newWallBullet != null) {
				newWallBullet.GetComponent<Bullet>().speed = speed;
			}
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
		// StartCoroutine (MoveByTrail());
		// StartCoroutine(pattern1());
		// StartCoroutine(pattern2());
		StartCoroutine(SpreadPattern());
	}

	IEnumerator Pattern1 () {
		while(true) {
			ShotFollow();
			yield return new WaitForSeconds(0.25f);
		}
	}

	IEnumerator Pattern2 () {
		while (true) {
			ShotWall(false);
			yield return new WaitForSeconds(2);
		}
	}

	IEnumerator SpreadPattern() {
		while (true) {
			ShotSpread();
			yield return new WaitForSeconds(spreadBulletDelay);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
