using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_1stage : MonoBehaviour {

	public float predelay;

	public GameObject crossbowBulletObj;
	public float defaultShot1Delay;
	public float defaultShot1SectorDelta;
	public GameObject seedBulletObj;
	public float defaultShot2Delay;
	public float defaultShot2ShotTime;
	public int defaultShot2Count;

	// 특수패턴 1 : 격자탄막
	public GameObject specialBullet1Obj;
	public float specialPattern1PreDelay;
	public float specialPattern1Time;
	public float specialPattern1Delay;

	// 특수패턴 2 : 버섯탄
	public GameObject specialBullet2Obj;
	public int specialBullet2Count;
	public float specialPattern2PreDelay;
	public float specialPattern2ShotDelay;
	public float specialPattern2Delay;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(predelay);
		StartCoroutine(DefaultShot1());
		StartCoroutine(DefaultShot2());
		StartCoroutine(SpecialPattern1());
		StartCoroutine(SpecialPattern2());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator SpecialPattern1() {
		yield return new WaitForSeconds(specialPattern1PreDelay);
		while (true) {
			// 격자탄막은 부모오브젝트째로 (0,0)에 생성
			GameObject specialBullet1 = Instantiate(specialBullet1Obj) as GameObject;
			yield return new WaitForSeconds(specialPattern1Time);
			Destroy(specialBullet1);
			yield return new WaitForSeconds(specialPattern1Delay);
		}
	}

	IEnumerator SpecialPattern2() {
		yield return new WaitForSeconds(specialPattern2PreDelay);

		float deltaTime = specialPattern2ShotDelay;

		while (true) {
			// 버섯탄은 플레이어 방향으로 날아간다
			for (int i = 0; i < specialBullet2Count; i++) {
				GameObject newBullet = Instantiate(specialBullet2Obj, transform.position, Quaternion.identity) as GameObject;
				Vector3 direction = FindObjectOfType<Player>().transform.position - transform.position;
				newBullet.GetComponent<Bullet>().direction = direction;
				yield return new WaitForSeconds(deltaTime);
			}
			yield return new WaitForSeconds(specialPattern2Delay - deltaTime * specialBullet2Count);
		}
	}

	IEnumerator DefaultShot1() {
		while (true) {
			GameObject upperBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;
			GameObject midBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;
			GameObject lowerBullet = Instantiate(crossbowBulletObj, transform.position, Quaternion.identity) as GameObject;

			Vector3 deltaVector = (FindObjectOfType<Player>().transform.position - transform.position).normalized;
			float delta = -1 * Mathf.Asin(deltaVector.y) * Mathf.Rad2Deg;

			Debug.Log(delta);

			upperBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(delta + 180 - defaultShot1SectorDelta);
			midBullet.GetComponent<Bullet>().direction = deltaVector;
			lowerBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(delta + 180 + defaultShot1SectorDelta);

			upperBullet.transform.rotation *= Quaternion.Euler(0,0,delta + 180 - defaultShot1SectorDelta);
			midBullet.transform.rotation *= Quaternion.Euler(0,0,delta);
			lowerBullet.transform.rotation *= Quaternion.Euler(0,0,delta + 180 + defaultShot1SectorDelta);

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
