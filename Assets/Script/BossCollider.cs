using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour {

	Boss boss;

	// Use this for initialization
	void Start () {
		boss = FindObjectOfType<Boss>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerBullet") {
			boss.Damaged(other.GetComponent<Bullet>().damage);
			Destroy(other.gameObject);
		}
	}
}
