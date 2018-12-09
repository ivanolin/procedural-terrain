using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public float health;
	int frames;
    private HashSet<Vector2Int> usedTrees;

    // Use this for initialization
    void Start () {
		frames = 0;
		health = 100f;
        usedTrees = new HashSet<Vector2Int>();
		print("Current health: " + health);
	}
	
	// Update is called once per frame
	void Update () {
		frames++;
		health -=0.005f;
		if (frames % 300 == 0){
			print("Health: " + health);
		}
		if(health < 0.0f){
			StartCoroutine(LoadScene("RestartMenu"));
		}


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 3f)) 
            {
                
                Transform parentTransform = hit.transform.parent;
                if (parentTransform != null && parentTransform.name.Contains("Tree"))
                {
                    Vector2Int treeKey = new Vector2Int((int)Mathf.Round(parentTransform.position.x), (int)Mathf.Round(parentTransform.position.z));
                    if (!usedTrees.Contains(treeKey))
                    {
                        print(hit.point);
                        // spawn fruit?
                        if(Random.Range(0.0f, 1.0f) < 0.5f)
                        {
                            print("FRUIT");
                            health = Mathf.Min(100.0f, health + 5.0f);
                            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            go.AddComponent<MeshCollider>();
                            go.transform.position = (hit.point + Camera.main.transform.position) / 2;
                        }
                        usedTrees.Add(treeKey);
                        foreach (Transform tran in parentTransform)
                        {
                            print(tran.name);
                            foreach (Material mat in tran.gameObject.GetComponent<Renderer>().materials)
                            {
                                print(mat.color);
                             
                                mat.color = Color.grey;
                                print("grey");
                                print(mat.color);
                            }
                        }
                    }
                }

            }
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
