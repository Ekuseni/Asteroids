using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine.Jobs;

public class Asteroid : MonoBehaviour
{
    //private SpriteRenderer spriteRenderer;
    //private PolygonCollider2D polygonCollider;
    //private Rigidbody2D rb2D;


    //public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }
    //public PolygonCollider2D PolygonCollider2D { get { return polygonCollider; } }
    //public Rigidbody2D Rigidbody2D { get { return rb2D; } }

    public static NativeArray<float3> positions = new NativeArray<float3>();
    public static NativeArray<float3> displacements = new NativeArray<float3>();

    public Vector3 displacement;

    private void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //polygonCollider = GetComponent<PolygonCollider2D>();
        //rb2D = GetComponent<Rigidbody2D>();

        

        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);

        displacement = new Vector3(x, y, 0);
        

        //float force = Random.Range(10f, 20f);

        //rb2D.velocity = /*(*/new Vector2(x,y)/* * force)*/;
    }


    //private void Update()
    //{
    //    transform.position += displacement * Time.deltaTime;
    //}


}

[BurstCompile]
public struct MoveJob : IJobParallelForTransform
{   
    public NativeArray<float3> displacement;
    [ReadOnly] public float deltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        transform.position += new Vector3(displacement[index].x, displacement[index].y, displacement[index].z)  * deltaTime;
    }
}
