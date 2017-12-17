using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RemoveEnemyBullets : MonoBehaviour {

	float radius;
	public float lifeTime;

	// Use this for initialization
	void Start () {
		radius = GetComponent<CircleCollider2D>().radius;

		if (lifeTime != 0)
			Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		Collider2D[] enemyBulletCollidersArray = Physics2D.OverlapCircleAll(transform.position, radius);
		List<Collider2D> enemyBulletColliders = enemyBulletCollidersArray.ToList();

		if (gameObject.tag == "MushroomBomb") {
			enemyBulletColliders = enemyBulletColliders.FindAll(
				coll => coll.GetComponent<MushroomBullet>() == null
			);
		}

		enemyBulletColliders.ForEach(coll => {
			if (coll.tag == "EnemyBullet") {
				if (coll.GetComponent<LaserBullet>() == null)
					coll.GetComponent<Bullet>().DestroyBullet();
			}
		});	
	}
}
