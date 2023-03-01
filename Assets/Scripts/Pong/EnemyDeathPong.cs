using System;
using UnityEngine;

public class EnemyDeathPong : MonoBehaviour
{
    [SerializeField] CanvasManagerPong canvasManagerPong;
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Ball"){
            BallPong ball = collider.GetComponent<BallPong>();
            BallManagerPong.Instance.reset();
            GameManagerPong.Instance.score += 5;
            canvasManagerPong.scoreText.text = Convert.ToString(GameManagerPong.Instance.score);
            canvasManagerPong.playWinSound();
        }
    }   
}
