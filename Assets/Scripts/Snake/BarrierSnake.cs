using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSnake : MonoBehaviour{
    SnakeManager sm;
    
    void Start(){
        sm = FindObjectOfType<SnakeManager>();
        GetComponent<SpriteRenderer>().color = GameManagerSnake.paddleColor;
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag != "Ball")
            Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.GetComponent<BarrierSnake>() && collision.gameObject.transform.position.x != transform.position.x){
            int random = Random.Range(0, 2);
            if(random == 0)
                Destroy(gameObject);
            else
                Destroy(collision.gameObject);
        }
    }

    void Update(){
        if(!GameManagerSnake.isGameStarted)
            return;
        if(!BlockManagerSnake.isHitting){
            transform.Translate(Vector2.down * sm.curSpeed * Time.deltaTime);
        }
        if (transform.position.y < -14)
            Destroy(gameObject);
    }
}