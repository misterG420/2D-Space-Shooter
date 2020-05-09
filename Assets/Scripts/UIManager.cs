using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text gameOverText;

    [SerializeField]
    private Sprite[] livesSprites;
    
    [SerializeField]
    private Image livesImage;

    private GameManager gameManager;

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreText.text = "Score: ";

        if(gameManager == null)
        {
            Debug.Log("GameManager is null");
        }
    }


    public void UpdateScore(int playerScore)
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        livesImage.sprite = livesSprites[currentLives];

        if (currentLives < 1)
            GameOverSequence();
    }

    void GameOverSequence()
    {
        gameOverText.gameObject.SetActive(true);
        gameManager.GameOver();
    }

}
