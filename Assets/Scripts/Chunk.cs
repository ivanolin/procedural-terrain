using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{

    ///////////
    // Debug //
    ///////////
    public float testSideLength;
    public int testDensity;
	public Color testLowColor;
	public Color testHighColor;

    private Mesh HighDetailMesh;
    private Mesh LowDetailMesh;

    // Use this for testing
    public void Load()
    {
        //GetComponent<MeshFilter>().mesh = GenerateChunkMesh(testSideLength, testDensity);
        HighDetailMesh = GenerateChunkMesh(testSideLength, testDensity);
        LowDetailMesh = GenerateChunkMesh(testSideLength, testDensity/4);

        GetComponent<MeshFilter>().mesh = LowDetailMesh;
        GetComponent<MeshRenderer>().material.mainTexture = GenerateChunkTexture(testSideLength, testDensity);
        gameObject.AddComponent<MeshCollider>();
    }

    public void SetLOD(bool high)
    {
        if(high)
        {
            GetComponent<MeshFilter>().mesh = HighDetailMesh;
        }
        else
        {
            GetComponent<MeshFilter>().mesh = LowDetailMesh;
        }
    }

    /**
     * @param sideLength the actual side length of this chunk mesh in game world distance units
     * @param vertexDensity the number of vertices in a single row (totalVertices = vertexDensity^2)
     * @return a chunk mesh with perlin noise applied to the height of its vertices based on their GLOBAL position
     */
    Mesh GenerateChunkMesh(float sideLength, int vertexDensity)
    {
        //// Precalculate some useful values
        // How to space the vertices to achieve the desired side length
        float vertexSpace = sideLength / (vertexDensity - 1);

        // Calculate the total number of triangles in the mesh
        int numTriangles = (vertexDensity - 1);
        numTriangles *= numTriangles;
        numTriangles *= 6;

        //// Initialize vertex data containers
        Vector3[] vertices = new Vector3[vertexDensity * vertexDensity];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[numTriangles];

        //// Populate vertices array
        // Tracks the x y z positions of the vertices to add
        float yPosition = 0;
        float xPosition = 0;
        float zPosition = 0;
        // Trackes the index into the vertices array
        int verticesIndex = 0;
        // Integers track the rows independently if x/z position (float inacurracies caused mesh thrashing)
        for (int z = 0; z < vertexDensity; z++)
        {
            for (int x = 0; x < vertexDensity; x++)
            {
                //// Add the current point to vertex data
                // Get the height value from perlin based on the GLOBAL position of the vertex to add
				yPosition = PerlinNoise.getHeightTest(transform.position.x + xPosition, transform.position.z + zPosition) * 2;
                vertices[verticesIndex] = new Vector3(xPosition, yPosition, zPosition);
                uvs[verticesIndex] = new Vector2(xPosition / sideLength, zPosition / sideLength);

                xPosition += vertexSpace;
                verticesIndex++;
            }
            // Loop back xPosition (and change zPosition) for new row of vertices
			xPosition = 0;
			zPosition += vertexSpace;
        }

        //// Populate the triangles array
        // Tracks index of the 1D triangle array
        int trianglesIndex = 0;
        // For every row of vertices (excluding the last one)
        for (int i = 0; i < vertices.Length - vertexDensity; i++)
        {
            // Generates a row of "upper" triangles
            if ((i + 1) % vertexDensity != 0)
            {
                triangles[trianglesIndex] = i;                          // This vertex
                triangles[trianglesIndex + 1] = i + vertexDensity;      // The adjacent vertex (in the next row)
                triangles[trianglesIndex + 2] = i + 1;                  // The adjacent vertex (in this row)

                // A triangle takes up 3 indices
                trianglesIndex += 3;
            }
            // Generates a row of "lower" triangles
            if (i % vertexDensity != 0)
            {
                triangles[trianglesIndex] = i;                          // This vertex
                triangles[trianglesIndex + 1] = i + vertexDensity - 1;  // The diagonal vertex
                triangles[trianglesIndex + 2] = i + vertexDensity;      // The adjacent vertex in the next row (also adjacent to the diagonal vertex)

                // A triangle takes up 3 indices
                trianglesIndex += 3;
            }
        }

        // Apply vertex data to new mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Recalculate normals for lighting
        mesh.RecalculateNormals();

        //// Return the constructed chunk mesh
        return mesh;
    }

    /**
     * @param sideLength the actual side length of the chunk mesh this texture will be applied to
     * @param vertexDensity the number of vertices in a single row of the chunk mesh (used to calculate number of corresponding pixels)
     * @return a texture with perlin noise applied to the color of its pixels based on their *predicted* GLOBAL position
     */
    Texture2D GenerateChunkTexture(float sideLength, int vertexDensity)
    {
        // This chunk texture has 1 pixel of color for every square in the chunk mesh (one "upper" and one "lower" triangle)
        Texture2D chunkTexture = new Texture2D(vertexDensity - 1, vertexDensity - 1);
        // Vertex spacing is needed to predict perlin values
		float vertexSpace = sideLength / (vertexDensity - 1);

        //// Populate chunkTexture pixels
		float xPosition = 0;
        float zPosition = 0;
		for (int z = 0; z < vertexDensity - 1; z++)
        {
            for (int x = 0; x < vertexDensity - 1; x++)
            {
                // Get the perlin value of a vertex touching this pixel
				float yValue = PerlinNoise.getHeightTest(transform.position.x + xPosition, transform.position.z + zPosition);
                // Set color based on perlin value
				chunkTexture.SetPixel(x, z, Color.Lerp(testLowColor, testHighColor, yValue));

				xPosition += vertexSpace;
            }
			xPosition = 0;
			zPosition += vertexSpace;
        }

        // Apply all "SetPixel()" calls
		chunkTexture.Apply();

        // Uncomment this line to disable texture blurring
		chunkTexture.filterMode = FilterMode.Point;

        // This line disables wrapping
        chunkTexture.wrapMode = TextureWrapMode.Clamp;

        //// Return the colored chunk texture
        return chunkTexture;
    }

}
