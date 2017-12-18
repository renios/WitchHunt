using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed;
	public int damage;

	public Vector3 direction;

	AudioSource BulletSound;

	// Use this for initialization
	void Start () {
		if (this.name == "PlayerBasicBullet")
		{
			BulletSound = gameObject.GetComponent<AudioSource>();
			if (Random.Range(0.0f, 1.0f) < 0.3f)
				BulletSound.Play();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction * Time.deltaTime * speed;

		if (Mathf.Abs(transform.position.x) > 10*2 || Mathf.Abs(transform.position.y) > 6*2) {
			DestroyBullet();
		}
		else if (GetComponent<MushroomBullet>() != null) {
			if (Mathf.Abs(transform.position.x) > 9.5f || Mathf.Abs(transform.position.y) > 5.5f) {
				GetComponent<MushroomBullet>().DestroyMushroomBullet();
			}
			else if(GetComponent<MushroomBullet>().selfDestroy == true)
			{
				if(transform.position.x > GetComponent<MushroomBullet>().selfDestroyDelay - 6){
					GetComponent<MushroomBullet>().DestroyMushroomBullet();
				}
			}
			
		}
	}

	public void DestroyBullet() {
		if (GetComponent<TrapBullet>() != null) {
			GetComponent<TrapBullet>().InactiveNow();
		}
		else {
			Destroy(gameObject);
		}
	}
}
