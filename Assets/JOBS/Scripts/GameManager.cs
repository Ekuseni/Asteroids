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

    [SerializeField]
    private static float spawnDistance = 5f;

    Bounds bounds;

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
                asteroids.Add(Instantiate(asteroidPrefab).GetComponent<Asteroid>());
                asteroids[asteroids.Count - 1].transform.position = new Vector3(bounds.min.x + x * spawnDistance + spawnDistance / 2, bounds.min.y + y * spawnDistance + spawnDistance / 2, 0);
            }
        }

        

        Asteroid.displacements = new NativeArray<float3>(asteroids.Count, Allocator.Persistent);
        Asteroid.positions = new NativeArray<float3>(asteroids.Count, Allocator.Persistent);
        Asteroid.angularVelocities = new NativeArray<float>(asteroids.Count, Allocator.Persistent);
        Asteroid.rotations = new NativeArray<float3>(asteroids.Count, Allocator.Persistent);
        Asteroid.raycastCommands = new NativeArray<CapsulecastCommand>(asteroids.Count, Allocator.Persistent);
        Asteroid.raycastHits = new NativeArray<RaycastHit>(asteroids.Count, Allocator.Persistent);
        Asteroid.quaternions = new NativeArray<Quaternion>(asteroids.Count, Allocator.Persistent);

    }

    private void OnDisable()
    {
        Asteroid.displacements.Dispose();
        Asteroid.positions.Dispose();
    }

    private void Update()
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            Asteroid.displacements[i] = asteroids[i].displacement;
            Asteroid.positions[i] = asteroids[i].transform.position;
            Asteroid.rotations[i] = asteroids[i].transform.rotation.eulerAngles;
            Asteroid.angularVelocities[i] = asteroids[i].angularVelocity;
            Asteroid.quaternions[i] = asteroids[i].transform.rotation;
        }

        MoveJob moveJob = new MoveJob
        {
            deltaTime = Time.deltaTime,
            displacement = Asteroid.displacements,
            position = Asteroid.positions,
            angularVelocity = Asteroid.angularVelocities,
            rotation = Asteroid.rotations,
            boundsMax = bounds.max,
            boundsMin = bounds.min,
            boundsSize = bounds.size,
            raycasts = Asteroid.raycastCommands,
            radius = asteroids[0].GetComponent<CapsuleCollider>().radius
        };

        JobHandle moveJobHande = moveJob.Schedule(asteroids.Count, 32);
        JobHandle raycastJobHandle = CapsulecastCommand.ScheduleBatch(Asteroid.raycastCommands, Asteroid.raycastHits, 32, moveJobHande);
        raycastJobHandle.Complete();

        for (int i = 0; i < asteroids.Count; i++)
        {
            asteroids[i].transform.position = Asteroid.positions[i];
            asteroids[i].transform.rotation = Quaternion.Euler(Asteroid.rotations[i].x, Asteroid.rotations[i].y, Asteroid.rotations[i].z);
            if(Asteroid.raycastHits[i].normal != Vector3.zero)
            {
                asteroids[i].GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
}



