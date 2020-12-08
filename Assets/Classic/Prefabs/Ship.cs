using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ship : MonoBehaviour
{


    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private Bullet bulletPrefab;

    private static Ship _instance;

    Vector3 velocity;

    WaitForSeconds halfSecond = new WaitForSeconds(0.5f);
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        gameObject.SetActive(false);
    }



    public static void EnableShip()
    {
        _instance.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        StartCoroutine("ShootCoroutine");
        velocity = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            float x = Mathf.Clamp(velocity.x + Mathf.Cos((transform.localRotation.eulerAngles.z + 90) * Mathf.Deg2Rad) * Time.deltaTime, -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);

            float y = Mathf.Clamp(velocity.y + Mathf.Sin((transform.localRotation.eulerAngles.z + 90) * Mathf.Deg2Rad) * Time.deltaTime, -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);

            velocity = new Vector3(x,y,0);

           
        }
       

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Rotate(-turnSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Rotate(turnSpeed * Time.deltaTime);
        }

        Move();
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.EndGame();  
        gameObject.SetActive(false);
    }

    IEnumerator ShootCoroutine()
    {
        while(true)
        {
            yield return halfSecond;

            Bullet temp = bulletPrefab.GetPooledInstance<Bullet>();
            temp.transform.SetParent(GameManager.BulletContainer);
            temp.transform.rotation = transform.localRotation;

            Vector2 direction = new Vector2(Mathf.Cos((transform.localRotation.eulerAngles.z + 90) * Mathf.Deg2Rad), Mathf.Sin((transform.localRotation.eulerAngles.z + 90) * Mathf.Deg2Rad));

            temp.AddVelocity(maxSpeed * direction);
        }
    }

    void Rotate(float angle)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, (transform.localRotation.eulerAngles.z + angle) % 360f);
    }

    private void Move()
    {
        GameManager.AsteroidContainer.transform.position -= velocity;
    }
}
