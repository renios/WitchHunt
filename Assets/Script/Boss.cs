using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {

	public int maxHp = 120;
	int currentHp;
	public Image hpCircleImage;

	// Use this for initialization
	void Start () {
		InitializeBossHp();
	}
	
	// Update is called once per frame
	void Update () {
		
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
