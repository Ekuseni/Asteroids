using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class Asteroid : MonoBehaviour
{


    public static NativeArray<float3> positions;
    public static NativeArray<float3> displacements;
    public static NativeArray<float3> rotations;
    public static NativeArray<float> angularVelocities;
    public static NativeArray<CapsulecastCommand> raycastCommands;
    public static NativeArray<RaycastHit> raycastHits;
    public static NativeArray<Quaternion> quaternions;

    public Vector3 displacement;
    public float angularVelocity;
    private void Awake()
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);

        displacement = new Vector3(x, y, 0);
        angularVelocity = UnityEngine.Random.Range(-15f, 15f);
    }
}

[BurstCompile]
public struct MoveJob : IJobParallelFor
{
    public NativeArray<float3> position;
    public NativeArray<float3> rotation;
    public NativeArray<CapsulecastCommand> raycasts;
    [ReadOnly] public float3 halfExtent;
    [ReadOnly] public float radius;
    [ReadOnly] public NativeArray<float3> displacement;
    [ReadOnly] public NativeArray<float> angularVelocity;
    [ReadOnly] public float deltaTime;
    [ReadOnly] public float3 boundsMin;
    [ReadOnly] public float3 boundsMax;
    [ReadOnly] public float3 boundsSize;

    public void Execute(int index)
    {
        float distance = ((Vector3)(displacement[index] * deltaTime)).magnitude;
        raycasts[index] = new CapsulecastCommand(position[index] + new float3(0,0,5), position[index] - new float3(0, 0, 5), radius, displacement[index], distance);

        Debug.DrawRay(position[index], displacement[index]);

        position[index] += displacement[index] * deltaTime;

        if (position[index].x > boundsMax.x)
        {
            position[index] = new float3(position[index].x - boundsSize.x, position[index].y, position[index].z);
        }

        if (position[index].y > boundsMax.y)
        {
            position[index] = new float3(position[index].x, position[index].y - boundsSize.y, position[index].z);
        }

        if (position[index].x < boundsMin.x)
        {
            position[index] = new float3(position[index].x + boundsSize.x, position[index].y, position[index].z);
        }

        if (position[index].y < boundsMin.y)
        {
            position[index] = new float3(position[index].x, position[index].y + boundsSize.y, position[index].z);
        }

        rotation[index] = new float3(0f, 0f, rotation[index].z + (angularVelocity[index] * deltaTime));
    }
}

//[BurstCompile]
//public struct PrepareRaycastCommands : IJobParallelFor
//{
//    public float deltaTime;

//    public NativeArray<RaycastCommand> raycasts;
//    [ReadOnly]
//    public NativeArray<float3> velocities;
//    [ReadOnly]
//    public NativeArray<float3> positions;


//    public void Execute(int i)
//    {
//        float distance = ((Vector3)(velocities[i] * deltaTime)).magnitude;
//        raycasts[i] = new RaycastCommand(positions[i], velocities[i], distance);
//    }
//}
