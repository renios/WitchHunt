using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class TrapBulletManager : MonoBehaviour {

	public float speed;
	public float moveRadius;
	List<TrapBullet> trapBullets;

	// Use this for initialization
	IEnumerator Start () {
		trapBullets = FindObjectsOfType<TrapBullet>().ToList();
		yield return new WaitForSeconds(FindObjectOfType<TrapBullet>().activeDelay);
		StartCoroutine(Move());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Move() {
		// 원운동으로 바꿀것
		Tween tween;
		while (true) {
			tween = transform.DOMove(new Vector3(moveRadius, moveRadius, 0), 1/speed, false).SetEase(Ease.Linear);
			yield return tween.WaitForCompletion();
			tween = transform.DOMove(new Vector3(0, moveRadius * 2, 0), 1/speed, false).SetEase(Ease.Linear);
			yield return tween.WaitForCompletion();
			tween = transform.DOMove(new Vector3(-moveRadius, moveRadius, 0), 1/speed, false).SetEase(Ease.Linear);
			yield return tween.WaitForCompletion();
			tween = transform.DOMove(new Vector3(0, 0, 0), 1/speed, false).SetEase(Ease.Linear);
			yield return tween.WaitForCompletion();

			trapBullets.ForEach(bullet => {
				if (bullet.isActive == false) {
					StartCoroutine(bullet.Active());
				}
			});
			yield return new WaitForSeconds(FindObjectOfType<TrapBullet>().activeDelay);
		}
	}
}
