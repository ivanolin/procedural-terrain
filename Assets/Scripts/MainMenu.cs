using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    void Start(){
        //Unlock cursor
        if ( Cursor.lockState == CursorLockMode.Locked){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

	public void PlayGame(){
		StartCoroutine(LoadScene("MainGame"));
	}

	public void QuitGame(){
		Application.Quit();
	}

    public void RestartGame(){
		StartCoroutine(LoadScene("MainMenu"));
    }

    public void GameOver(){
    	StartCoroutine(LoadScene("RestartMenu"));
    }

    IEnumerator LoadScene(string scene){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
