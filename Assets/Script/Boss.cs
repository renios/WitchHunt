using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Boss : MonoBehaviour {

	public int maxHp = 120;
	public int currentHp;
	public Image hpCircleImage;

	public Coroutine patternCoroutine;
	TextManager textManager;

	// Use this for initialization
	void Start () {
		textManager = FindObjectOfType<TextManager>();
		InitializeBossHp();
	}
	
	// Update is called once per frame
	void Update () {
		// if ((currentHp < 10) && (textManager.dialogueState == TextManager.DialogueState.Ingame)) {
		// 	// StopCoroutine(patternCoroutine);
		// 	DestroyAllBullets();

		// 	textManager.dialogueState = TextManager.DialogueState.After;
		// }
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
