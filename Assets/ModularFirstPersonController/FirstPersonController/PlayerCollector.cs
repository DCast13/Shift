using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlayerCollector : MonoBehaviour {

    public int score = 0;

    public Text scoreText;

    public GameObject winScreen;

    public Text winMessage;

    private bool gameEnd = false;
    
    void Start () {

        UpdateScoreUI();

        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }

    }

    void UpdateScoreUI(){

        if (scoreText != null) {

            scoreText.text = "Score: " + score.ToString();

        }
    }

    void OnTriggerEnter(Collider other){

        if (gameEnd)
        {
            return;
        }

        if (other.gameObject.tag == "Collectable")
        {
            score += 1;
            Destroy(other.gameObject);
            UpdateScoreUI();
            Debug.Log("Score: " + score);

            if (score >= 8)
            {

                SceneManager.LoadScene("GameWon");

                EndGame();
            }
        }
    }

    void EndGame()
    {
        gameEnd = true;
        Debug.Log("Thanks for playing our demo!");

        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }

        if (winMessage != null)
        {
            winMessage.text = "Thanks for playing our demo!";
        }

        Time.timeScale = 0f;
    }
}