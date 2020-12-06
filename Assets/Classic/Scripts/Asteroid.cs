using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : PooledObject
{

    [SerializeField]
    private float velocityMultiplier;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * velocityMultiplier;
        GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-15f, 15f);
    }
}


