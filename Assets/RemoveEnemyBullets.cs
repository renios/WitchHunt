using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RemoveEnemyBullets : MonoBehaviour {

	float radius;

	// Use this for initialization
	void Start () {
		radius = GetComponent<CircleCollider2D>().radius;

		Destroy(gameObject, 0.8f);
	}
	
	// Update is called once per frame
	void Update () {
		Collider2D[] enemyBulletColliders = Physics2D.OverlapCircleAll(transform.position, radius);
		if (enemyBulletColliders.ToList().Any(coll => coll.tag == "EnemyBullet")) {
			// Instantiate(hitParticle, transform.position, Quaternion.identity);
		}
		enemyBulletColliders.ToList().ForEach(coll => {
			if (coll.tag == "EnemyBullet") {
				if (coll.GetComponent<LaserBullet>() == null)
					coll.GetComponent<Bullet>().DestroyBullet();
			}
		});	
	}
}
