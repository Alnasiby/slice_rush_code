using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;   // ⭐ REQUIRED FOR TEXTMESH PRO

public class GameManager : MonoBehaviour
{
    [Header("Score UI")]
    public TMP_Text scoreText;          // Live score text

    [Header("Game Over UI")]
    public GameObject gameOverPanel;    // Panel
    public TMP_Text gameOverText;       // "Game Over"
    public TMP_Text finalScoreText;     // Final Score
    public TMP_Text highScoreText;      // ⭐ High Score Text
    public Button playAgainButton;      // Restart button

    private int score;
    private bool isGameOver = false;

    private const string HIGH_SCORE_KEY = "HighScore";

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        score = 0;
        isGameOver = false;

        if (scoreText != null)
            scoreText.text = score.ToString();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        if (finalScoreText != null)
            finalScoreText.gameObject.SetActive(false);

        if (highScoreText != null)
            highScoreText.gameObject.SetActive(false);

        if (playAgainButton != null)
            playAgainButton.interactable = true;
    }

    public void IncreaseScore(int amount = 1)
    {
        if (isGameOver) return;

        score += amount;

        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        isGameOver = true;

        // Stop spawner
        Spawner spawner = FindObjectOfType<Spawner>();
        if (spawner != null)
            spawner.enabled = false;

        // Destroy fruits
        foreach (Fruit f in FindObjectsOfType<Fruit>())
            Destroy(f.gameObject);

        // Destroy bombs
        foreach (Bomb b in FindObjectsOfType<Bomb>())
            Destroy(b.gameObject);

        // Show UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);

        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(true);
            finalScoreText.text = "Score: " + score;
        }

        // ⭐ HIGH SCORE
        int savedHighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

        if (score > savedHighScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
            savedHighScore = score;
        }

        if (highScoreText != null)
        {
            highScoreText.gameObject.SetActive(true);
            highScoreText.text = "High Score: " + savedHighScore;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (playAgainButton != null)
            playAgainButton.interactable = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
            RestartGame();
    }
}
