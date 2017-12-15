using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerCollider : MonoBehaviour {

	public GameObject hitParticle;
	Player player;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "EnemyBullet") {
			player.Damaged();

			Instantiate(hitParticle, transform.position, Quaternion.identity);
		}
	}

	//public static Collider2D[] OverlapCircleAll(Vector2 point, 
	//	float radius, int layerMask = DefaultRaycastLayers, 
	//	float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
}
