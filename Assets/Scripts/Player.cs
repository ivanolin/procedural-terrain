using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public float health;
	int frames;

	// Use this for initialization
	void Start () {
		frames = 0;
		health = 100f;
		print("Current health: " + health);
	}
	
	// Update is called once per frame
	void Update () {
		frames++;
		health -=0.5f;
		if (frames % 300 == 0){
			print("Health: " + health);
		}
		if(health < 0.0f){
			StartCoroutine(LoadScene("RestartMenu"));
		}
	}

	IEnumerator LoadScene(string scene){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
