using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FenceBullet : MonoBehaviour {

	Bullet bullet;
	SpriteRenderer sr;
	Collider2D coll;

	public float initSpeed;

	// Use this for initialization
	IEnumerator Start () {
		bullet = GetComponent<Bullet>();
		bullet.speed = initSpeed;

		sr = GetComponent<SpriteRenderer>();
		coll = GetComponent<BoxCollider2D>();

		sr.color = new Color(1,1,1,0);

		Tween tw = sr.DOFade(1, 1);
		yield return tw.WaitForCompletion();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x < -11) 
			Destroy(gameObject);

		// 7부터 시작, -12에서 0이 되도록
		bullet.speed = initSpeed * ((transform.position.x + 12f) / 19f);
	}
}
