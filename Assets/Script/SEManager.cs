using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour {

	public enum Sounds {Bomb, Dead, EnemyDeath, Laser, LaserAlert, PlayerShot, Select};
	public List<AudioClip> SEList;

	AudioSource BombSound;
	AudioSource DeathSound;
	AudioSource EnemyDeathSound;
	AudioSource LaserSound;
	AudioSource LaserAlertSound;
	AudioSource PlayerShotSound;
	AudioSource SelectSound;
	


	// Use this for initialization
	void Awake () {
		BombSound.clip = SEList[0];
		DeathSound.clip = SEList[1];
		EnemyDeathSound.clip = SEList[2];
		LaserSound.clip = SEList[3];
		LaserAlertSound.clip = SEList[4];
		PlayerShotSound.clip = SEList[5];
		SelectSound.clip = SEList[6];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(Sounds s){
		switch(s){
			case Sounds.Bomb:
				if (BombSound != null)
					BombSound.Play();
				Debug.Log("BombSound\n");
				break;
			case Sounds.Dead:
				if (DeathSound != null)
					DeathSound.Play();
				break;
			case Sounds.EnemyDeath:
				if (EnemyDeathSound != null)
					EnemyDeathSound.Play();
				break;
			case Sounds.Laser:
				if (LaserSound != null)
					LaserSound.Play();
				break;
			case Sounds.LaserAlert:
				if (LaserAlertSound != null)
					LaserAlertSound.Play();
				break;
			case Sounds.PlayerShot:
				if (PlayerShotSound != null)
					PlayerShotSound.Play();
				break;
			case Sounds.Select:
				if (SelectSound != null)
					SelectSound.Play();
				break;
			default:
				break;
		}
	}
}
