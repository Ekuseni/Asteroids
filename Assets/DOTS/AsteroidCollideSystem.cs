using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.U2D.Entities.Physics;
using Unity.Transforms;

public class AsteroidCollideSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var physicsWorldSystem = World.GetExistingSystem<PhysicsWorldSystem>();
        var physicsWorld = physicsWorldSystem.PhysicsWorld;

        var didExplode = false;

        Entities
            .ForEach((
                Entity missileEntity,
                ref PhysicsColliderBlob collider,
                ref Translation tr,
                ref Rotation rot) =>
            {
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

                    PostUpdateCommands.DestroyEntity(asteroidEntity);
                    PostUpdateCommands.DestroyEntity(missileEntity);

                    didExplode = true;
                }
            });

        if (didExplode)
        {
           
        }
    }
}

    
