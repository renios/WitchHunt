using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_1stage : MonoBehaviour {

	public GameObject crossbowBulletObj;
	public float defaultShot1Delay;
	public float defaultShot1SectorDelta;
	public GameObject seedBulletObj;
	public float defaultShot2Delay;
	public float defaultShot2ShotTime;
	public int defaultShot2Count;

	public GameObject specialBullet1Obj;
	public GameObject specialBullet2Obj;

	// Use this for initialization
	void Start () {
		StartCoroutine(DefaultShot1());
		StartCoroutine(DefaultShot2());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator DefaultShot1() {
		while (true) {
			GameObject upperBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;
			GameObject midBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;
			GameObject lowerBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;

			upperBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(180 - defaultShot1SectorDelta);
			midBullet.GetComponent<Bullet>().direction = Vector3.left;
			lowerBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(180 + defaultShot1SectorDelta);

			upperBullet.transform.rotation *= Quaternion.Euler(0,0,180 - defaultShot1SectorDelta);
			lowerBullet.transform.rotation *= Quaternion.Euler(0,0,180 + defaultShot1SectorDelta);

			upperBullet.tag = "EnemyBullet";
			midBullet.tag = "EnemyBullet";
			lowerBullet.tag = "EnemyBullet";
	
			yield return new WaitForSeconds(defaultShot1Delay);
		}
	}

	IEnumerator DefaultShot2() {
		// 0초에는 쏘지 말자
		yield return new WaitForSeconds(defaultShot2Delay);

		float delta = 360/(float)defaultShot2Count;
		float deltaTime = defaultShot2ShotTime/(float)defaultShot2Count;

		while (true) {
			for (int i = 0; i < defaultShot2Count; i++) {
				GameObject newBullet = Instantiate(seedBulletObj, transform.position, Quaternion.identity) as GameObject;
				newBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(delta * i);
				yield return new WaitForSeconds(deltaTime);
			}
			yield return new WaitForSeconds(defaultShot2Delay - deltaTime);
		}
	}
}
