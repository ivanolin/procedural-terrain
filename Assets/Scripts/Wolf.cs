using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour {

	public GameObject player;
	public float acceleration;
	public float accelerationDecayPerSecond;
	public float maxVelocity;
	public int damage;

	// Use this for initialization
	void Start () {
		StartCoroutine(decayAcceleration());
	}
	
	// Update is called once per frame
	void Update () {

		// Look at player
		transform.LookAt(player.transform);
		transform.Rotate(-90, 0, 0);

		// Move toward player
		Vector3 pursuitVector = (player.transform.position - transform.position) * acceleration * Time.deltaTime;
		GetComponent<Rigidbody>().AddForce(pursuitVector * acceleration);
		GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxVelocity);

	}

	IEnumerator decayAcceleration()
	{
		while(true){
		// Slow down over time
		yield return new WaitForSeconds(1);
		acceleration -= accelerationDecayPerSecond;
		if(acceleration < 0)
			acceleration = 0;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<Health>().Damage(damage);
		}
	}
}
