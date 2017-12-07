using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBullet : MonoBehaviour {

	public float delay;
	public float speed;

	Player player;
	Bullet bullet;

	// Use this for initialization
	IEnumerator Start () {
		player = FindObjectOfType<Player>();
		bullet = GetComponent<Bullet>();
		bullet.speed = 0;

		yield return new WaitForSeconds(delay);

		Vector3 destPos = new Vector3(player.transform.position.x + Random.Range(-1f, 1f),
									  player.transform.position.y + Random.Range(-1.5f, 1.5f),
									  0);
		Vector3 direction = (destPos - transform.position).normalized;
		bullet.direction = direction;
		bullet.speed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
