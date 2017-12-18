using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour {

	public float speed;
	public float remainTime;

	// Use this for initialization
	void Start () {
		GetComponent<Bullet>().speed = 0; // 레이저는 전진하지 않음
		Destroy(gameObject, remainTime);
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<LaserController>().length += speed;
	}
}
