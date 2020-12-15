using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PooledObject
{
    WaitForSeconds seconds = new WaitForSeconds(3f);
    Collider2D[] hits = new Collider2D[1];
    Vector3 velocity;
    int layerMask;
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Asteroids");
    }

    public void AddVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
        CheckCollision();
    }

    private void OnEnable()
    {
        StartCoroutine("ReturnToPoolCoroutine");
    }

    IEnumerator ReturnToPoolCoroutine()
    {
        yield return seconds;
        ReturnToPool();
    }

    private void CheckCollision()
    {
        if (Physics2D.OverlapBoxNonAlloc(transform.position, transform.localScale, transform.localRotation.z, hits, layerMask) > 0)
        {
            foreach (Collider2D hit in hits)
            {
                hit.gameObject.GetComponent<Asteroid>().ReturnToPool();
                GameManager.SpawnNewAsteroid();
            }

            Score.IncreaseScore(100);
            StopAllCoroutines();
            ReturnToPool();
        }
    }
}
