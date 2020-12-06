using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Asteroid asteroidPrefab;
    [SerializeField]
    private Vector2Int gridSize;
    // Start is called before the first frame update

    [SerializeField]
    private static float spawnDistance = 5f;

    private static Bounds bounds;

    private static List<Asteroid> asteroids = new List<Asteroid>();

    //NativeArray<float3> displacementArray;
    //NativeArray<float3> positions;
    //TransformAccessArray transformAccessArray;

    //public static void AddAsteroidToList(Asteroid asteroid)
    //{
    //    asteroids.Add(asteroid);
    //}

    //public static void RemoveAsteroidFromList(Asteroid asteroid)
    //{
    //    asteroids.Remove(asteroid);
    //}

    void Start()
    {
        bounds = new Bounds(Vector2.zero, new Vector2(gridSize.x * spawnDistance, gridSize.y * spawnDistance));
        //ObjectPool.Preload(asteroidPrefab, Mathf.CeilToInt(gridSize.x * gridSize.y * 1.1f));

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Asteroid newAsteroid = asteroidPrefab.GetPooledInstance<Asteroid>();

                asteroids.Add(newAsteroid);
                asteroids[asteroids.Count - 1].transform.position = new Vector3(bounds.min.x + x * spawnDistance + spawnDistance / 2, bounds.min.y + y * spawnDistance + spawnDistance / 2, 0);
            }
        }


    }

    

    private void Update()
    {
        
    }
}



