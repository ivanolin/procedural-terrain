using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise {

    public const int PALM_TREE = 1;
    public const int SNOW_TREE = 2;
    public const int NORMAL_TREE = 3;

    public static int getTreeType(float x, float z) {
        float height = getHeightTest(x, z);
        float moisture = getMoisture(x, z);
        int xr = (int)Mathf.Round(x);
        int zr = (int)Mathf.Round(z);
        int positionSeed = (xr ^ (xr >> 16)) << 16 | ((zr ^ (zr >> 16)) & 0xFFFF);
        Random.seed = positionSeed ^ 41867;
        float rand = Random.Range(0.0f, 1.0f);

        float threshold = (
            0.25f * // base threshold, 1/4
            Mathf.SmoothStep(0.0f, 1.0f, moisture) * // Make moister climates have more vegetation
            Mathf.SmoothStep(0.0f, 1.0f, getHeight(x/64.0f, z/64.0f) / 2.0f + 0.5f)); // tend to put trees at local maximum
        
        if(rand >  threshold) {
            return 0;
        }

        if (moisture < 0.5f){
            if(height < 12.5f){
                return PALM_TREE;
            } else {
                return 0;
            }
        } else {
            if(height < 12.5f){
                return NORMAL_TREE;
            } else {
                return SNOW_TREE;
            }
        }
    }

    // meme octave method
    public static float getHeightTest(float x, float z)
    {
        float height1 = getHeight(x, z) * 0.5f;
        float height2 = getHeight(x/64.0f, z/64.0f) * 20;
        // largest octave, gradual changes
        float height3 = getHeight(x/512.0f, z/512.0f) * 50;

        return height1 + height2 + height3;
    }

    public static float getMoisture(float x, float z){
        // Seed is just a constant random number
        // Scale values to [0.0f, 1.0f]
        return (getPerlin(x / 256.0f, z / 256.0f, 38171) + 1) / 2.0f;
    }

    public static float getHeight(float x, float z){
        return getPerlin(x, z, 0);
    }

    public static float getPerlin(float x, float z, int seed){

        //x /= 1;
        //z /= 1;

        int lowerX = (int)Mathf.Floor(x);
        int lowerZ = (int)Mathf.Floor(z);
        int upperX = lowerX + 1;
        int upperZ = lowerZ + 1;
        float offsetX = x - lowerX;
        float offsetZ = z - lowerZ;

        float lowerLeftGrad = Vector2.Dot(new Vector2(offsetX, offsetZ), getCornerVector(lowerX, lowerZ, seed));
        float upperLeftGrad = Vector2.Dot(new Vector2(offsetX, offsetZ - 1), getCornerVector(lowerX, lowerZ + 1, seed));
        float upperRightGrad = Vector2.Dot(new Vector2(offsetX - 1, offsetZ - 1), getCornerVector(lowerX + 1, lowerZ + 1, seed));
        float lowerRightGrad = Vector2.Dot(new Vector2(offsetX - 1, offsetZ), getCornerVector(lowerX + 1, lowerZ, seed));

        float lowerLerp = lerp(lowerLeftGrad, lowerRightGrad, fade(offsetX));
        float upperLerp = lerp(upperLeftGrad, upperRightGrad, fade(offsetX));
        float totalLerp = lerp(lowerLerp, upperLerp, fade(offsetZ));
        return totalLerp / 0.70710678118f;
    }

    public static float fade(float t){
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    public static float lerp(float u, float v, float time){
        return u * (1 - time) + v * time;
    }

    public static Vector2 getCornerVector(int x, int y, int otherSeed){
        int positionSeed = (x ^ (x >> 16)) << 16 | ((y ^ (y >> 16)) & 0xFFFF);
        Random.seed = positionSeed ^ otherSeed;
        float angle = Random.Range(0.0f, 2 * Mathf.PI);
        Vector2 corner = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        corner.Normalize();
        return corner;
    }
    
}