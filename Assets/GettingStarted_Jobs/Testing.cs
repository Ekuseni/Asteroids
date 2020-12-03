/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class Testing : MonoBehaviour {

    //[SerializeField] private bool useJobs;
    [SerializeField] private Transform pfZombie;
    private List<Zombie> zombieList;

    public class Zombie {
        public Transform transform;
        public float moveY;
    }

    private void Start() {
        zombieList = new List<Zombie>();
        for (int i = 0; i < 160 * 160; i++) {
            Transform zombieTransform = Instantiate(pfZombie, new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-5f, 5f)), Quaternion.identity);
            zombieList.Add(new Zombie {
                transform = zombieTransform,
                moveY = UnityEngine.Random.Range(1f, 2f)
            });
        }
    }

    private void Update() {
        float startTime = Time.realtimeSinceStartup;
        
            NativeArray<float> moveYArray = new NativeArray<float>(zombieList.Count, Allocator.TempJob);
            TransformAccessArray transformAccessArray = new TransformAccessArray(zombieList.Count);

            for (int i = 0; i < zombieList.Count; i++)
            {
                
                moveYArray[i] = zombieList[i].moveY;
                transformAccessArray.Add(zombieList[i].transform);
            }

            ReallyToughParallelJobTransforms reallyToughParallelJobTransforms = new ReallyToughParallelJobTransforms {
                deltaTime = Time.deltaTime,
                moveYArray = moveYArray,
            };

            JobHandle jobHandle = reallyToughParallelJobTransforms.Schedule(transformAccessArray);
            jobHandle.Complete();

            for (int i = 0; i < zombieList.Count; i++) {
                
                zombieList[i].moveY = moveYArray[i];
            }

            
            moveYArray.Dispose();
            transformAccessArray.Dispose();
        
    }

}

[BurstCompile]
public struct ReallyToughParallelJobTransforms : IJobParallelForTransform {
    
    public NativeArray<float> moveYArray;
    [ReadOnly] public float deltaTime;

    public void Execute(int index, TransformAccess transform) {
        transform.position += new Vector3(0, moveYArray[index] * deltaTime, 0f);
        if (transform.position.y > 5f) {
            moveYArray[index] = -math.abs(moveYArray[index]);
        }
        if (transform.position.y < -5f) {
            moveYArray[index] = +math.abs(moveYArray[index]);
        }

        float value = 0f;
        for (int i = 0; i < 1000; i++) {
            value = math.exp10(math.sqrt(value));
        }
    }

}
