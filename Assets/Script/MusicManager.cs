using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public static MusicManager instance = null; 

	GameObject gameMusic;
	GameObject gameMusicOnPlay;

	bool changeMusic = false;
  
	public static MusicManager Instance {
		get {return instance;}
	}
// Use this for initialization
    void Awake () {
		if (instance != null && instance != this){
			Destroy(this.gameObject);
			return;
		}
		else{
			instance = this;
		}
		gameMusic = GameObject.Find("GameMusic");
		if(gameMusicOnPlay == null || gameMusic.GetComponent<AudioSource>().clip.name != gameMusicOnPlay.GetComponent<AudioSource>().clip.name)
		{
			gameMusicOnPlay = gameMusic;
			changeMusic = true;
		}
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (changeMusic){
			Debug.Log("Music Change\n");
			gameMusicOnPlay.GetComponent<AudioSource>().Play();
			changeMusic = false;
		}
	}
}
