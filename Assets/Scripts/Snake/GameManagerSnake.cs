using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSnake : MonoBehaviour
{
    public static bool isGameStarted;
    public static bool isGameEnded;
    public static Color color1;
    public static Color color2;
    public static Color paddleColor;
    public int score;
    public TextAsset text;
    public bool isShowingInst;
    bool isFirstSnake;
    public InstructionsSnake inst;
    CanvasManagerSnake cms;

    void Awake(){
        loadLevelData();
        isGameStarted = false;
        isGameEnded = false;
    }

    void Update(){
        if(!isGameStarted && !isGameEnded && !isShowingInst &&Input.GetMouseButton(0) && Camera.main.ScreenToWorldPoint(new Vector2(0, Input.mousePosition.y)).y < 3.5 &&  Time.timeScale != 0){
            isGameStarted = true;
            cms.instructions.gameObject.SetActive(false);
        }
    }

    void Start(){
        cms = FindObjectOfType<CanvasManagerSnake>();
        isFirstSnake = PlayerPrefs.GetInt("snakeInst", 1) == 1 ? true : false;
        if(isFirstSnake)
            showInstructions();
    }

    public void showInstructions(){
        inst.gameObject.SetActive(true);
        inst.showInst();
        isShowingInst = true;
        if(isFirstSnake){
            isFirstSnake = false;
            PlayerPrefs.SetInt("snakeInst", 0);
        } else {
            FindObjectOfType<CanvasManagerSnake>().playButtonSound();
        }
    }

    public void gameOver(){
        cms.gameObject.SetActive(true);
        bool isHS = false;  
        if(GameModeSelect.gameModeCompletedLevels[GameModeSelect.gameMode] < score){
            GameModeSelect.gameModeCompletedLevels[GameModeSelect.gameMode] = score;
            PlayerPrefs.SetInt(GameModeSelect.gameMode, score);
            isHS = true;
        }
        cms.gameOver(isHS);   
    }

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
        ColorUtility.TryParseHtmlString("#" + currentlevel[0].Substring(7, 6), out color1);
        ColorUtility.TryParseHtmlString("#" + currentlevel[1].Substring(7, 6), out color2);
        ColorUtility.TryParseHtmlString("#" + currentlevel[2].Substring(7, 6), out paddleColor);
    }

}
