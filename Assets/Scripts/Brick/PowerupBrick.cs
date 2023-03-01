using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PowerupBrick : MonoBehaviour {   
    [SerializeField] float speed;
    [SerializeField] public List<Sprite> sprites;
    [SerializeField] public ParticleSystem destroyEffectParticle;
    public enum Powerup {
        Multiple,
        Slow,
        Fast,
        Short,
        Long,
        Money
    }   
    public Powerup powerup;
    private void Start(){
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int random;
        random = Random.Range(0, 11);
        switch(random){
            case 0:
            case 1:
                powerup = Powerup.Multiple;
                sr.sprite = sprites[0];
                break;
            case 2:
                powerup = Powerup.Slow;
                sr.sprite = sprites[1];
                break;
            case 3:
            case 4:
                powerup = Powerup.Fast;
                sr.sprite = sprites[2];
                break;
            case 5:
                powerup = Powerup.Short;
                sr.sprite = sprites[3];
                break;
            case 6:
            case 7:
                powerup = Powerup.Long;
                sr.sprite = sprites[4];
                break;
            case 8:
            case 9:
            case 10:
                powerup = Powerup.Money;
                sr.sprite = sprites[5];
                break;
        }        
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Paddle"){
            CanvasManagerBrick canvasManagerBrick = FindObjectOfType<CanvasManagerBrick>();
            switch (powerup)
            {
                case Powerup.Multiple:
                    BallManagerBrick.Instance.multiple();
                    break;
                case Powerup.Slow:
                    BallManagerBrick.Instance.slow();
                    break;
                case Powerup.Fast:
                    BallManagerBrick.Instance.fast();
                    break;
                case Powerup.Short:
                    PaddleBrick.Instance.paddleShort();
                    break;
                case Powerup.Long:
                    PaddleBrick.Instance.paddleLong();
                    break;
            }
            canvasManagerBrick.playPowerupSound();
            GameManagerBrick.Instance.score += 5;
            canvasManagerBrick.scoreText.text = GameManagerBrick.Instance.score + "";
        }
        if(collider.gameObject.tag == "Paddle" || collider.gameObject.tag == "Death")
            die();
    }

    private bool isAldreadySpawned(int i){
        foreach(PowerupBrick pwp in BrickManager.Instance.powerupList){
            if((int) pwp.powerup == i && pwp != this)
                return true;
        }
        return false;
    }

    public void die(){
        BrickManager.Instance.powerupList.Remove(this);
        ParticleSystem effect = Instantiate(destroyEffectParticle, transform.position, Quaternion.identity);
        MainModule mm  = effect.main;
        mm.startColor = GetComponent<SpriteRenderer>().color;
        Destroy(effect, destroyEffectParticle.main.startLifetime.constant);
        Destroy(gameObject);
    }
}
