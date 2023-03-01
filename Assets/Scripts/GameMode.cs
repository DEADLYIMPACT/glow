using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class GameMode : MonoBehaviour
{
    public string mode;
    public void selectGameMode(){
        GameModeSelect.gameMode = mode;
        string toLoad;
        if(mode == "Pong" || mode == "Snake")
            toLoad = mode;
        else 
            toLoad = "LevelSelect";
        StartCoroutine(GameModeSelect.Instance.fadeOut(GameModeSelect.Instance.fade, 0.3f, toLoad));
    }
}
