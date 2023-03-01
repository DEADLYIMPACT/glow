using System;   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour {
    private int hitPoints;
    private SpriteRenderer sr;
    [SerializeField] public ParticleSystem destroyEffectParticle;
    [SerializeField] public PowerupBrick powerUpPrefab;
    
    private void Start(){
        sr = GetComponent<SpriteRenderer>();
        hitPoints = 3;
    }

    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.tag == "Ball"){
            BallBrick ball = collider.gameObject.GetComponent<BallBrick>();
            if(sr.color == GameManagerBrick.Instance.color3)
                return;
            if(ColorUtility.ToHtmlStringRGB(ball.GetComponent<SpriteRenderer>().color) == ColorUtility.ToHtmlStringRGB(sr.color))
                hitPoints = 0;
            else
                hitPoints--;
            if(hitPoints == 0){
                destroyBrick();
            } else{
                sr.sprite = BrickManager.Instance.Sprites[hitPoints - 1];
            }
        }
    }

    public void destroyBrick(){
        BrickManager.Instance.bricks.Remove(this);
        destroyEffect();
        if(BrickManager.Instance.bricks.Count == 0){
            BrickManager.Instance.resetPowerups();
            BallManagerBrick.Instance.resetBalls();
            FindObjectOfType<CanvasManagerBrick>().playWinSound();
            BrickManager.Instance.Invoke("levelComplete", 1);
        } else {
            if(UnityEngine.Random.Range(0, 3) == 0){
                PowerupBrick pwb = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
                BrickManager.Instance.powerupList.Add(pwb);
                pwb.GetComponent<SpriteRenderer>().color = GameManagerBrick.Instance.paddleColor;
            }
            if(GameModeSelect.doSoundEffects)
                GetComponent<AudioSource>().Play();
        }
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke("destroy", 1);
    }

    void destroy(){
        Destroy(gameObject);
    }

    private void destroyEffect(){
        ParticleSystem effect = Instantiate(destroyEffectParticle, transform.position, Quaternion.identity);
        MainModule mm  = effect.main;
        mm.startColor = sr.color;
        Destroy(effect, destroyEffectParticle.main.startLifetime.constant);
    }
}
