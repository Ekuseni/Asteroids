using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.U2D.Entities.Physics;


public class GameManager : MonoBehaviour
{

    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private GameObject asteroidPrefab;


    private Entity asteroidEntity;
    private EntityManager entityManager;

    void Start()
    {
       entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        BlobAssetStore blobAssetStore = new BlobAssetStore();

        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        asteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab, settings);

        //blobAssetStore.Dispose();


        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {

                Entity spawnedEntity = entityManager.Instantiate(asteroidEntity);

                entityManager.SetComponentData(spawnedEntity, new Translation { Value = new float3(x * 5, y * 5, 0) });
                entityManager.SetComponentData(spawnedEntity, new PhysicsVelocity{ Linear = new float2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) });
            }
        }
    }
}
