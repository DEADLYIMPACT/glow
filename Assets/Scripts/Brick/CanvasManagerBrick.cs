using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasManagerBrick : MonoBehaviour
{      
    [SerializeField] private List<Sprite> buttonSprites;
    [SerializeField] private TMP_Text levelHeading;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image next;
    [SerializeField] private Image pausePanel;
    [SerializeField] private Image pauseButton;
    [SerializeField] private Color failedColor;
    [SerializeField] private Color completedColor;
    [SerializeField] public Image fade;
    public Image instructions;
    AudioSource audioSource;    
    public AudioClip winSound;
    public AudioClip powerupSound;
    private float time;
    bool won;

    private void Awake(){
        transform.GetChild(0).gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);        
    }

    private void Start(){
        StartCoroutine(fadeIn(fade, 0.3f));
        audioSource = GetComponent<AudioSource>();
        scoreText.color = GameManagerBrick.Instance.paddleColor;
        pauseButton.color = GameManagerBrick.Instance.paddleColor;
        instructions.color = GameManagerBrick.Instance.paddleColor;
        for(int i = 0; i < 2; i++){
            pausePanel.transform.GetChild(i).GetComponent<Image>().color = GameManagerBrick.Instance.paddleColor;
        }
    }

    public void gameOver(bool won){
        this.won = won;
        StartCoroutine(fadeOut(fade, 0.3f, "Canvas"));
    }    

    public void playWinSound(){
        if(GameModeSelect.doSoundEffects)
            audioSource.PlayOneShot(winSound, 1.5f);
    }

    public void playPowerupSound(){
        if(GameModeSelect.doSoundEffects)
            audioSource.PlayOneShot(powerupSound, 0.75f);
    }

    public void playButtonSound(){
        if(GameModeSelect.doSoundEffects)
            audioSource.Play();
    }

    public void pause(){
        playButtonSound();
        Time.timeScale = 0;
        pausePanel.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void resume(){
        playButtonSound();
        Time.timeScale = 1;
        pausePanel.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    private void goUpAnimation(){
        int n = Int32.Parse(coinText.text) + 1;
        if(n <= GameModeSelect.coins){
            coinText.text = n + "";
            Invoke("goUpAnimation", time);
        }
    }

    public void back(){
        playButtonSound();
        StartCoroutine(fadeOut(fade, 0.3f, "LevelSelect"));
    }

    public void backHome(){
        Time.timeScale = 1;
        playButtonSound();
        StartCoroutine(fadeOut(fade, 0.3f, "GameModeSelector"));
    }

    public void nextLevel(){
        playButtonSound();
        StartCoroutine(fadeOut(fade, 0.3f, "Brick"));
    }

    IEnumerator fadeOut(Image myImage, float duration, string toLoad){
        fade.gameObject.SetActive(true);
        float counter = 0;
        Color color = myImage.color;

        while (counter < duration){
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter / duration);
            myImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        if(toLoad == "Canvas"){
            transform.GetChild(0).gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            string levelString = Convert.ToString(LevelSelect.level);
            if(LevelSelect.level < 10)
                levelString = "0" + levelString;
            levelHeading.text = levelString;
            if(won){
                gameOverText.color = completedColor;
                next.sprite = buttonSprites[0];
                gameOverText.text = "COMPLETED";
            } else {
                gameOverText.color = failedColor;
                next.sprite = buttonSprites[1];
                gameOverText.text = "FAILED";
            }
            if(LevelSelect.level == GameModeSelect.gameModeLevels[GameModeSelect.gameMode]){
                next.sprite = buttonSprites[1];
            }
            int score = GameManagerBrick.Instance.score;
            coinText.text = GameModeSelect.coins + "";
            GameModeSelect.coins += score;
            if(won) {
                GameModeSelect.coins += 50;
            }
            time = 1.0f / (GameModeSelect.coins - Int32.Parse(coinText.text));
            Invoke("goUpAnimation", 1);
            GameManagerBrick.Instance.isWon = won;
            PlayerPrefs.SetInt("coins", GameModeSelect.coins);
            StartCoroutine(fadeIn(fade, 0.3f));
        }
        else
            SceneManager.LoadScene(toLoad);
    }

    IEnumerator fadeIn(Image myImage, float duration){
        float counter = 0;
        Color color = myImage.color;
        while (counter < duration){
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            myImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        fade.gameObject.SetActive(false);
    }
}
