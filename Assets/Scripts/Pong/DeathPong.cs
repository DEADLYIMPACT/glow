using UnityEngine;

public class DeathPong : MonoBehaviour
{
    [SerializeField] CanvasManagerPong canvasManagerPong;
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Ball"){
            BallPong ball = collider.GetComponent<BallPong>();
            GameManagerPong.Instance.isGameEnded = true;
            ball.Die();
            if(BrickManagerPong.Instance.powerup)
                BrickManagerPong.Instance.powerup.die();
            if(GameModeSelect.doVibrate)
                Handheld.Vibrate();
            Invoke("gameOver", 1);
        }
    }   

    private void gameOver(){      
        bool isHS = false;  
        if(GameModeSelect.gameModeCompletedLevels[GameModeSelect.gameMode] < GameManagerPong.Instance.score){
            GameModeSelect.gameModeCompletedLevels[GameModeSelect.gameMode] = GameManagerPong.Instance.score;
            PlayerPrefs.SetInt(GameModeSelect.gameMode, GameManagerPong.Instance.score);
            isHS = true;
        }
        canvasManagerPong.gameOver(isHS);   
    }
}
