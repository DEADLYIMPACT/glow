using UnityEngine;

public class SnakePart : MonoBehaviour{
    SnakeManager snakeManager;
    public bool isColliding;
    public Transform collidingWith;

    void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.GetComponent<BarrierSnake>() || collider.gameObject.tag == "Wall"){
            if(transform == snakeManager.bodyParts[0] && Mathf.Abs(transform.position.x - collider.gameObject.transform.position.x) < 0.1f && transform.position.y < collider.gameObject.transform.position.y){
                BlockManagerSnake.isHitting = true;
                BlockManagerSnake.hittingWith = collider.gameObject;
            }
            isColliding = true;
            collidingWith = collider.gameObject.transform;
        }
    }

    void OnCollisionStay2D(Collision2D collider){
        if(collider.gameObject.GetComponent<BarrierSnake>() ){
            isColliding = true;
            collidingWith = collider.gameObject.transform;
        }
    }

    void OnCollisionExit2D(Collision2D collider){
        if(collider.gameObject.GetComponent<BarrierSnake>() || collider.gameObject.tag == "Wall"){
            isColliding = false;
            if(transform == snakeManager.bodyParts[0] && transform.position.y < collider.gameObject.transform.position.y){
                BlockManagerSnake.isHitting = false;
            }
        }
    }

    void Start(){
        snakeManager = transform.GetComponentInParent<SnakeManager>();
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Food"){
            Food food = collision.gameObject.GetComponent<Food>();
            if(transform == snakeManager.bodyParts[0]){
                for(int i = 0; i < food.foodAmt; i++){
                    snakeManager.AddBodyPart(food.color);
                    FindObjectOfType<BlockManagerSnake>().foods.Remove(food.transform);
                }
                Destroy(food.gameObject);
                snakeManager.updateText();
            }
        }
        if(collision.gameObject.tag == "Block" && snakeManager.bodyParts[0] == transform){
            BlockManagerSnake.isHitting = true;
            BlockManagerSnake.hittingWith = collision.gameObject;
            BlockManagerSnake.checkStopHitting = true;
            snakeManager.curSpeed = 0;
            transform.position = new Vector2(transform.position.x, collision.gameObject.transform.position.y - 1.25f);
        }
    }

    void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.tag == "Block" && snakeManager.bodyParts[0] == transform){
            BlockManagerSnake.isHitting = true;
            BlockManagerSnake.checkStopHitting = true;
            BlockManagerSnake.checkStopHitting = true;
            BlockManagerSnake.hittingWith = collision.gameObject;
            snakeManager.curSpeed = 0;
            transform.position = new Vector2(transform.position.x, collision.gameObject.transform.position.y - 1.25f);
        }
    }

    void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.GetComponent<BlockSnake>() && snakeManager.bodyParts[0] == transform && BlockManagerSnake.hittingWith == collision.gameObject){
            BlockManagerSnake.isHitting = false;
        }
    }
}
