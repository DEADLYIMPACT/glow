using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BrickPong : MonoBehaviour
{
    private int hitPoints = 3;
    private SpriteRenderer sr;
    [SerializeField] public ParticleSystem destroyEffectParticle;
    [SerializeField] public PowerupPong powerUpPrefab;
    
    private void Start(){
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.tag == "Ball"){
            BallPong ball = collider.gameObject.GetComponent<BallPong>();
            ApplyCollisionLogic(ball);
        }
    }

    private void ApplyCollisionLogic(BallPong ball){
        if(ColorUtility.ToHtmlStringRGB(ball.GetComponent<SpriteRenderer>().color) == ColorUtility.ToHtmlStringRGB(sr.color))
            hitPoints = 0;
        else
            hitPoints--;
        if(hitPoints == 0){
            destroyEffect();
            PowerupPong pwb = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            BrickManagerPong.Instance.powerup = pwb;
            pwb.GetComponent<SpriteRenderer>().color = GameManagerPong.Instance.paddleColor;
            BrickManagerPong.Instance.spawnRandomBrick(Random.Range(3, 10));
            if(GameModeSelect.doSoundEffects)
                GetComponent<AudioSource>().Play();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            Invoke("destroy", 1);
        } else{
            sr.sprite = BrickManagerPong.Instance.Sprites[hitPoints - 1];
        }
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
