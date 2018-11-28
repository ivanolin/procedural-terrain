using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise {

    // meme octave method
    public static float getHeightTest(float x, float z)
    {
        float height1 = getHeight(x, z);
        float height2 = getHeight(x/80, z/80) * 30;

        return height1 + height2;
    }

    public static float getHeight(float x, float z){

        //x /= 1;
        //z /= 1;

        int lowerX = (int)Mathf.Floor(x);
        int lowerZ = (int)Mathf.Floor(z);
        int upperX = lowerX + 1;
        int upperZ = lowerZ + 1;
        float offsetX = x - lowerX;
        float offsetZ = z - lowerZ;

        float lowerLeftGrad = Vector2.Dot(new Vector2(offsetX, offsetZ), getCornerVector(lowerX, lowerZ));
        float upperLeftGrad = Vector2.Dot(new Vector2(offsetX, offsetZ - 1), getCornerVector(lowerX, lowerZ + 1));
        float upperRightGrad = Vector2.Dot(new Vector2(offsetX - 1, offsetZ - 1), getCornerVector(lowerX + 1, lowerZ + 1));
        float lowerRightGrad = Vector2.Dot(new Vector2(offsetX - 1, offsetZ), getCornerVector(lowerX + 1, lowerZ));

        float lowerLerp = lerp(lowerLeftGrad, lowerRightGrad, fade(offsetX));
        float upperLerp = lerp(upperLeftGrad, upperRightGrad, fade(offsetX));
        float totalLerp = lerp(lowerLerp, upperLerp, fade(offsetZ));
        return totalLerp;
    }

    public static float fade(float t){
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    public static float lerp(float u, float v, float time){
        return u * (1 - time) + v * time;
    }

    public static Vector2 getCornerVector(int x, int y){
        int seed = (x ^ (x >> 16)) << 16 | ((y ^ (y >> 16)) & 0xFFFF);
        Random.seed = seed;
        Vector2 corner = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        corner.Normalize();
        return corner;
    }
    
}