using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioner : MonoBehaviour {

	public GameObject chunkManager;

	// Use this for initialization
	void Start () {
		System.Random rand = new System.Random(System.DateTime.Now.Millisecond);
		transform.position = new Vector3(rand.Next(-10000, 10000), 300, rand.Next(-10000, 10000));
		chunkManager.SetActive(true);
	}
	
}
