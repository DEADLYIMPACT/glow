using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   

public class Level : MonoBehaviour
{
    public void startLevel(){
        int lvl = Int32.Parse(transform.GetChild(0).GetComponent<Text>().text);
        LevelSelect.level = lvl;
        StartCoroutine(LevelSelect.Instance.fadeOut(LevelSelect.Instance.fade, 0.3f, GameModeSelect.gameMode));
    }
}
