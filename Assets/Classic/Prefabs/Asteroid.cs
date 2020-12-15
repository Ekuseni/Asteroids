using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Asteroid : PooledObject
{

    [SerializeField]
    private float velocityMultiplier;

    Vector3 velocity;
    Vector3 angularVelocity;
    float radius;
    int layerMask;
    Collider2D[] hits = new Collider2D[2];
    private void Awake()
    {
        //layerMask = LayerMask.GetMask("Asteroids", "Ship");
        //radius = GetComponent<CircleCollider2D>().radius;
    }

    private void OnEnable()
    {
        velocity = new Vector3(GameManager.random.Next(-10, 10) / 10f, GameManager.random.Next(-10, 10) / 10f, 0f) * velocityMultiplier;
        angularVelocity = new Vector3(0f, 0f, GameManager.random.Next(-150, 150) / 10f);
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
        transform.Rotate(angularVelocity * Time.deltaTime);

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

        //CheckCollision();
    }

    private void CheckCollision()
    {
        if (Physics2D.OverlapCircleNonAlloc(transform.position, radius, hits, layerMask) > 1)
        {
            foreach (Collider2D hit in hits)
            {
                hit.gameObject.GetComponent<Asteroid>().ReturnToPool();
                GameManager.SpawnNewAsteroid();
            }
        }
    }
}


