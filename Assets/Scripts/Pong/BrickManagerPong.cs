using System.Collections.Generic;
using UnityEngine;

public class BrickManagerPong : MonoBehaviour
{
    #region Singleton
    
    private static BrickManagerPong brickManager;
    public static BrickManagerPong Instance => brickManager;

    private void Awake(){
        if(brickManager != null)
            Destroy(gameObject);
        else{
            brickManager = this;
        }
    }
    #endregion

    public Sprite[] Sprites;
    private GameObject bricksContainer;
    public BrickPong brick;
    [SerializeField] public BrickPong brickPrefab;
    [SerializeField] public float initSpawnX;
    [SerializeField] public float initSpawnY;
    [SerializeField] float shiftX;
    [SerializeField] float shiftY;
    [SerializeField] SpriteRenderer leftWallSR;
    [SerializeField] SpriteRenderer rightWallSR;    
    public PowerupPong powerup;
    float xPos;
    float yPos;

    private void Start(){
        bricksContainer = new GameObject("BricksContainer");
        leftWallSR.color = GameManagerPong.Instance.color1;
        rightWallSR.color = GameManagerPong.Instance.color2;
    }
    
    public void spawnRandomBrick(int i){
        do{
            int x = Random.Range(0, 7);
            int y = Random.Range(0, 11);
            xPos = initSpawnX + x * shiftX;
            yPos = initSpawnY + y * shiftY;
        }
        while(Mathf.Abs(xPos) < 0.1 || Mathf.Abs(xPos - BallManagerPong.Instance.ballRb.position.x) < 0.35 && Mathf.Abs(xPos - BallManagerPong.Instance.ballRb.position.x) < 0.3);
        Invoke("spawn", i);
    }

    public void spawn(){
        brick = Instantiate(brickPrefab, new Vector2(xPos, yPos), Quaternion.identity);
        if(Random.Range(0, 2) == 1)
            brick.GetComponent<SpriteRenderer>().color = GameManagerPong.Instance.color1;
        else
            brick.GetComponent<SpriteRenderer>().color = GameManagerPong.Instance.color2;
    }


}
