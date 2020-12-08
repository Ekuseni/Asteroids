using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PooledObject
{
    WaitForSeconds seconds = new WaitForSeconds(3f);

    public void AddVelocity(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    private void OnEnable()
    {
        StartCoroutine("ReturnToPoolCoroutine");
    }

    IEnumerator ReturnToPoolCoroutine()
    {
        yield return seconds;
        this.ReturnToPool();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Score.IncreaseScore(100);
        StopAllCoroutines();
        ReturnToPool();
    }
}
