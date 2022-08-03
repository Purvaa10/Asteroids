using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SpaceShipController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
    public Button button;

    public float Thrust;
    public float turnThrust;
    public float thrustInput;
    private float turnInput;

    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;

    private float deathForce;
    public float bulletForce;

    public int score;
    public int lives;

    public TextMeshProUGUI highScoreListText;

    private bool hyperspace;    //true = currently hyperspacing
    private bool gameOver;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    public TMP_InputField highScoreInput;
    
    public GameObject bullet;
    public GameObject explosion;
    public GameObject gameOverPanel;
    public GameObject newHighScorePanel;

    public GameManager gm;

    public Color inColor;
    public Color normalColor;

    public AlienScript alien;
    
    // Speed
    public float _velocity = 1;
    public float _rotationSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        score = 0;
        hyperspace = false;
        scoreText.text = "Score " + score;
        livesText.text = "Lives " + lives;
        PlayerPrefs.SetInt("highscore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //gameOver = false;
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

    
        //bullet input
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        //if (Input.GetButtonDown("Fire1"))
        {
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletForce);
            Destroy(newBullet, 2.0f);
        }


        //check for hyperspace

        if(Input.GetButtonDown("HyperSpace") && !hyperspace)
        {
            hyperspace = true;
            //turn off Spriterenderer and colliders
            spriteRenderer.enabled = false;
            collider.enabled = false;
            Invoke(nameof(HyperSpace),1f);
           // Invoke("HyperSpace", 1f);
        }

        //Rotating the sheep
        transform.Rotate(Vector3.forward * turnInput * Time.deltaTime * -turnThrust);
        //screen wraping
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

    void FixedUpdate()
    {
        // Vector3 velocity = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //gameObject. GetComponent<Rigidbody2D>().velocity = velocity;
        //rb.velocity = Vector2.right * turnInput;
        //rb.AddRelativeForce(Vector2.up * thrustInput);

        // Applying rotation according to inputs
       // transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z - (turnInput * _rotationSpeed));

        // Getting angle which will help to move object to forward
        // + 90 is the factor as 2d sprites have z rotation of -90 degrees while it look visually as angle with 0 degrees. PLay with it if your object is child of another.
        float theta = transform.localEulerAngles.z + 90;

        // Getting new X direction
        float newDirX = Mathf.Cos(theta * Mathf.Deg2Rad);

        // Getting new Y direction
        float newDirY = Mathf.Sin(theta * Mathf.Deg2Rad);

        // Applying velocity according to current angle
        rb.velocity = new Vector2(newDirX, newDirY) * thrustInput* Thrust;
        // rb.AddTorque(-turnInput);
    }

    /// <summary>
    /// Ths method is used to increament the score
    /// </summary>
    void ScorePoints(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score " + score;
    }
    void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;

        spriteRenderer.enabled = true;
        spriteRenderer.color = inColor;

        Invoke(nameof(Invulnerable), 3f);
        //Invoke("Invulnerable", 3f);
    }
    void Invulnerable()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = normalColor;
    }/// <summary>
    /// Used for hyperspace
    /// </summary>
    void HyperSpace()
    {
        //move to a random position
        Vector2 newPosition = new Vector2(Random.Range(-16f, 16f), Random.Range(-8f, 8f));
        transform.position = newPosition;
        //turn off colliders and sprite renderer
        spriteRenderer.enabled = true;
        collider.enabled = true;

        hyperspace = false;
    }

    void LoseLife()
    {
        lives--;
        // livesText.text = "Lives " + lives;
        GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(newExplosion, 3.0f);
        livesText.text = "Lives " + lives;

        //respawn new life
        spriteRenderer.enabled = false;
        collider.enabled = false;
        

        // Debug.Log("Death");
        if (lives <= 0)
        {
            GameOver();
        }
       /* else
        {
            Invoke(nameof(Respawn), 3f);
        }*/
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);

        if(collision.relativeVelocity.magnitude > deathForce)
        {
            LoseLife();
           
        }
        else
        {
            Invoke(nameof(Respawn), 3f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Beam"))
        {
            LoseLife();
            alien.Disable();
        }
    }
    void GameOver()
    {
        gameOver = true;
        CancelInvoke();
        
        //Tell the GameManager to check the highscore
        if(gm.CheckForHighScore(score) )
        {
            newHighScorePanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
            highScoreListText.text = "HIGH SCORE" + "\n" + "\n" + PlayerPrefs.GetString("highscoreName") + " " + PlayerPrefs.GetInt("highscore");
        }

    }
    public void HighScoreInput()
    {
        //if (highScoreInput.Input.GetKeyDown(KeyCode.KeypadEnter))
       
            string newInput = highScoreInput.text;
            Debug.Log(newInput);
        if (highScoreInput.text != string.Empty)
        {
            newHighScorePanel.SetActive(false);
            gameOverPanel.SetActive(true);
            PlayerPrefs.SetString("highScoreName", newInput);
            PlayerPrefs.SetInt("highscore", score);
            highScoreListText.text = "HIGH SCORE" + "\n" + "\n" + PlayerPrefs.GetString("highScoreName") + " " + PlayerPrefs.GetInt("highscore");
        } 
    }
  
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("Main");
    }
   

}
