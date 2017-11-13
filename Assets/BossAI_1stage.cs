using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_1stage : MonoBehaviour {

	public GameObject crossbowBulletObj;
	public float defaultShot1Delay;
	public GameObject seedBulletObj;
	public float defaultShot2Delay;

	// Use this for initialization
	void Start () {
		StartCoroutine(DefaultShot1());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator DefaultShot1() {
		while (true) {
			float sectorDelta = 15;

			GameObject upperBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;
			GameObject midBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;
			GameObject lowerBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;

			upperBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(180 - sectorDelta);
			midBullet.GetComponent<Bullet>().direction = Vector3.left;
			lowerBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(180 + sectorDelta);

			upperBullet.transform.rotation *= Quaternion.Euler(0,0,180 - sectorDelta);
			lowerBullet.transform.rotation *= Quaternion.Euler(0,0,180 + sectorDelta);

			upperBullet.tag = "EnemyBullet";
			midBullet.tag = "EnemyBullet";
			lowerBullet.tag = "EnemyBullet";
	
			yield return new WaitForSeconds(defaultShot1Delay);
		}
	}

	IEnumerator DefaultShot2() {
		while (true) {
			yield return new WaitForSeconds(defaultShot2Delay);
		}
	}
}
