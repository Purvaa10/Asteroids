using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int numberOfAsteroids;   //this is the current no of asteroids in the scene
    public int levelNumber = 1;
    private Button button;

    public GameObject asteroid;
    public AlienScript alien;


    public void UpdateNumberOfAsteroids(int change)
    {
        numberOfAsteroids += change;

        //check if we have any asteroids left

        if(numberOfAsteroids <= 0)
        {
            //satrt new level
            //Invoke("StartNewLevel", 3.0f);
            Invoke(nameof(StartNewLevel), 3.0f);
        }
        
    }
    void StartNewLevel()
    {
        levelNumber++;

        //spawn new asteroids

        for (int i = 0; i < levelNumber*2; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-17.2f, 17.2f), 11f);
            Instantiate(asteroid, spawnPosition, Quaternion.identity);
            numberOfAsteroids++;
        }
        //setup the alien
        alien.NewLevel();
    }
    public bool CheckForHighScore(int score)
    {
        int highScore = PlayerPrefs.GetInt("highscore");
        if(score > highScore)
        {
            return true;
        }
        return false;
    }

}
