using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBrick : MonoBehaviour
{
    #region Singleton

    private static PaddleBrick paddle;

    public static PaddleBrick Instance => paddle;

    private void Awake(){
        if(paddle != null)
            Destroy(gameObject);
        else{
            paddle = this;
        }
    }

    #endregion
    SpriteRenderer sr;
    public bool isLengthDiff;
    private float initalClamp = 1.65f;
    private float currentClamp;
    private float initalScaleX;
    private float startLengthTime, endLengthTime;

    void Start()
    {  
        sr = GetComponent<SpriteRenderer>();
        sr.color = GameManagerBrick.Instance.paddleColor;
        currentClamp = initalClamp;
        initalScaleX = transform.localScale.x;
        isLengthDiff = false;
    }

    void Update()
    {   
        if(Input.GetMouseButton(0) && GameManagerBrick.Instance.isGameStarted && Camera.main.ScreenToWorldPoint(new Vector2(0, Input.mousePosition.y)).y < 3.5 && Time.timeScale == 1){
            float x = Input.mousePosition.x;
            float worldX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(new Vector2(x, 0)).x, -currentClamp, currentClamp);
            transform.position = new Vector2(worldX, transform.position.y);
        }
        if(startLengthTime < endLengthTime)
                startLengthTime += Time.deltaTime;
        else if(isLengthDiff)
            resetPaddle();
    }
    
    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.tag == "Ball" && collider.gameObject.transform.position.y > transform.position.y){
            Rigidbody2D ballrb = collider.gameObject.GetComponent<Rigidbody2D>();
            Vector2 hitpoint = collider.contacts[0].point;
            Vector2 paddleCenter = transform.position;
            ballrb.velocity = Vector2.zero;
            float difference = paddleCenter.x - hitpoint.x;
            ballrb.AddForce(new Vector2(-(difference * 250), BallManagerBrick.Instance.currentBallSpeed));
        }
    }

    public void paddleLong(){
        transform.localScale = new Vector2(initalScaleX * 1.5f, transform.localScale.y);
        currentClamp = 1.25f;
        isLengthDiff = true;
        startLengthTime = 0;
        endLengthTime = 15;
    }

    public void paddleShort(){
        transform.localScale = new Vector2(initalScaleX * 0.75f, transform.localScale.y);
        currentClamp = 1.85f;
        isLengthDiff = true;
        startLengthTime = 0;
        endLengthTime = 15;
    }

    private void resetPaddle(){
        transform.localScale = new Vector2(initalScaleX, transform.localScale.y);
        currentClamp = initalClamp;
        isLengthDiff = false;
    }
}
