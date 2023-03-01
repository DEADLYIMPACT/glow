using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBrick : MonoBehaviour
{
    #region Singleton

    private static GameManagerBrick gameManager;
    public static GameManagerBrick Instance => gameManager;

    [SerializeField] TextAsset levelText;
    [SerializeField] TextAsset colors;
    
    private void Awake(){
        if(gameManager != null)
            Destroy(gameObject);
        else{
            gameManager = this;
        }
    }
    #endregion

    public bool isGameStarted {get; set;}
    public Color color1;
    public Color color2;
    [SerializeField] public Color color3;
    public Color paddleColor;
    public int level;
    public static int totalLevels = 2;
    public bool isWon;
    public int score;
    public bool isShowingInst;
    bool isFirstBrick;
    public InstructionsBrick inst;

    public int[,] loadLevelData(){
        level = LevelSelect.level;
        string[] rows = levelText.text.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        int i = 1;
        List<string> currentlevel = new List<string>();
        foreach(String row in rows){
            if(i == level)
                currentlevel.Add(row);
            if(row.IndexOf("--") != -1)
                i++;
            if(i > level)
                break;
        }
        rows = colors.text.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        int color = (level - 1) % 4;
        ColorUtility.TryParseHtmlString("#" + rows[color * 4].Substring(0, 6), out color1);
        ColorUtility.TryParseHtmlString("#" + rows[color * 4 + 1].Substring(0, 6), out color2);
        ColorUtility.TryParseHtmlString("#" + rows[color * 4 + 2].Substring(0, 6), out paddleColor);
        int[,] bricksPos = new int[15, 7];
        for(int j = 0; j < currentlevel.Count - 1; j++){
            string[] bricks = currentlevel[j].Split(new char[] {',', ' ', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            for(int col = 0; col < bricks.Length; col++){
                bricksPos[j, col] = Int32.Parse(bricks[col]);
            }
        }
        return bricksPos;
    }

    void Start(){
        isFirstBrick = PlayerPrefs.GetInt("brickInst", 1) == 1 ? true : false;
        if(isFirstBrick)
            showInstructions();
    }

    public void showInstructions(){
        inst.gameObject.SetActive(true);
        inst.showInst();
        isShowingInst = true;
        if(isFirstBrick){
            isFirstBrick = false;
            PlayerPrefs.SetInt("brickInst", 0);
        } else {
            FindObjectOfType<CanvasManagerBrick>().playButtonSound();
        }
    }

    public void nextLevel(){
        if(isWon && LevelSelect.level != GameModeSelect.gameModeLevels[GameModeSelect.gameMode]){
            LevelSelect.level++;
        }
        FindObjectOfType<CanvasManagerBrick>().nextLevel();
    }
}
