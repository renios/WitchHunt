using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RemoveEnemyBullets : MonoBehaviour {

	float radius;
	public float lifeTime;
	Collider2D coll;

	// Use this for initialization
	void Start () {
		if (GetComponent<CircleCollider2D>() != null)
			radius = GetComponent<CircleCollider2D>().radius;
		
		if (lifeTime != 0)
			Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		List<Collider2D> enemyBulletColliders = new List<Collider2D>();
		
		if (GetComponent<CircleCollider2D>() != null) {
			Collider2D[] enemyBulletCollidersArray;
			enemyBulletCollidersArray = Physics2D.OverlapCircleAll(transform.position, radius);
			enemyBulletColliders = enemyBulletCollidersArray.ToList();
		}	
		else {			
			List<GameObject> enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet").ToList();
			enemyBullets.ForEach(bullet => enemyBulletColliders.Add(bullet.GetComponent<Collider2D>()));
		}
			
		
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
