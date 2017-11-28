using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBullet : MonoBehaviour {

	public GameObject mushroomSubBulletObj;
	public float subBulletSpeed;
	public int subBulletCount;
	public float rotateSpeed;

	public void DestroyBulletByWall() {
		float delta = 360/(float)subBulletCount;

		for (int i = 0; i < subBulletCount; i++) {
			GameObject newBullet = Instantiate(mushroomSubBulletObj, transform.position, Quaternion.identity) as GameObject;
			Bullet bullet = newBullet.GetComponent<Bullet>();
			bullet.direction = Utility.GetUnitVector(delta * i);
			bullet.speed = subBulletSpeed;
		}

		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * rotateSpeed);	
	}
}
