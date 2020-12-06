using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.U2D.Entities.Physics;
using UnityEngine;

public class AsteroidCollision : SystemBase
{
    protected override void OnUpdate()
    {
        var physicsWorldSystem = World.GetExistingSystem<PhysicsWorldSystem>();
        var physicsWorld = physicsWorldSystem.PhysicsWorld;



        Entities.WithAll<Asteroid>().ForEach((
                Entity missileEntity,
                ref PhysicsColliderBlob collider,
                ref Translation tr,
                ref Rotation rot) =>
            {

                Debug.Log("Check: " + missileEntity.ToString());

                if (physicsWorld.OverlapCollider(
                    new OverlapColliderInput
                    {
                        Collider = collider.Collider,
                        Transform = new PhysicsTransform(tr.Value, rot.Value),
                        Filter = collider.Collider.Value.Filter
                    },
                    out OverlapColliderHit hit))
                {
                    var asteroidEntity = physicsWorld.AllBodies[hit.PhysicsBodyIndex].Entity;

                    Debug.Log("Hit:" + asteroidEntity.ToString());
                }
            }).Run();

        
    }
    
}

