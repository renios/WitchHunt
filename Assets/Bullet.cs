using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed;
	public int damage;

	public Vector3 direction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction * Time.deltaTime * speed;

		if (Mathf.Abs(transform.position.x) > 10*2 || Mathf.Abs(transform.position.y) > 6*2) {
			DestroyBullet();
		}
	}

	public void DestroyBullet() {
		if (GetComponent<TrapBullet>() != null) {
			GetComponent<TrapBullet>().InactiveNow();
		}
		else {
			Destroy(gameObject);
		}
	}
}
