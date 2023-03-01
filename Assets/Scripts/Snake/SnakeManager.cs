using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SnakeManager : MonoBehaviour
{
    public GameManagerSnake gameManagerSnake;
    public CanvasManagerSnake canvasManagerSnake;
    public List<Transform> bodyParts = new List<Transform>();
    public float minDistance;
    public int initalAmount;
    public float speed;
    public float curSpeed;
    public float rotationSpeed;
    public float LerpX;
    public GameObject bodyPartPrefab;
    private float distance;
    private float time;
    public float hitTime;
    private bool isFirstPart;
    Vector2 mousePreviousPos;
    Vector2 mouseCurrentPos;
    public TextMesh snakeLengthText;
    public ParticleSystem snakeParticle;
    
    void Start(){
        canvasManagerSnake = FindObjectOfType<CanvasManagerSnake>();
        isFirstPart = true;
        for(int i = 0; i < initalAmount; i++)
            AddBodyPart(0);
        snakeLengthText.text = bodyParts.Count + "";
    }

    void Update(){
        if(GameManagerSnake.isGameStarted && Time.deltaTime != 0){
            move();
            if(BlockManagerSnake.isHitting && BlockManagerSnake.hittingWith.GetComponent<BlockSnake>()){
                if(Input.GetMouseButton(0))
                    mousePreviousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if(time >= hitTime){
                    hit();
                    time = 0;
                }
                else 
                    time += Time.deltaTime;
            }
            else {
                curSpeed = speed;
                time = hitTime;
            }
        }   
    }

    void FixedUpdate(){
        if(GameManagerSnake.isGameStarted && Time.timeScale != 0){
            if(!BlockManagerSnake.isHitting)
                bodyParts[0].position = new Vector2(bodyParts[0].position.x, -1);
            else
                bodyParts[0].position = new Vector2(bodyParts[0].position.x, BlockManagerSnake.hittingWith.transform.position.y - 1.25f);
            for(int i = 1; i < bodyParts.Count; i++){
                Transform curBodyPart = bodyParts[i];
                Transform prevBodyPart = bodyParts[i - 1];
                distance = Vector2.Distance(prevBodyPart.position, curBodyPart.position);
                Vector2 prevPos = prevBodyPart.position;
                Vector2 pos = curBodyPart.position;
                SnakePart sp = curBodyPart.GetComponent<SnakePart>();
                bool s = true;
                if(sp.isColliding){
                    float sign = Mathf.Sign(prevPos.x - pos.x);
                    float signSP = Mathf.Sign(sp.collidingWith.position.x - sp.transform.position.x);
                    if(sign == signSP){
                        s = false;
                    }
                }
                
                if(s){
                    float thisLerp = LerpX;
                    if(i == 1)
                        thisLerp += 0.25f;
                    pos.x = Mathf.Lerp(pos.x, prevPos.x, thisLerp);
                }
                pos.y = prevPos.y - minDistance;
                curBodyPart.position = pos;
            }
        }
    }

    public void updateText(){
        snakeLengthText.text = bodyParts.Count + "";
    }

    void hit(){
        BlockSnake hittingWith =  BlockManagerSnake.hittingWith.GetComponent<BlockSnake>();
        hittingWith.health -= bodyParts[0].GetComponent<SpriteRenderer>().color == BlockManagerSnake.hittingWith.GetComponent<SpriteRenderer>().color ? 2 : 1;
        hittingWith.updateText();
        if(hittingWith.health <= 0){
            hittingWith.die();
            canvasManagerSnake.playBreakSound();
        }
        gameManagerSnake.score++;
        for(int i = 0; i < bodyParts.Count - 1; i++){
            bodyParts[i].gameObject.GetComponent<SpriteRenderer>().color = bodyParts[i + 1].gameObject.GetComponent<SpriteRenderer>().color;
        }
        bodyParts[0].position = new Vector2(bodyParts[0].position.x, BlockManagerSnake.hittingWith.transform.position.y - 1.25f);
        ParticleSystem effect = Instantiate(snakeParticle, bodyParts[0].position, new Quaternion(0, 0, 0, 0));
        MainModule mm  = effect.main;
        mm.startColor = bodyParts[0].GetComponent<SpriteRenderer>().color;
        Destroy(effect, snakeParticle.main.startLifetime.constant);
        Destroy(bodyParts[bodyParts.Count - 1].gameObject);
        bodyParts.Remove(bodyParts[bodyParts.Count - 1]);
        updateText();
        if (bodyParts.Count == 0){
            if(GameModeSelect.doVibrate)
                Handheld.Vibrate();
            GameManagerSnake.isGameEnded = true;
            GameManagerSnake.isGameStarted = false;
            BlockManagerSnake.isHitting = true;
            gameManagerSnake.Invoke("gameOver", 1);
        }
    }

    void move(){
        float maxX = Camera.main.orthographicSize * Screen.width / Screen.height;
        if(bodyParts[0].position.x > maxX - minDistance / 2)
            bodyParts[0].position = new Vector2(maxX - minDistance / 2 - 0.01f, bodyParts[0].position.y);
        else if(bodyParts[0].position.x < -maxX + minDistance / 2)
            bodyParts[0].position = new Vector2(-maxX + minDistance / 2 + 0.01f, bodyParts[0].position.y);
        if(Input.GetMouseButtonDown(0)){
            mousePreviousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if(Input.GetMouseButton(0)){
            if(mousePreviousPos.Equals(Vector2.zero)){
                mousePreviousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if(Mathf.Abs(bodyParts[0].position.x) < maxX){
                mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float deltaMousePos = Mathf.Abs(mousePreviousPos.x - mouseCurrentPos.x);
                float sign = Mathf.Sign(mousePreviousPos.x - mouseCurrentPos.x);
                bodyParts[0].GetComponent<Rigidbody2D>().AddForce(Vector2.right * rotationSpeed * 0.833f * deltaMousePos * -sign);
                mousePreviousPos = mouseCurrentPos;
            } else if(bodyParts[0].position.x > maxX - minDistance / 2)
                bodyParts[0].position = new Vector2(maxX - minDistance / 2 - 0.01f, bodyParts[0].position.y);
            else if(bodyParts[0].position.x < -maxX + minDistance / 2)
                bodyParts[0].position = new Vector2(-maxX + minDistance / 2 + 0.01f, bodyParts[0].position.y);
        }
    }

    public void AddBodyPart(int color){
        Transform newPart;
        if(isFirstPart){
            newPart = (Instantiate(bodyPartPrefab, new Vector2(0, -1), Quaternion.identity) as GameObject).transform;
            snakeLengthText.transform.parent = newPart;
            snakeLengthText.transform.position = newPart.position + new Vector3(0, 1, 0);
            snakeLengthText.color = GameManagerSnake.paddleColor;
            isFirstPart = false;
        }
        else{
            newPart = (Instantiate(bodyPartPrefab, new Vector2(bodyParts[bodyParts.Count - 1].position.x, bodyParts[bodyParts.Count - 1].position.y - minDistance), bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;
        }
        SpriteRenderer sr = newPart.GetComponent<SpriteRenderer>();
        sr.color = color == 0 ? GameManagerSnake.color1 : GameManagerSnake.color2;
        sr.sprite = GameModeSelect.Instance.skins[GameModeSelect.Instance.selectedBall];
        newPart.SetParent(transform);
        bodyParts.Add(newPart);    
    }

    public int sumOfSnake(Color color){
        int sum = 0;
        foreach(Transform bodyPart in bodyParts){
            if(color == bodyPart.GetComponent<SpriteRenderer>().color)
                sum += 2;
            else
                sum++;
        }
        return sum;
    }
}
