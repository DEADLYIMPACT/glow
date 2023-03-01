using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerPong : MonoBehaviour
{
    #region Singleton

    private static GameManagerPong gameManager;
    public static GameManagerPong Instance => gameManager;

    [SerializeField] TextAsset text;
    
    private void Awake(){
        if(gameManager != null)
            Destroy(gameObject);
        else{
            gameManager = this;
            loadLevelData();
        }
    }
    #endregion

    public bool isGameStarted {get; set;}
    public bool isGameEnded {get; set;}
    public Color color1;
    public Color color2;
    public Color paddleColor;
    public int score;
    public bool isShowingInst;
    bool isFirstPong;
    public InstructionsPong inst;

    public void loadLevelData(){
        int color = UnityEngine.Random.Range(1, 5);
        string[] rows = text.text.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        int i = 1;
        List<string> currentlevel = new List<string>();
        foreach(String row in rows){
            if(i == color)
                currentlevel.Add(row);
            if(row.IndexOf("--") != -1)
                i++;
            if(i > color)
                break;
        }
        ColorUtility.TryParseHtmlString("#" + currentlevel[0].Substring(0, 6), out color1);
        ColorUtility.TryParseHtmlString("#" + currentlevel[1].Substring(0, 6), out color2);
        ColorUtility.TryParseHtmlString("#" + currentlevel[2].Substring(0, 6), out paddleColor);
    }

    void Start(){
        isFirstPong = PlayerPrefs.GetInt("pongInst", 1) == 1 ? true : false;
        if(isFirstPong)
            showInstructions();
    }

    public void showInstructions(){
        inst.gameObject.SetActive(true);
        inst.showInst();
        isShowingInst = true;
        if(isFirstPong){
            isFirstPong = false;
            PlayerPrefs.SetInt("pongInst", 0);
        } else {
            FindObjectOfType<CanvasManagerPong>().playButtonSound();
        }
    }
}
