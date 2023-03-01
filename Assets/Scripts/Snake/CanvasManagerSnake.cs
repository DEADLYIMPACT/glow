using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasManagerSnake : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreHeading;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image pausePanel;
    [SerializeField] private Image pauseButton;
    [SerializeField] private Color failedColor;
    [SerializeField] private Color completedColor;
    [SerializeField] Image fade;
    public GameManagerSnake gameManagerSnake;
    private float time;
    public Image instructions;
    bool isHs;
    AudioSource audioSource;
    public AudioClip breakSound;
    public AudioClip powerupSound;
    public AudioClip hitSound;

    private void Awake(){
        transform.GetChild(0).gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
    }

    void Update(){
        scoreText.text = gameManagerSnake.score + "";
    }

    private void Start(){
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(fadeIn(fade, 0.3f));
        scoreText.color = GameManagerSnake.paddleColor;
        pauseButton.color = GameManagerSnake.paddleColor;
        instructions.color = GameManagerSnake.paddleColor;
        for(int i = 0; i < 2; i++){
            pausePanel.transform.GetChild(i).GetComponent<Image>().color = GameManagerSnake.paddleColor;
        }
    }

    public void gameOver(bool isHs){
        this.isHs = isHs;
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

    public void backHome(){
        Time.timeScale = 1;
        playButtonSound();
        StartCoroutine(fadeOut(fade, 0.3f, "GameModeSelector"));
    }

    public void nextLevel(){
        playButtonSound();
        StartCoroutine(fadeOut(fade, 0.3f, "Snake"));
    }

    public void playButtonSound(){
        if(GameModeSelect.doSoundEffects)
            audioSource.Play();
    }

    public void playBreakSound(){
        if(GameModeSelect.doSoundEffects){
            audioSource.PlayOneShot(breakSound, 0.75f);
        }
    }

    public void playPowerupSound(){
        if(GameModeSelect.doSoundEffects){
            audioSource.PlayOneShot(powerupSound, 0.75f);
        }
    }

    public void playHitSound(){
        if(GameModeSelect.doSoundEffects){
            audioSource.PlayOneShot(hitSound, 1f);
        }
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
            int score =  gameManagerSnake.score;
            coinText.text = GameModeSelect.coins + "";
            GameModeSelect.coins += score;
            time = 1.0f / (GameModeSelect.coins - Int32.Parse(coinText.text));
            Invoke("goUpAnimation", 1);
            scoreHeading.text = score + "";
            if(isHs){
                gameOverText.color = completedColor;
                gameOverText.text = "NEW BEST";
            } else {
                gameOverText.color = failedColor;
                gameOverText.text = "BEST - " + GameModeSelect.gameModeCompletedLevels[GameModeSelect.gameMode];
            }
            PlayerPrefs.SetInt("coins", GameModeSelect.coins);
            BlockManagerSnake bm = FindObjectOfType<BlockManagerSnake>();
            foreach(Transform food in new List<Transform>(bm.foods)){
                Destroy(food.gameObject);            
            }
            foreach(BlockSnake block in FindObjectsOfType<BlockSnake>()){
                Destroy(block.gameObject);
            }
            foreach(BarrierSnake barrier in FindObjectsOfType<BarrierSnake>()){
                Destroy(barrier.gameObject);
                StartCoroutine(fadeIn(fade, 0.3f));
            }
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
