using UnityEngine;

public class BallManagerPong : MonoBehaviour
{
    #region Singleton
    
    private static BallManagerPong ballManager;

    public static BallManagerPong Instance => ballManager;

    private void Awake(){
        if(ballManager != null)
            Destroy(gameObject);
        else{
            ballManager = this;
        }
    }

    #endregion
    
    [SerializeField] private BallPong ballPrefab;
    private BallPong ball;
    public Rigidbody2D ballRb;
    public float initalBallSpeed = 250; 
    public float currentBallSpeed;
    public bool isSpeedDiff;
    private float startSpeedTime, endSpeedTime;
    
    private void Start(){
        InitBall();
        currentBallSpeed = initalBallSpeed;
        isSpeedDiff = false;
    }

    private void Update(){
        if(!GameManagerPong.Instance.isGameStarted){
            ball.transform.position = Vector2.zero;
            if(Input.GetMouseButton(0) && Camera.main.ScreenToWorldPoint(new Vector2(0, Input.mousePosition.y)).y < 3.5 &&  Time.timeScale != 0 && !GameManagerPong.Instance.isShowingInst){
                GameManagerPong.Instance.isGameStarted = true;
                startBall();
                BrickManagerPong.Instance.spawnRandomBrick(5);
                FindObjectOfType<CanvasManagerPong>().instructions.gameObject.SetActive(false);
            }
        }
        if(startSpeedTime < endSpeedTime)
            startSpeedTime += Time.deltaTime;
        else if(isSpeedDiff)
            resetSpeed();
    }

    private void startBall(){
        ballRb.isKinematic = false;
        ballRb.AddForce(new Vector2(Random.Range(-150, 150), -initalBallSpeed));
    }

    public void InitBall(){
        Vector2 paddlePosition = PaddlePong.Instance.gameObject.transform.position;
        Vector2 startingPosition = Vector2.zero;
        ball = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        ballRb = ball.GetComponent<Rigidbody2D>();
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
        sr.sprite = GameModeSelect.Instance.skins[GameModeSelect.Instance.selectedBall];
        sr.color = GameManagerPong.Instance.color1;
    }
    
    public void fast(){
        ballRb.velocity *= 2 * initalBallSpeed / currentBallSpeed;
        currentBallSpeed = initalBallSpeed * 2;
        isSpeedDiff = true;
        startSpeedTime = 0;
        endSpeedTime = 15;
    }

    private void resetSpeed(){
        isSpeedDiff = false;
        ballRb.velocity *= initalBallSpeed / currentBallSpeed;
        currentBallSpeed = initalBallSpeed;
    }

    public void reset(){
        ball.Die();
        InitBall();
        resetSpeed();
        Invoke("startBall", 0.5f);
        initalBallSpeed += 10;
    }
}
