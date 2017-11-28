using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_1stage : MonoBehaviour {

	public float predelay = 1;

	public GameObject crossbowBulletObj;
	public float defaultShot1Speed = 10;
	public float defaultShot1Delay = 2;
	public float defaultShot1SectorDelta = 15;
	public GameObject seedBulletObj;
	public float defaultShot2Speed = 5;
	public float defaultShot2Delay = 8;
	public float defaultShot2ShotTime = 2;
	public int defaultShot2Count = 80;

	// 특수패턴 1 : 격자탄막
	public GameObject specialBullet1Obj;
	public float specialPattern1PreDelay = 2;
	public float specialPattern1Time = 10;
	public float specialPattern1Delay = 10;

	// 특수패턴 2 : 버섯탄
	public GameObject specialBullet2Obj;
	public float specialBullet2Speed = 0.5f;
	public int specialBullet2Count = 3;
	public float specialPattern2PreDelay = 12;
	public float specialPattern2ShotDelay = 1;
	public float specialPattern2Delay = 12;

	// 특수패턴 2-1 : 버섯포자탄
	public GameObject specialBullet2SubObj;
	public float specialBullet2SubSpeed = 2;
	public int specialBullet2SubCount = 60;

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
				Bullet bullet = newBullet.GetComponent<Bullet>();
				bullet.direction = direction;
				bullet.speed = specialBullet2Speed;
				MushroomBullet mBullet = newBullet.GetComponent<MushroomBullet>();
				mBullet.mushroomSubBulletObj = specialBullet2SubObj;
				mBullet.subBulletCount = specialBullet2SubCount;
				mBullet.subBulletSpeed = specialBullet2SubSpeed;
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

			// Debug.Log(delta);

			upperBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(delta + 180 - defaultShot1SectorDelta);
			midBullet.GetComponent<Bullet>().direction = deltaVector;
			lowerBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(delta + 180 + defaultShot1SectorDelta);

			upperBullet.GetComponent<Bullet>().speed = defaultShot1Speed;
			midBullet.GetComponent<Bullet>().speed = defaultShot1Speed;
			lowerBullet.GetComponent<Bullet>().speed = defaultShot1Speed;

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
				Bullet bullet = newBullet.GetComponent<Bullet>();
				bullet.direction = Utility.GetUnitVector(delta * i);
				bullet.speed = defaultShot2Speed;
				yield return new WaitForSeconds(deltaTime);
			}
			yield return new WaitForSeconds(defaultShot2Delay - deltaTime);
		}
	}
}
