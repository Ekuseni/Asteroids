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
    private GameObject asteroidPrefab;
    [SerializeField]
    private Vector2Int gridSize;
    // Start is called before the first frame update

    private static List<Asteroid> asteroids = new List<Asteroid>();

    NativeArray<float3> displacementArray;
    TransformAccessArray transformAccessArray;

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

        //ObjectPool.Preload(asteroidPrefab, Mathf.CeilToInt(gridSize.x * gridSize.y * 1.1f));

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                asteroids.Add(Instantiate(asteroidPrefab).GetComponent<Asteroid>());
                asteroids[asteroids.Count - 1].transform.position = new Vector3(x * 5, y * 5, 0);
            }
        }

        displacementArray = new NativeArray<float3>(asteroids.Count, Allocator.Persistent);
        transformAccessArray = new TransformAccessArray(asteroids.Count);

        for (int i = 0; i < asteroids.Count; i++)
        {
            displacementArray[i] = asteroids[i].displacement;
            transformAccessArray.Add(asteroids[i].transform);
        }

    }

    private void Update()
    {
        MoveJob moveJob = new MoveJob
        {
            displacement = displacementArray,
            deltaTime = Time.deltaTime
        };

        JobHandle jobHandle = moveJob.Schedule(transformAccessArray);
        jobHandle.Complete();
    }
}



