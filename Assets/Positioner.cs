using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(Random.Range(10000, -10000), 300, Random.Range(10000, -10000));
	}
	
}
