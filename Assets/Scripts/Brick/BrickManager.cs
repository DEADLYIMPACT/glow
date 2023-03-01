using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    #region Singleton
    
    private static BrickManager brickManager;
    public static BrickManager Instance => brickManager;

    private void Awake(){
        if(brickManager != null)
            Destroy(gameObject);
        else{
            brickManager = this;
            levelData = GameManagerBrick.Instance.loadLevelData();
        }
    }
    #endregion

    public Sprite[] Sprites;
    public int[,] levelData {get; set;}
    private GameObject bricksContainer;
    public List<Brick> bricks {get; set;}
    private float currentSpawnX;
    private float currentSpawnY;
    [SerializeField] public Brick brickPrefab;
    [SerializeField] public float initSpawnX;
    [SerializeField] public float initSpawnY;
    [SerializeField] public float shiftX;
    [SerializeField] public float shiftY;
    [SerializeField] SpriteRenderer leftWallSR;
    [SerializeField] SpriteRenderer rightWallSR;    
    [SerializeField] CanvasManagerBrick canvasManagerBrick;
    public List<PowerupBrick> powerupList;

    private void Start(){
        bricksContainer = new GameObject("BricksContainer");
        bricks = new List<Brick>();
        powerupList = new List<PowerupBrick>();
        generateBricks();
        leftWallSR.color = GameManagerBrick.Instance.color1;
        rightWallSR.color = GameManagerBrick.Instance.color2;
    }

    private void generateBricks(){
        currentSpawnX = initSpawnX;
        currentSpawnY = initSpawnY;
        for (int r = 0; r < 15; r++){
            for (int c = 0; c < 7; c++){
                int brickType = levelData[r, c];
                if(brickType > 0){
                    Brick newBrick = Instantiate(brickPrefab, new Vector2(currentSpawnX, currentSpawnY), Quaternion.identity) as Brick;
                    newBrick.transform.parent = bricksContainer.transform;
                    if(brickType == 1){
                        newBrick.GetComponent<SpriteRenderer>().color = GameManagerBrick.Instance.color1;
                        bricks.Add(newBrick);
                    }
                    if(brickType == 2){
                        newBrick.GetComponent<SpriteRenderer>().color = GameManagerBrick.Instance.color2;
                        bricks.Add(newBrick);
                    }
                    if(brickType == 3){
                         newBrick.GetComponent<SpriteRenderer>().color = GameManagerBrick.Instance.color3;
                    }
                }
                currentSpawnX += shiftX;
            }
            currentSpawnY += shiftY;
            currentSpawnX = initSpawnX;
        }
    }

    public void resetPowerups(){
        List<PowerupBrick> temp = new List<PowerupBrick>(powerupList);
        foreach(PowerupBrick powerup in temp)
            powerup.die();
    }

    public void destroyAllBricks(){
        foreach(Brick brick in new List<Brick>(bricks))
            brick.destroyBrick();
    }

    public void levelComplete(){
        if(GameModeSelect.gameModeCompletedLevels[GameModeSelect.gameMode] == LevelSelect.level - 1){
            GameModeSelect.gameModeCompletedLevels[GameModeSelect.gameMode]++;
            PlayerPrefs.SetInt(GameModeSelect.gameMode, PlayerPrefs.GetInt(GameModeSelect.gameMode, 0) + 1);
        }
        canvasManagerBrick.gameOver(true);
    }
}
