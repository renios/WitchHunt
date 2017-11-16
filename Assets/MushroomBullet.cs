using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBullet : MonoBehaviour {

	public GameObject mushroomSubBulletObj;
	public int subBulletCount;
	public float rotateSpeed;

	public void DestroyBulletByWall() {
		float delta = 360/(float)subBulletCount;

		for (int i = 0; i < subBulletCount; i++) {
			GameObject newBullet = Instantiate(mushroomSubBulletObj, transform.position, Quaternion.identity) as GameObject;
			newBullet.GetComponent<Bullet>().direction = Utility.GetUnitVector(delta * i);
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
