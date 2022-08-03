using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed;
    public int points;
    public float timeBeforeSpawning;

    public int currentLevel = 0;
   
    public float shootingDelay;     //Time between shots in seconds
    public float lastTimeShot = 0f;
    public float bulletSpeed;

    public bool disabled;//true when currently disabled

    public GameObject explosion;
    public SpriteRenderer spriteRender;
    public Collider2D collider1;
    public Transform player;
    public GameObject bullet;
    public Transform startPosition;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        /*timeBeforeSpawning = Random.Range(5f, 20f);
        Invoke(nameof(Enable), timeBeforeSpawning);*/
        NewLevel();
   
    }

    void Update()
    {
        if(disabled)
        { 
            return;
        } 
        if(Time.time > lastTimeShot + shootingDelay)
        {
            //shoot
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            //make a bullet
            GameObject newbullet = Instantiate(bullet, transform.position, q);

            newbullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, bulletSpeed));
            lastTimeShot = Time.time;
        }
    }
    private void FixedUpdate()
    {
        if (disabled)
        {
            return;
        }
        //figer out which way to move to approach the player

        direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
 
    }
    public void NewLevel()
    {
        Disable();
        currentLevel++;
        
        timeBeforeSpawning = Random.Range(5f, 20f);
        Invoke(nameof(Enable), timeBeforeSpawning);
        
        speed = currentLevel;
        bulletSpeed = 250 * currentLevel;
        points = 100 * currentLevel;
    }
    void Enable()
    {
        //move to start position
        transform.position = startPosition.position;
        //turn on collider and sprite 
        collider1.enabled = true;
        spriteRender.enabled = true; 
        disabled = false;
    }
    public void Disable()
    {

        //turn of colliders and sprite
        collider1.enabled = false;
        spriteRender.enabled = false;
        disabled = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bullet"))
        {
            //tell the player to score some points
            player.SendMessage("ScorePoints", points);

            //explosion
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            Disable();
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            Disable();
        }
    }
}
