using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BallBrick : MonoBehaviour
{
    Rigidbody2D rb;
    private float lastX = 0, lastY = 0;
    [SerializeField] public ParticleSystem destroyEffectParticle;


    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.tag == "Wall")
            GetComponent<SpriteRenderer>().color =  collider.gameObject.GetComponent<SpriteRenderer>().color;
        
    }

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject obj in otherObjects) {
            Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), GetComponent<Collider2D>()); 
        }
    }
    private void Update(){
        float x = transform.position.x;
        if(Math.Round(transform.position.x, 4) == Math.Round(lastX, 4) && transform.position.y != lastY){
            if(transform.position.x > 0)
                rb.AddForce(new Vector2(-50f, 0));
            else
                rb.AddForce(new Vector2(50f, 0));
        }   
        lastX = transform.position.x;
        lastY = transform.position.y;
    }

    void FixedUpdate(){
        transform.Rotate(0, 0, 2f);
    }

    public void Die(){
        ParticleSystem effect = Instantiate(destroyEffectParticle, transform.position, Quaternion.identity);
        MainModule mm  = effect.main;
        mm.startColor = GetComponent<SpriteRenderer>().color;
        Destroy(effect, destroyEffectParticle.main.startLifetime.constant);
        Destroy(gameObject);
    }
}
