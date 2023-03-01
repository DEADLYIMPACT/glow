using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour{
    SnakeManager sm;
    BlockManagerSnake bms;
    public int foodAmt;
    public int color;

    void Awake(){
        sm = FindObjectOfType<SnakeManager>();
        bms = FindObjectOfType<BlockManagerSnake>();
    }

    void Start(){
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        color = Random.Range(0, 2);
        sr.color = color == 0 ? GameManagerSnake.color1 : GameManagerSnake.color2;
        sr.sprite = GameModeSelect.Instance.skins[GameModeSelect.Instance.selectedBall];
        foodAmt = Random.Range(0, 4) == 0 ? Random.Range(4, 6) : Random.Range(1, 4);
        transform.GetChild(0).GetComponent<TextMesh>().text = foodAmt + "";
    }
    
    void Update(){
        if(!GameManagerSnake.isGameStarted)
            return;
        if(!BlockManagerSnake.isHitting)
            transform.Translate(Vector2.down * sm.curSpeed * Time.deltaTime);
        if (transform.position.y < -14){
            bms.foods.Remove(gameObject.transform);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.GetComponent<Food>() && collider != null){
            bms.foods.Remove(gameObject.transform);
            Destroy(gameObject);
        }
    }
}
