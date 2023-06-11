using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button menuButton;
    public Button playButton;
    public Text scoreText;
    public Text currentScoreText;
    public Text HighScoreText;
    public Image fadeImage;
    private string highScoreKey = "HighScore"; 

    private Blade blade;
    private Spawner spawner;

    private int score;
    private int highScore = 0;

    [SerializeField]
    private GameObject gameOverPanel;

    private void Awake() {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
    }

    private void Start() {
        NewGame();
    }

    private void NewGame (){
        Time.timeScale = 1f;
        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString(); 
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);

        ClearScene();
    }

    public void MenuButton() {
        SceneManager.LoadScene("MenuScene");
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("PlayScene");
    }

    private void ClearScene() {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach(Fruit fruit in fruits){
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach(Bomb bomb in bombs){
            Destroy(bomb.gameObject);
        }
        
    }

    public void IncreaseScore() {
        score++;
        scoreText.text = score.ToString();
        if (score > highScore) 
        {
            highScore = score;
            PlayerPrefs.SetInt(highScoreKey, highScore);
        }
    }

    public void Explode() {
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExPlodeSequence());
    }

    public void showPanel () {
        gameOverPanel.gameObject.SetActive(true);
        currentScoreText.text = score.ToString();
        HighScoreText.text = highScore.ToString(); 
    }

    private IEnumerator ExPlodeSequence() {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration) {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;
            fadeImage.color = Color.Lerp( Color.white, Color.clear, t);
            yield return null;
        }

        ClearScene();
        scoreText.gameObject.SetActive(false);
        showPanel();
        Time.timeScale = 0;
    }
}
