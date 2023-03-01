using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBrick : MonoBehaviour
{
    [SerializeField] CanvasManagerBrick canvasManagerBrick;
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Ball"){
            BallBrick ball = collider.GetComponent<BallBrick>();
            BallManagerBrick.Instance.Balls.Remove(ball);
            ball.Die();
            if(BallManagerBrick.Instance.Balls.Count == 0){
                BrickManager.Instance.resetPowerups();
                Invoke("gameOver", 1);
                if(GameModeSelect.doVibrate)
                    Handheld.Vibrate();
            }
        }
    }   

    private void gameOver(){        
        canvasManagerBrick.gameOver(false);
    }
}
