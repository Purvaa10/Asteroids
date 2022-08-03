using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public float maxThrust;
    public float maxTorque;
    public Rigidbody2D rb;

    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;

    public int asteroidSize;
    public int points;

    public GameObject Asteroid1;
    public GameObject Asteroid2;
    public GameObject player;
    public GameObject explosion;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        //adding random amount of torque and force
        Vector2 thrust = new Vector2(Random.Range(-maxThrust, maxThrust), Random.Range(-maxThrust, maxThrust));
        float torque = Random.Range(-maxTorque, maxTorque);

        rb.AddForce(thrust);
        rb.AddTorque(torque);
        player = GameObject.FindWithTag("Player");
        gm = GameObject.FindObjectOfType<GameManager>();

        //asteroidSize = 3;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = transform.position;
        if (transform.position.y > screenTop)
        {
            newPos.y = screenBottom;
        }
        if (transform.position.y < screenBottom)
        {
            newPos.y = screenTop;
        }
        if (transform.position.x > screenRight)
        {
            newPos.x = screenLeft;
        }
        if (transform.position.x < screenLeft)
        {
            newPos.x = screenRight;
        }
        transform.position = newPos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            //spawn the medium asteroids
            if(asteroidSize == 3)
            {
                /*GameObject ast1 = Instantiate(Asteroid4, transform.position, transform.rotation);
                GameObject ast2 = Instantiate(Asteroid4, transform.position, transform.rotation);
                ast1.GetComponent<AsteroidScript>().asteroidSize = 2;
                ast2.GetComponent<AsteroidScript>().asteroidSize = 2;*/
                Instantiate(Asteroid2, transform.position, transform.rotation);
                Instantiate(Asteroid2, transform.position, transform.rotation);

                gm.UpdateNumberOfAsteroids(1);
            }
            //spawn the small asteroids
            if (asteroidSize == 2)
            {
                Instantiate(Asteroid1, transform.position, transform.rotation);
                Instantiate(Asteroid1, transform.position, transform.rotation);

                gm.UpdateNumberOfAsteroids(1);
            }
            if (asteroidSize == 1)
            {
                //remove the asteroid
                gm.UpdateNumberOfAsteroids(-1);
            }

            player.SendMessage("ScorePoints",points);
            //make a explosion
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion,3f);
            //remove the current asteroids
            Destroy(gameObject);
        }
    }
}
