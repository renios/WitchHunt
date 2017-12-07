using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

	public float scrollSpeed = 0.5f; 
	float offset;

    void Start () {
    
	}

    void Update () {
		offset += Time.deltaTime * scrollSpeed;
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset, 0);		
    }
}
