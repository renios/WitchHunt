using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public static MusicManager instance = null; 

	GameObject gameMusic;
	GameObject gameMusicOnPlay;

	public bool changeMusic = false;
  
	public static MusicManager Instance {
		get {return instance;}
	}
// Use this for initialization
    void Awake () {
		gameMusic = GameObject.Find("GameMusic");
		if ((instance != null && instance != this) && gameMusic.GetComponent<AudioSource>().clip.name != this.GetComponent<AudioSource>().clip.name){
			gameMusic.GetComponent<AudioSource>().clip = this.GetComponent<AudioSource>().clip;
			gameMusic.GetComponent<MusicManager>().changeMusic = true;
			Destroy(this.gameObject);
			return;
		}
		else if ((instance != null && instance != this) && gameMusic.GetComponent<AudioSource>().clip.name == this.GetComponent<AudioSource>().clip.name){
			Destroy(this.gameObject);
			return;
		}
		else{
			instance = this;
		}
		
		
		if(gameMusicOnPlay == null)
		{
			gameMusicOnPlay = gameMusic;
			changeMusic = true;
		}
		if (gameMusic == null){
			Debug.Log("gameMusic null\n");
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
