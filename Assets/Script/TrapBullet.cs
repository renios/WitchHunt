using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrapBullet : MonoBehaviour {

	public Collider2D coll;
	public float activeDelay;

	public bool isActive;

	// Use this for initialization
	void Start () {
		StartCoroutine(Active());
	}

	public IEnumerator Active () {
		GetComponent<SpriteRenderer>().DOFade(1, activeDelay);
		yield return new WaitForSeconds(activeDelay * 0.8f);
		coll.enabled = true;
		isActive = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InactiveNow() {
		StopAllCoroutines();
		GetComponent<SpriteRenderer>().DOFade(0, 0);
		coll.enabled = false;
		isActive = false;
	}

	public IEnumerator Inactive() {
		GetComponent<SpriteRenderer>().DOFade(0, activeDelay);
		yield return new WaitForSeconds(activeDelay * 0.5f);
		coll.enabled = false;
		isActive = false;
	}
}
