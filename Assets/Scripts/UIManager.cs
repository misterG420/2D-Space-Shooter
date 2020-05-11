using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text ammoText;
    [SerializeField]
    private Text currentAmmoText;

    [SerializeField]
    private Text gameOverText;

    [SerializeField]
    private Text thrusterDepleted;

    [SerializeField]
    private Text thrusterDepletionIndicator;

    [SerializeField]
    private Sprite[] livesSprites;
    
    [SerializeField]
    private Image livesImage;

    private GameManager gameManager;

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreText.text = "Score: 0";

        if(gameManager == null)
        {
            Debug.Log("GameManager is null");
        }
    }

    private void Update()
    {
        currentAmmoText.text = "Ammo: " + Player.ammo + " /15 Ammo";
        thrusterDepletionIndicator.text = "Thruster capacity: " + Player.lostThruster.ToString("F0");
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

    public void ShowLowAmmo()
    {
        ammoText.gameObject.SetActive(true);
        StartCoroutine(DisableTextRoutine());
    }

    public void ShowThrusterDepleted()
    {
        thrusterDepleted.gameObject.SetActive(true);
        StartCoroutine(DisableTextRoutine());
    }

    public void HideThrusterText()
    {
        thrusterDepleted.gameObject.SetActive(false);
    }

    IEnumerator DisableTextRoutine()
    {
        yield return new WaitForSeconds(5);
        ammoText.gameObject.SetActive(false);
    }
}
