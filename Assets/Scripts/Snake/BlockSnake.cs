using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlockSnake : MonoBehaviour {
    SnakeManager sm;
    BlockManagerSnake bm;
    public int health;
    public List<GameObject> barriers;
    public bool isSpecialHealth = false;
    public bool isLoneBox = false; 
    public ParticleSystem blockParticle;

    void Start(){
        sm = FindObjectOfType<SnakeManager>();
        bm = FindObjectOfType<BlockManagerSnake>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Random.Range(0, 2) == 0 ? GameManagerSnake.color1 : GameManagerSnake.color2;
        if(isSpecialHealth)
            health = Random.Range(1, sm.sumOfSnake(sr.color) - 1);
        else
            health = Random.Range(1, sm.bodyParts.Count + 20);
        updateText();
        if(Random.Range(0, 3) == 0){ 
            int rand = Random.Range(0, 4) + 2;
            GameObject food = Instantiate(bm.foodPrefab, new Vector2(transform.position.x, barriers[rand].transform.position.y), Quaternion.identity);
            food.transform.parent = null;
        }
        for(int i = 0; i < 8; i++){
            int random = Random.Range(0, 5);
            if(random == 0 && Mathf.Abs(barriers[i].transform.position.x) < 4.2){
                barriers[i].transform.parent = null;
            }
            else
                Destroy(barriers[i]);
        }
        
    }

    public void updateText(){
        transform.GetComponentInChildren<TextMesh>().text = health + "";
    }

    void Update(){
        if(!GameManagerSnake.isGameStarted)
            return;
        if(!BlockManagerSnake.isHitting){
            transform.Translate(Vector2.down * sm.curSpeed * Time.deltaTime);
        }
        if (transform.position.y < -15){
            Destroy(gameObject);
        }
    }

    public void die(){
        ParticleSystem effect = Instantiate(blockParticle, transform.position, Quaternion.identity);
        MainModule mm  = effect.main;
        mm.startColor = GetComponent<SpriteRenderer>().color;
        effect.transform.localScale *= 1.33f;   
        Destroy(effect, blockParticle.main.startLifetime.constant);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Block" && !collision.gameObject.transform.IsChildOf(transform)){
            if(isLoneBox)
                Destroy(gameObject);
            else
                Destroy(collision.gameObject);
        }
    }
}
