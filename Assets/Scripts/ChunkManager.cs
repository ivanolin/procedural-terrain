
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {

	public GameObject chunkPrefab;
	public int loadDistance;
	public int lodCutoff;
	public int chunkSideLength;

	public Vector2Int currentPosition;
	public Vector2Int loadDirection;

	private Dictionary<Vector2Int, GameObject> loadedChunks; 
	

	// Use this for initialization
	void Start () {
		loadedChunks = new Dictionary<Vector2Int, GameObject>();

		UpdateCurrentPosition();
		StopAllCoroutines();
		StartCoroutine(LoadChunks());
	}
	
	// Update is called once per frame
	void Update () {
		UpdateCurrentPosition();
	}

	void UpdateCurrentPosition()
	{
		int x = (int)(Mathf.Floor(transform.position.x/chunkSideLength));
		int z = (int)(Mathf.Floor(transform.position.z/chunkSideLength));

		Vector2Int updatedPosition = new Vector2Int(x,z);
		if(updatedPosition != currentPosition)
		{
			loadDirection = currentPosition - updatedPosition;
			currentPosition = updatedPosition;
			StartCoroutine(LoadChunks());
			UnloadChunks();
		}
		currentPosition = updatedPosition;
	}

	IEnumerator LoadChunks()
	{
		for(int z = currentPosition.y - loadDistance; z <= currentPosition.y + loadDistance; z++)
		{
			for(int x = currentPosition.x - loadDistance; x <= currentPosition.x + loadDistance; x++)
			{
				LoadChunk(x,z);
				yield return new WaitForEndOfFrame();
			}
		}
	}

	void LoadChunk(int x, int z)
	{

		GameObject chunkToLoad;
		Vector2Int chunkKey = new Vector2Int(x, z);

		if(loadedChunks.ContainsKey(chunkKey))
		{
			chunkToLoad = loadedChunks[chunkKey];
		}
		else
		{
			chunkToLoad = GameObject.Instantiate(chunkPrefab);
			Vector3 position = new Vector3(x*chunkSideLength, 0, z*chunkSideLength);
			chunkToLoad.transform.position = position;
			chunkToLoad.GetComponent<Chunk>().LoadTerrain();
			chunkToLoad.name = "CHUNK [" + x + ", " + z + "]";

			loadedChunks.Add(chunkKey, chunkToLoad);
		}

		int localX = Mathf.Abs(x - currentPosition.x);
		int localZ = Mathf.Abs(z - currentPosition.y);

		if(localX >= lodCutoff || localZ >= lodCutoff)
		{
			// Debuge
			chunkToLoad.name = "CHUNK [" + x + ", " + z + "] (LOW LOD)";
			chunkToLoad.GetComponent<Chunk>().SetLOD(false);
		}
		else
		{
			// Debug
			chunkToLoad.name = "CHUNK [" + x + ", " + z + "]";
			chunkToLoad.GetComponent<Chunk>().SetLOD(true);
		}

	}

	void UnloadChunks()
	{
		int rowToUnload = (currentPosition + loadDirection * (loadDistance + 1)).y;
		int colToUnload = (currentPosition + loadDirection * (loadDistance + 1)).x;

		List<Vector2Int> keysToRemove = new List<Vector2Int>();

		foreach(Vector2Int chunkKey in loadedChunks.Keys)
		{
			if(loadDirection.y != 0 && chunkKey.y == rowToUnload)
			{
				keysToRemove.Add(chunkKey);
			}
			else if(loadDirection.x != 0 && chunkKey.x == colToUnload)
			{
				keysToRemove.Add(chunkKey);
			}
		}

		foreach(Vector2Int chunkKey in keysToRemove)
		{
			GameObject toUnload = loadedChunks[chunkKey];
			loadedChunks.Remove(chunkKey);

			GameObject.Destroy(toUnload);
		}
	}
}
