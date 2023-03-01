using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    #region Singleton
    
    private static Music music;
    public static Music Instance => music;
    public List<AudioClip> musics;
    public AudioSource audioSource;
    int playingNo = -1;

    private void Awake(){
        if(music != null){
            Destroy(gameObject);
            return;
        }
        else{
            music = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    void Start(){
        audioSource = GetComponent<AudioSource>();
        if(GameModeSelect.playMusic)
            play();
    }

    public void play(){
        if(playingNo == -1){
            playingNo = Random.Range(0, musics.Count);
        }
        else {
            int i;
            do i = Random.Range(0, musics.Count);
            while(i == playingNo);
            playingNo = i;
        }
        audioSource.PlayOneShot(musics[playingNo], 1);
    }

    void Update(){
        if(!audioSource.isPlaying && GameModeSelect.playMusic){
            play();
        }
    }
}
