using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManagerSnake : MonoBehaviour{
    public SnakeManager snakeManager;
    public static bool isHitting = false;
    public static bool checkStopHitting = false;
    public static bool check = false;
    public static GameObject hittingWith;
    public float distBetweenBlocks;
    public GameObject boxPrefab;
    private List<Transform[]> blockBarriers = new List<Transform[]>();
    public GameObject foodPrefab;
    public List<Transform> foods = new List<Transform>();
    float diff = 6.57f;
    float nextSpawnPos;

    void Start(){
        nextSpawnPos = 16.64f;
        spawnBarrier();
        isHitting = false;
        check = false;
        checkStopHitting = false;
    }

    void spawnBarrier(){
        float dist = (Camera.main.orthographicSize * Screen.width / Screen.height) / 5;
        int num = UnityEngine.Random.Range(7, 8);
        Transform[] blockBarrier = new Transform[5];
        int j = UnityEngine.Random.Range(-2, 3);
        for(int i = -2; i < 3; i++){
            float x = 2 * i * dist;
            float y = -1 + dist * 2 + nextSpawnPos;
            BlockSnake bs = Instantiate(boxPrefab, new Vector2(x, y), Quaternion.identity, transform).GetComponent<BlockSnake>();
            if(i == j)
                bs.isSpecialHealth = true;
            blockBarrier[i + 2] = bs.transform;
        }
        blockBarriers.Add(blockBarrier);
        for(int i = 1; i <= num; i++)
            spawnBlock(-1 + dist * 2 + nextSpawnPos + diff * i);
        nextSpawnPos = -9.52f + num * 6.535f;
    }

    void spawnBlock(float loneSpawnPos){
        float dist = (Camera.main.orthographicSize * Screen.width / Screen.height) / 5;
        int[] poss = new int[2];
        for(int i = 0; i < UnityEngine.Random.Range(1, 3); i++){
            int pos = 69;
            do pos = UnityEngine.Random.Range(-2, 3);
            while(Array.IndexOf(poss, pos) != -1);
            poss[i] = pos;
            float x = 2 * pos * dist;
            Instantiate(boxPrefab, new Vector2(x, loneSpawnPos), Quaternion.identity, transform);
        }
        if(UnityEngine.Random.Range(0, 3) == 0){
            int pos;
            do pos = UnityEngine.Random.Range(-2, 3);
            while(Array.IndexOf(poss, pos) != -1);
            float x = 2 * pos * dist;
            foods.Add((Instantiate(foodPrefab, new Vector2(x, loneSpawnPos), Quaternion.identity) as GameObject).transform);
        }
    }

    void Update(){
        if(GameManagerSnake.isGameEnded)
            return;
        if(allNull(blockBarriers[blockBarriers.Count - 1])){
            int i = blockBarriers.Count - 1;
            spawnBarrier();
            blockBarriers.Remove(blockBarriers[i]);
            snakeManager.speed = snakeManager.speed + 0.75f; //Mathf.Clamp(snakeManager.speed  + 0.75f, 0, 18.5f);
        }
        if(check){
            if(!checkStopHitting)
                isHitting = false;
            check = false;
        }
    }

    bool allNull(Transform[] transforms){
        bool isNull = true;
        foreach(Transform t in transforms){
            if(t != null)
                isNull = false;
        }
        return isNull;
    }
}
