using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PowerupPong : MonoBehaviour
{   
    [SerializeField] float speed;
    [SerializeField] public List<Sprite> sprites;
    [SerializeField] public ParticleSystem destroyEffectParticle;
    public enum Powerup {
        Fast,
        Short,
        Long,
    }   
    public Powerup powerup;
    private void Start(){
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int random;
        random = Random.Range(0, 5);
        switch(random){
            case 0:
            case 1:
                powerup = Powerup.Fast;
                sr.sprite = sprites[2];
                break;
            case 2:
                powerup = Powerup.Short;
                sr.sprite = sprites[3];
                break;
            case 3:
            case 4:
                powerup = Powerup.Long;
                sr.sprite = sprites[4];
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Paddle"){
            switch (powerup)
            {
                case Powerup.Fast:
                    BallManagerPong.Instance.fast();
                    break;
                case Powerup.Short:
                    PaddlePong.Instance.paddleShort();
                    break;
                case Powerup.Long:
                    PaddlePong.Instance.paddleLong();
                    break;
            }
            CanvasManagerPong cmp = FindObjectOfType<CanvasManagerPong>();
            cmp.playPowerupSound();
            cmp.scoreText.text = ++GameManagerPong.Instance.score + "";
        }
        if(collider.gameObject.tag == "Paddle" || collider.gameObject.tag == "Death"){
            die();
        }
    }

    public void die(){
        ParticleSystem effect = Instantiate(destroyEffectParticle, transform.position, Quaternion.identity);
        MainModule mm  = effect.main;
        mm.startColor = GetComponent<SpriteRenderer>().color;
        Destroy(effect, destroyEffectParticle.main.startLifetime.constant);
        Destroy(gameObject);
    }
}
