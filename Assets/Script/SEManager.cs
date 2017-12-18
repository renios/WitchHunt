using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour {

	public enum Sounds {Bomb, Dead, EnemyDeath, Laser, LaserAlert, PlayerShot, Select};
	public List<AudioSource> SEList;

	int cooldown=0;

	// Use this for initialization
	void Awake () {
		SEList[0].enabled = true;
		SEList[1].enabled = true;
		SEList[2].enabled = true;
		SEList[3].enabled = true;
		SEList[4].enabled = true;
		SEList[5].enabled = true;
		SEList[6].enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(Sounds s){
		switch(s){
			case Sounds.Bomb:
				if (SEList[0] != null)
				SEList[0].Play();
				break;
			case Sounds.Dead:
				if (SEList[1] != null)
				SEList[1].Play();
				break;
			case Sounds.EnemyDeath:
				if (SEList[2] != null)
				SEList[2].Play();
				break;
			case Sounds.Laser:
				if (SEList[3] != null)
				SEList[3].Play();
				break;
			case Sounds.LaserAlert:
				if (SEList[4] != null)
				SEList[4].Play();
				break;
			case Sounds.PlayerShot:
				if (SEList[5] != null)
				SEList[5].Play();
				break;
			case Sounds.Select:
				if (SEList[6] != null)
				SEList[6].Play();

				break;
			default:
				break;
		}
	}
}
