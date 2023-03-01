using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class ShopManager : MonoBehaviour {

    #region Singleton

    private static ShopManager shopManager;

    public static ShopManager Instance => shopManager;
    
    private void Awake(){
        if(shopManager != null)
            Destroy(gameObject);
        else
            shopManager = this;
    }

    #endregion

    public int[] prices;
    public bool[] isOwned;
    public static int level;
    bool goingDown;
    private string[] rows;
    [SerializeField] TextAsset skinsText;
    [SerializeField] RectTransform shopLinePrefab;
    [SerializeField] RectTransform panel;
    [SerializeField] TMP_Text coinText;
    [SerializeField] public Color ownedColor;
    [SerializeField] public Color canBuyColor;
    [SerializeField] public Color cantBuyColor;
    [SerializeField] public Color selectedColor;
    [SerializeField] public RectTransform shopScroll;
    [SerializeField] Image fade;
    private int totalSkins;
    public List<RectTransform> shopLines;
    AudioSource audioSource;    
    public AudioClip winSound;
    float time;

    private void Start(){
        StartCoroutine(fadeIn(fade, 0.3f));
        audioSource = GetComponent<AudioSource>();
        List<Sprite> skins = GameModeSelect.Instance.skins;
        totalSkins = skins.Count;
        shopLines = new List<RectTransform>();
        string path = Application.persistentDataPath + "/balls.txt";
        if(!File.Exists(path)){
            File.WriteAllLines(path, skinsText.text.Split(new string[] {Environment.NewLine}, StringSplitOptions.None));
        } 
        rows = File.ReadAllLines(Application.persistentDataPath + "/balls.txt");
        prices = new int[totalSkins];
        isOwned = new bool[totalSkins];
        for(int i = 0; i < rows.Length; i += 3){
            prices[i / 3] = Int32.Parse(rows[i + 1]);
            isOwned[i / 3] = Convert.ToBoolean(rows[i + 2]);
        }
        for(int i = 1; i <= totalSkins; i += 2){
            RectTransform shopLine = Instantiate(shopLinePrefab, new Vector2(0, 0), Quaternion.identity) as RectTransform;
            shopLines.Add(shopLine);
            fixSkins(i, shopLine);
            shopLine.SetParent(panel);
            shopLine.localScale = new Vector2(1, 1);                         
        }
        panel.localPosition = new Vector2(0, totalSkins * -50 - 10000);
        coinText.text = GameModeSelect.coins + "";
    }

    public void fixSkins(int firstSkinNo, RectTransform shopLine){
        for(int i = 0; i < 2; i++){
            Shop skin = shopLine.transform.GetChild(i).GetComponent<Shop>();
            if(firstSkinNo + i > totalSkins){
                Destroy(skin);
            }
            else{
                skin.n = firstSkinNo + i - 1;
                skin.fixSkin();
            }
       }
    }

    public void back(){
        StartCoroutine(fadeOut(fade, 0.3f));
    }

    public void buySkin(int n){
        isOwned[n] = true;
        string path = Application.persistentDataPath + "/balls.txt";
        string[] lines = File.ReadAllLines(path);
        lines[n * 3 + 2] = "true";
        File.WriteAllLines(path, lines);
        GameModeSelect.coins -= prices[n];
        PlayerPrefs.SetInt("coins", GameModeSelect.coins);
        int decrement = (Int32.Parse(coinText.text) - GameModeSelect.coins) / 100 + UnityEngine.Random.Range(1, 10);
        if(GameModeSelect.doSoundEffects)
            audioSource.PlayOneShot(winSound, 1f);
        if(!goingDown)
            StartCoroutine(StartCountdown(decrement));
        else
            decrement = (Int32.Parse(coinText.text) - GameModeSelect.coins) / 100 + UnityEngine.Random.Range(1, 10);
        goingDown = true;
        selectBall(n);
        foreach(RectTransform rt in shopLines){
            rt.GetChild(0).GetComponent<Shop>().fixSkin();
            rt.GetChild(1).GetComponent<Shop>().fixSkin();
        }
    }

    public IEnumerator StartCountdown(int decrement){
        float currCountdownValue = Int32.Parse(coinText.text);
        while (currCountdownValue > GameModeSelect.coins + decrement){
            currCountdownValue -= decrement;
            coinText.text = currCountdownValue + "";
            yield return new WaitForSeconds(0.0175f);
        }
        coinText.text = GameModeSelect.coins + "";
        goingDown = false;
    }

    public void selectBall(int n){
        int selectBall = GameModeSelect.Instance.selectedBall;
        PlayerPrefs.SetInt("skin", n);
        Transform shop = shopLines[selectBall / 2].transform.GetChild(selectBall % 2);
        shop.GetComponent<Image>().color = ShopManager.Instance.ownedColor;
        shop.GetComponent<Button>().interactable = true;
        GameModeSelect.Instance.selectedBall = n;
    }


    IEnumerator fadeOut(Image myImage, float duration){
        fade.gameObject.SetActive(true);
        float counter = 0;
        Color spriteColor = myImage.color;
        if(GameModeSelect.doSoundEffects)
            audioSource.Play();
        while (counter < duration){
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter / duration);
            myImage.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
        SceneManager.LoadScene("GameModeSelector");
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
