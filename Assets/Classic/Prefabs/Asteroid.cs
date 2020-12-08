using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Unity.Burst.BurstCompile]
public class Asteroid : PooledObject
{

    [SerializeField]
    private float velocityMultiplier;

    

    private void OnEnable()
    {
        System.Random random = new System.Random(GameManager.AsteroidContainer.childCount);
        GetComponent<Rigidbody2D>().velocity = new Vector2(random.Next(-10, 10) / 10f, random.Next(-10, 10) / 10f) * velocityMultiplier;
        GetComponent<Rigidbody2D>().angularVelocity = random.Next(-150, 150) / 10f;
    }

    private void Update()
    {
        if (transform.position.x > GameManager.bounds.max.x)
        {
            transform.position = new Vector2(transform.position.x - GameManager.bounds.size.x, transform.position.y);
        }

        if (transform.position.y > GameManager.bounds.max.y)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - GameManager.bounds.size.y);
        }

        if (transform.position.x < GameManager.bounds.min.x)
        {
            transform.position = new Vector2(transform.position.x + GameManager.bounds.size.x, transform.position.y);
        }

        if (transform.position.y < GameManager.bounds.min.y)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + GameManager.bounds.size.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        ReturnToPool();
        GameManager.SpawnNewAsteroid();
    }
}


