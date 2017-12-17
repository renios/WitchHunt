using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.parent = FindObjectOfType<Player>().gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
