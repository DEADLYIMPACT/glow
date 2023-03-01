using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManagerBrick : MonoBehaviour
{
    #region Singleton
    
    private static BallManagerBrick ballManager;

    public static BallManagerBrick Instance => ballManager;

    private void Awake(){
        if(ballManager != null)
            Destroy(gameObject);
        else{
            ballManager = this;
        }
    }

    #endregion
    
    [SerializeField] private BallBrick ballPrefab;
    private BallBrick initialBall;
    private Rigidbody2D initBallRb;

    public float initBallSpeed = 250; 
    public float currentBallSpeed;
    public bool isSpeedDiff;
    private float startSpeedTime, endSpeedTime;

    public List<BallBrick> Balls {get; set;}
    
    private void Start(){
        InitBall();
        currentBallSpeed = initBallSpeed;
        isSpeedDiff = false;
    }

    private void Update(){
        if(!GameManagerBrick.Instance.isGameStarted){
            Vector2 paddlePosition = PaddleBrick.Instance.gameObject.transform.position;
            initialBall.transform.position = new Vector2(paddlePosition.x, paddlePosition.y + .27f);

            if(Input.GetMouseButton(0) && Camera.main.ScreenToWorldPoint(new Vector2(0, Input.mousePosition.y)).y < 3.5 && Time.timeScale != 0 && !GameManagerBrick.Instance.isShowingInst){
                initBallRb.isKinematic = false;
                initBallRb.AddForce(new Vector2(Random.Range(-150, 150), initBallSpeed));
                GameManagerBrick.Instance.isGameStarted = true;
                FindObjectOfType<CanvasManagerBrick>().instructions.gameObject.SetActive(false);
            }
        }
        if(startSpeedTime < endSpeedTime)
            startSpeedTime += Time.deltaTime;
        else if(isSpeedDiff)
            resetSpeed();
    }

    private void InitBall(){
        Vector2 paddlePosition = PaddleBrick.Instance.gameObject.transform.position;
        Vector2 startingPosition = new Vector2(paddlePosition.x, paddlePosition.y + .27f);
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        initBallRb = initialBall.GetComponent<Rigidbody2D>();
        SpriteRenderer sr = initialBall.GetComponent<SpriteRenderer>();
        sr.sprite = GameModeSelect.Instance.skins[GameModeSelect.Instance.selectedBall];
        sr.color = GameManagerBrick.Instance.color1;
        Balls = new List<BallBrick>{initialBall};
    }

    public void multiple(){
        Color[] color = new Color[]{GameManagerBrick.Instance.color1, GameManagerBrick.Instance.color2};
        Vector2 paddlePosition = PaddleBrick.Instance.gameObject.transform.position;
        Vector2 startingPosition = new Vector2(paddlePosition.x, paddlePosition.y + .27f);
        for(int i = 0; i < 2; i++){
            BallBrick ball = Instantiate(ballPrefab, startingPosition, Quaternion.identity) as BallBrick;
            SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
            sr.sprite = GameModeSelect.Instance.skins[GameModeSelect.Instance.selectedBall];
            sr.color = color[i];
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            rb.AddForce(new Vector2(Random.Range(-150, 150), currentBallSpeed));
            Balls.Add(ball);
        }
    }
    
    public void fast(){
        foreach(BallBrick ball in BallManagerBrick.Instance.Balls){
            ball.GetComponent<Rigidbody2D>().velocity *= 2 * initBallSpeed / currentBallSpeed;
        }
        currentBallSpeed = initBallSpeed * 2;
        isSpeedDiff = true;
        startSpeedTime = 0;
        endSpeedTime = 15;
    }

    public void slow(){
        foreach(BallBrick ball in BallManagerBrick.Instance.Balls){
            ball.GetComponent<Rigidbody2D>().velocity *= 0.5f * initBallSpeed / currentBallSpeed;
        }
        currentBallSpeed = initBallSpeed * 0.5f;
        isSpeedDiff = true;
        startSpeedTime = 0;
        endSpeedTime = 15;
    }

    private void resetSpeed(){
        isSpeedDiff = false;
        foreach(BallBrick ball in BallManagerBrick.Instance.Balls){
            ball.GetComponent<Rigidbody2D>().velocity *= initBallSpeed / currentBallSpeed;
        }
        currentBallSpeed = initBallSpeed;
    }

    public void resetBalls(){
        foreach(BallBrick ball in BallManagerBrick.Instance.Balls)
            ball.Die();
    }
}