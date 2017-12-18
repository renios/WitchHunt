using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDamage : MonoBehaviour {

	public int damageRatio;
	public int damageAmount;
	public GameObject hitParticle;

	// Use this for initialization
	void Start () {
		Boss boss = FindObjectOfType<Boss>();
		
		if (damageRatio > 0) {
			int damage = (int)((damageRatio / 100f) * boss.maxHp);
			boss.Damaged(damage);
		}

		if (damageAmount > 0) {
			boss.Damaged(damageAmount);
		}

		if ((damageRatio > 0 || damageAmount > 0)) {
			if (hitParticle == null) return;
			GameObject particle = Instantiate(hitParticle, boss.transform.position + Vector3.back * 2, Quaternion.identity);
			Destroy(particle, 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
