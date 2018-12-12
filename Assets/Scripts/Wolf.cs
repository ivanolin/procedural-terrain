using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{

    public GameObject player;
    public float acceleration;
    public float maxVelocity;
    public int damage;
	public float lifespan;

    private bool fleeing = false;
    private bool activated = false;

    // Use this for initialization
    void Start()
    {
		player = GameObject.FindWithTag("Player");
        StartCoroutine(Activate());
    }

    // Update is called once per frame
    void Update()
    {

        if (activated)
        {
            Reorient();
            Pursue();
            UpdateBarkVolume();
        }

    }

    private void Reorient()
    {
        // Look at player
        transform.LookAt(player.transform);
        transform.Rotate(-90, 0, 0);
    }

    private void Pursue()
    {
        // Move toward player
        Vector3 pursuitVector = (player.transform.position - transform.position) * acceleration * Time.deltaTime;
        if(fleeing)
            pursuitVector *= -1;

        GetComponent<Rigidbody>().AddForce(pursuitVector * acceleration);
        GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxVelocity);

    }

    private void UpdateBarkVolume()
    {
        // Update bark volume
        float distance = (player.transform.position - transform.position).magnitude;
        distance = Mathf.Clamp(distance, 0, 100);
        float volume = (100 - distance) / 100;

        GetComponent<AudioSource>().volume = volume * 0.5f;
    }

	IEnumerator Activate()
	{
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<AudioSource>().Stop();
		yield return new WaitForSeconds(4);
		activated = true;
		GetComponent<MeshRenderer>().enabled = true;
		GetComponent<AudioSource>().Play();
		GetComponent<AudioSource>().time = Random.Range(0, 3);
        StartCoroutine(Die());
	}

	IEnumerator Die()
    {
        // Slow down over time
        yield return new WaitForSeconds(lifespan);
        fleeing = true;
        yield return new WaitForSeconds(10);
        GameObject.Destroy(gameObject);

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Health>().Damage(damage);
        }
    }
}
