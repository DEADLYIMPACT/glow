using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPaddle : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = GameManagerPong.Instance.paddleColor;
    }

    private void FixedUpdate(){
        speed = BallManagerPong.Instance.initalBallSpeed / 100 - 1.5f;
        if(GameManagerPong.Instance.isGameEnded){
            rb.velocity = Vector2.zero;
            return;
        }
        Rigidbody2D ball = BallManagerPong.Instance.ballRb;
        if(ball.velocity.y > 0 && Mathf.Abs(ball.position.x - transform.position.x) > 0.25){
            if(ball.position.x > transform.position.x && transform.position.x < 1.65){
                rb.velocity = new Vector2(speed, 0);
            } else if(ball.position.x < transform.position.x && transform.position.x > -1.65){
                rb.velocity = new Vector2(-speed, 0);
            }
        } else{
            if(transform.position.x < 0){
                rb.AddForce(Vector2.right * 100)    ;
            } else if(transform.position.x > 0){
                rb.AddForce(Vector2.left * 100);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.tag == "Ball" && collider.gameObject.transform.position.y < transform.position.y){
            Rigidbody2D ballrb = collider.gameObject.GetComponent<Rigidbody2D>();
            Vector2 hitpoint = collider.contacts[0].point;
            Vector2 paddleCenter = transform.position;
            ballrb.velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
            float difference = paddleCenter.x - hitpoint.x;
            ballrb.AddForce(new Vector2(-(difference * 250), -BallManagerPong.Instance.currentBallSpeed));
            BallManagerPong.Instance.currentBallSpeed++;
        }
    }
}
