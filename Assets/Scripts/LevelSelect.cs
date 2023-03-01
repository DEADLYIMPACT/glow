using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{   
    #region Singleton

    private static LevelSelect levelSelect;

    public static LevelSelect Instance => levelSelect;
    
    private void Awake(){
        if(levelSelect != null)
            Destroy(gameObject);
        else
            levelSelect = this;
    }

    #endregion
    public static int level;
    [SerializeField] RectTransform levelLinePrefab;
    [SerializeField] RectTransform panel;
    [SerializeField] TMP_Text heading;
    [SerializeField] TMP_Text fraction;
    [SerializeField] public Color completeColor;
    [SerializeField] public Color currentColor;
    [SerializeField] public Color lockedColor;
    [SerializeField] public RectTransform levelScroll;
    [SerializeField] public Image fade;
    private int totalLevels;
    private int completedLevels;

    private void Start(){
        StartCoroutine(fadeIn(fade, 0.3f));
        heading.text = GameModeSelect.gameMode.ToUpper();
        totalLevels = GameModeSelect.gameModeLevels[GameModeSelect.gameMode];
        completedLevels = GameModeSelect.gameModeCompletedLevels[GameModeSelect.gameMode];
        int levelsRemaining = totalLevels;
        for(int i = 1; i <= totalLevels; i += 4){
            RectTransform levelLine = Instantiate(levelLinePrefab, new Vector2(0, 0), Quaternion.identity) as RectTransform;
            RectTransform rt = levelLine.GetComponent<RectTransform>();
            rt.SetParent(panel);     
            rt.localScale = new Vector2(1, 1);                         
            fixLevels(i, levelLine);
        }
        panel.localPosition = new Vector2(0, totalLevels * -25);

        string fractionText = completedLevels + "/" + totalLevels;
        if(0 < completedLevels && completedLevels < 10)
            fractionText = "0" + fractionText;
        fraction.text = fractionText;
    }


    public void back(){
        StartCoroutine(fadeOut(fade, 0.3f, "GameModeSelector"));
    }

    public void fixLevels(int firstLevelNo, RectTransform levelLine){
        for(int i = 0; i < 4; i++){
            Transform level = levelLine.transform.GetChild(i);
            if(firstLevelNo + i > totalLevels){
                Destroy(level.gameObject);
                Debug.Log(firstLevelNo + i);
            }
            else
                level.GetChild(0).GetComponent<Text>().text = Convert.ToString(firstLevelNo + i);
            if(firstLevelNo + i - 1 == completedLevels)
                level.GetComponent<UnityEngine.UI.Image>().color = currentColor;
            else if(firstLevelNo + i - 1 < completedLevels)
                level.GetComponent<UnityEngine.UI.Image>().color = completeColor;             
            else{
                level.GetComponent<UnityEngine.UI.Image>().color = lockedColor;
                level.transform.GetChild(0).GetComponent<Text>().color = new Color(255, 255, 255, 100);
                level.GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
       }
    }

    public IEnumerator fadeOut(Image myImage, float duration, string toLoad){
        fade.gameObject.SetActive(true);
        float counter = 0;
        Color spriteColor = myImage.color;
        if(GameModeSelect.doSoundEffects)
            GetComponent<AudioSource>().Play();

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
