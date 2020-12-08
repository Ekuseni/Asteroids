
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Asteroid asteroidPrefab;
    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    static float spawnDistance = 5f;

    [SerializeField]
    private GameObject shipPrefab;

    [SerializeField]
    private Transform asteroidContainer;
    public static Transform AsteroidContainer { get { return _instance.asteroidContainer; } }
    [SerializeField]
    private Transform bulletContainer;
    public static Transform BulletContainer { get { return _instance.bulletContainer; } }


    [SerializeField]
    UnityEvent restartGameEvents;
    WaitForSeconds second = new WaitForSeconds(1f);

    

    public static Bounds bounds;
    static Bounds cameraBounds;
    static GameManager _instance;

    

    void Start()
    {
        _instance = this;


        Instantiate(shipPrefab, transform);

        bounds = new Bounds(Vector2.zero, new Vector2(gridSize.x * spawnDistance, gridSize.y * spawnDistance));
        cameraBounds = new Bounds(Vector2.zero, new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize) * 2.25f);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Transform temp = asteroidPrefab.GetPooledInstance<Asteroid>().transform;

                temp.SetParent(asteroidContainer);
                temp.position = new Vector3(bounds.min.x + x * spawnDistance + spawnDistance / 2, bounds.min.y + y * spawnDistance + spawnDistance / 2, 0);
            }
        }
    }

    public static void ResetWorld()
    {
        Vector3 offset = new Vector3 (AsteroidContainer.transform.position.x, AsteroidContainer.transform.position.y, 0f);
        AsteroidContainer.transform.position = Vector2.zero;
        foreach (Transform asteroid in AsteroidContainer.transform)
        {
            asteroid.localPosition = asteroid.localPosition + offset;
        }
    }

    System.Random random = new System.Random(42);

    private static Vector2 RandomSpawnPos()
    {
        float x;
        float y;
        Vector2 returnVector;
        
        do
        {
            x = _instance.random.Next(Mathf.FloorToInt(bounds.min.x) * 10, Mathf.CeilToInt(bounds.max.x) * 10) / 10f;
            y = _instance.random.Next(Mathf.FloorToInt(bounds.min.x) * 10, Mathf.CeilToInt(bounds.max.x) * 10) / 10f;

            returnVector = new Vector2(x, y);
        } while (cameraBounds.Contains(returnVector));

        return returnVector;
    }

    public void StartGame()
    {
        Ship.EnableShip();
        ResetWorld();
        Score.ResetScore();
    }

    public static void EndGame()
    {
        foreach (Bullet bullet in BulletContainer.GetComponentsInChildren<Bullet>())
        {
            bullet.ReturnToPool();
        }
        _instance.restartGameEvents?.Invoke();
    }

    public static void SpawnNewAsteroid()
    {
        _instance.StartSpawnCoroutine();
    }

    private void StartSpawnCoroutine()
    {
        StartCoroutine("SpawnAfterWait");
    }

    IEnumerator SpawnAfterWait()
    {
        yield return second;

        Transform temp = _instance.asteroidPrefab.GetPooledInstance<Asteroid>().transform;
        temp.SetParent(asteroidContainer);
        temp.position = RandomSpawnPos();
    }
}



