using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class Boss : MonoBehaviour {

	public float speed = 0.5f;

	public int maxHp = 120;
	public int currentHp;
	public Image hpCircleImage;

	public Coroutine patternCoroutine;
	TextManager textManager;

	SEManager SEPlayer;

	// Use this for initialization
	void Start () {
		textManager = FindObjectOfType<TextManager>();
		InitializeBossHp();

		SEPlayer = FindObjectOfType<SEManager>();
	}

	public void StartPattern() {
		StartCoroutine(Move());
	}

	public void StopPattern() {
		StopAllCoroutines();
	}
	
	// Update is called once per frame
	void Update () {
		// if ((currentHp < 10) && (textManager.dialogueState == TextManager.DialogueState.Ingame)) {
		// 	// StopCoroutine(patternCoroutine);
		// 	DestroyAllBullets();

		// 	textManager.dialogueState = TextManager.DialogueState.After;
		// }
	}

	IEnumerator Move() {
		yield return new WaitForSeconds(0.5f);

		// 기본위치 (6,0) -> (5,-2) ~ (7, 2) 사이에서 움직임
		while (true) {
			float xPos = Random.Range(5f, 7f);
			float yPos = Random.Range(-2f, 2f);
			Vector3 nextPos = new Vector3(xPos, yPos, transform.position.z);
			float dist = (nextPos - transform.position).magnitude;

			Tween tw = transform.DOMove(nextPos, dist/speed);
			yield return tw.WaitForCompletion();
		}
	}

	public void DestroyAllBullets() {
		if (GameObject.Find("TrapBullets(Clone)") != null) {
			Destroy(GameObject.Find("TrapBullets(Clone)"));
		}
		GameObject.FindGameObjectsWithTag("EnemyBullet").ToList().ForEach(bullet => Destroy(bullet));
	}

	void InitializeBossHp() {
		currentHp = maxHp;
		UpdateBossHpUI();
	}

	public void Damaged(int damage) {
		currentHp -= damage;
		if (currentHp < 0) currentHp = 0;
		UpdateBossHpUI();
	}

	void UpdateBossHpUI() {
		hpCircleImage.fillAmount = currentHp / (float)maxHp;
	}
}
