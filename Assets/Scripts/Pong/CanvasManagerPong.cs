using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasManagerPong : MonoBehaviour
{      
    [SerializeField] private TMP_Text scoreHeading;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image pausePanel;
    [SerializeField] private Image pauseButton;
    [SerializeField] private Image fade;
    [SerializeField] private Color failedColor;
    [SerializeField] private Color completedColor;
    private float time;
    public Image instructions;
    AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip powerupSound;    
    bool isHS;

    private void Awake(){
        transform.GetChild(0).gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
    }

    private void Start(){
        StartCoroutine(fadeIn(fade, 0.3f));
        scoreText.color = GameManagerPong.Instance.paddleColor;
        pauseButton.color = GameManagerPong.Instance.paddleColor;
        instructions.color = GameManagerPong.Instance.paddleColor;
        audioSource = GetComponent<AudioSource>();
        for(int i = 0; i < 2; i++){
            pausePanel.transform.GetChild(i).GetComponent<Image>().color = GameManagerPong.Instance.paddleColor;
        }
    }

    public void gameOver(bool isHS){
        this.isHS = isHS;
        StartCoroutine(fadeOut(fade, 0.3f, "Canvas"));
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

    public void backHome(){
        Time.timeScale = 1;
        playButtonSound();
        StartCoroutine(fadeOut(fade, 0.3f, "GameModeSelector"));
    }

    public void nextLevel(){
        playButtonSound();
        StartCoroutine(fadeOut(fade, 0.3f, "Pong"));
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
            scoreText.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
            int score = GameManagerPong.Instance.score;
            coinText.text = GameModeSelect.coins + "";
            GameModeSelect.coins += score;
            time = 1.0f / (GameModeSelect.coins - Int32.Parse(coinText.text));
            Invoke("goUpAnimation", 1);
            scoreHeading.text = score + "";
            if(isHS){
                gameOverText.color = completedColor;
                gameOverText.text = "NEW BEST";
            } else {
                gameOverText.color = failedColor;
                gameOverText.text = "BEST - " + GameModeSelect.gameModeCompletedLevels[GameModeSelect.gameMode];
            }
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
