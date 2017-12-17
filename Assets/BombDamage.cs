using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDamage : MonoBehaviour {

	public int damageRatio;
	public int damageAmount;

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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
