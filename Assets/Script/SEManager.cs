using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour {

	public enum Sounds {Bomb, Dead, EnemyDeath, Laser, LaserAlert, PlayerShot, Select};
	public List<AudioClip> SEList;
	AudioSource audioSource;

	int cooldown=0;

	// Use this for initialization
	void Awake () {
		audioSource = GetComponent<AudioSource>();
		audioSource.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(Sounds s){
		switch(s){
			case Sounds.Bomb:
				audioSource.PlayOneShot(SEList[0]);
				Debug.Log("Bomb");
				break;
			case Sounds.Dead:
				audioSource.PlayOneShot(SEList[1]);
				break;
			case Sounds.EnemyDeath:
				audioSource.PlayOneShot(SEList[2]);
				break;
			case Sounds.Laser:
				audioSource.PlayOneShot(SEList[3]);
				break;
			case Sounds.LaserAlert:
				audioSource.PlayOneShot(SEList[4]);
				break;
			case Sounds.PlayerShot:
				audioSource.PlayOneShot(SEList[5]);
				break;
			case Sounds.Select:
				audioSource.PlayOneShot(SEList[6]);
				break;
			default:
				break;
		}
	}
}
