using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gameWonUI;

    private Frogger frogger;
    private Home[] homes;
    private int score;
    private int lives;
    private bool levelDone;
    public Text livesText;
    public Text scoreText;

    public void gameOver()
    {
        if (!levelDone)
            gameOverUI.SetActive(true);
        else
            gameWonUI.SetActive(true);
    }

    public void Start()
    {
        NewGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        NewGame();
    }

    private void Awake()
    {
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
    }

    public void Died()
    {
        SetLives(lives - 1);
        if(lives == 0)
        {
            Invoke(nameof(gameOver), 1.5f);
        }
        else
        {
            Invoke(nameof(Respawn), 1f);
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewLevel();
    }

    private void NewLevel()
    {
        for(int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }

        NewRound();
    }

    private void NewRound()
    {
        Respawn();
    }

    private void Respawn()
    {
        frogger.Respawn();
    }

    public void HomeReached()
    {
        frogger.gameObject.SetActive(false);

        SetScore(score + 50);
        if (LevelDone())
        {
            levelDone = true;
            SetScore(score + 1000);
            Invoke(nameof(gameOver), 1.5f);
        }else
        {
            Invoke(nameof(NewRound), 1f);
        }
    }

    private bool LevelDone()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            if (!homes[i].enabled)
                return false;
        }
        return true;
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }
}
