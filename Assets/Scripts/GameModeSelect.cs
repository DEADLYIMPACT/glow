using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameModeSelect : MonoBehaviour {
    #region Singleton

    private static GameModeSelect gameModeSelect;

    public static GameModeSelect Instance => gameModeSelect;
    
    private void Awake(){
        if(gameModeSelect != null)
            Destroy(gameObject);
        else
            gameModeSelect = this;
    }

    #endregion
    [SerializeField] public List<Sprite> skins;
    [SerializeField] public string[] gameModes;
    [SerializeField] private float initalX;
    [SerializeField] private float initalY;
    [SerializeField] private float shiftY;
    [SerializeField] private GameMode gameModePrefab;
    [SerializeField] private GameObject panel;
    [SerializeField] private List<string> keys;
    [SerializeField] private List<int> totalLevels;
    [SerializeField] TMP_Text coinText;
    [SerializeField] public Image fade;
    public static bool doVibrate;
    public static bool playMusic;
    public static bool doSoundEffects;
    public static string gameMode;
    public int selectedBall;
    public static Dictionary<string, int> gameModeLevels;
    public static Dictionary<string, int> gameModeCompletedLevels;
    public static int coins;
    public AudioSource audioSource;

    private void Start(){
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        StartCoroutine(fadeIn(fade, 0.3f));
        gameModeLevels = new Dictionary<string,int>();
        gameModeCompletedLevels = new Dictionary<string,int>();
        coins = PlayerPrefs.GetInt("coins", 0);
        selectedBall = PlayerPrefs.GetInt("skin", 0);
        doVibrate = PlayerPrefs.GetInt("vibrate", 1) == 1 ? true : false;
        doSoundEffects = PlayerPrefs.GetInt("sound", 1) == 1 ? true : false;
        playMusic = PlayerPrefs.GetInt("music", 1) == 1 ? true : false;
        coinText.text = coins + "";
        audioSource = GetComponent<AudioSource>();
        for(int i = 0; i < keys.Count; i++){
            gameModeLevels.Add(keys[i], totalLevels[i]);
            gameModeCompletedLevels.Add(keys[i], PlayerPrefs.GetInt(keys[i], 0));
        }
        float currentY = initalY;
        foreach (string mode in gameModes){
            GameMode gm = Instantiate(gameModePrefab, new Vector2(0, 0), Quaternion.identity) as GameMode;
            RectTransform rt = gm.GetComponent<RectTransform>();
            rt.SetParent(panel.transform);
            rt.anchorMin = new Vector2(0.5f, 1);
            rt.anchorMax = new Vector2(0.5f, 1);
            rt.localScale = new Vector2(1, 1);
            rt.anchoredPosition = new Vector2(initalX, currentY);
            gm.GetComponentInChildren<Text>().text = mode.ToUpper();
            gm.mode = mode;
            currentY += shiftY;
        }
    }

    public void toShop(){
        StartCoroutine(fadeOut(fade, 0.3f, "Shop"));
    }

    public IEnumerator fadeOut(Image myImage, float duration, string toLoad){
        fade.gameObject.SetActive(true);
        float counter = 0;
        Color spriteColor = myImage.color;
        if(doSoundEffects)
            audioSource.Play();
        while (counter < duration){
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter / duration);
            myImage.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
        SceneManager.LoadScene(toLoad);
    }

    IEnumerator fadeIn(Image myImage, float duration){
        float counter = 0;
        Color spriteColor = myImage.color;

        while (counter < duration){
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            myImage.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
        fade.gameObject.SetActive(false);
    }
}
