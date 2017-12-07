using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_2Astage : MonoBehaviour {

	public float preDelay = 1;

	public GameObject spreadBullet;
	public float spreadBulletSpeed = 10;
	public int spreadBulletCount = 20;
	public float spreadBulletRotateAngle = 10;
	public float spreadBulletDelay = 0.1f;	
	public int spreadBulletShotCount = 9;
	public float spreadBulletTotalDelay = 3;
	float lastDelta; 

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
			yield return new WaitForSeconds(spreadBulletTotalDelay);
		}
	}

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(preDelay);

		StartCoroutine(Pattern1());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
