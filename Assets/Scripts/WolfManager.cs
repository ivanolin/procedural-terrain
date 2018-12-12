using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfManager : MonoBehaviour
{

    public float waveGap;
    public int minSpawn;
    public int maxSpawn;

    public AudioSource howl;
    public GameObject player;
    public GameObject wolfPrefab;

    // Use this for initialization
    void Start()
    {
		StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {

        int toSpawn = minSpawn;
        while (true)
        {
            yield return new WaitForSeconds(waveGap);

			howl.Play();

            Vector3 playerPosition = player.transform.position;
			for(int i = 0; i < toSpawn; i++)
			{
				GameObject newWolf = GameObject.Instantiate(wolfPrefab);
				newWolf.transform.position = new Vector3(
					playerPosition.x + (Random.Range(0,2) == 1 ? 100 : -100), 
					200,
					playerPosition.z + (Random.Range(0,2) == 1 ? 100 : -100));
				yield return new WaitForSeconds(Random.Range(0, 1.5f));
			}

			if(toSpawn < maxSpawn)
			{
				toSpawn += 1;
			}

            //yield return new WaitForSeconds(waveGap);
        }
    }

}
