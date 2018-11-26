using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perlin : MonoBehaviour {

	// Use this for initialization
	void Start () {

		GameObject obj = GameObject.Find("Terrain");
 
        if (obj.GetComponent<Terrain>())
        {
            Terrain terrain = obj.GetComponent<Terrain>();
			float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

			float tileSize = 10.0f;
			print(terrain.terrainData.heightmapWidth);
			print(terrain.terrainData.heightmapHeight);
			for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
			{
				for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
				{
					float x = ((float)i / (float)terrain.terrainData.heightmapWidth) * tileSize;
					float y = ((float)k / (float)terrain.terrainData.heightmapHeight) * tileSize;
					// print(x);
					// print(y);
					float h = (PerlinNoise.getHeight(x, y) + 1) / 10.0f;

					heights[i, k] = h;
				}
			}
 
        	terrain.terrainData.SetHeights(0, 0, heights);
        }

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


