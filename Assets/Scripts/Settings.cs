using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Settings : MonoBehaviour {
    public Image music, vibrate, sound;
    public List<Sprite> sprites;
    public Image background;
    public RectTransform back, border;
    
    public void setSettings(){
        music.sprite = GameModeSelect.playMusic ? sprites[1] : sprites[0];
        vibrate.sprite = GameModeSelect.doVibrate ? sprites[3] : sprites[2];
        sound.sprite = GameModeSelect.doSoundEffects ? sprites[5] : sprites[4];
    }

    public void toggleMusic(){
        GameModeSelect.playMusic = !GameModeSelect.playMusic;
        int i = GameModeSelect.playMusic ? 1 : 0;
        Music.Instance.audioSource.volume = i;
        music.sprite = sprites[i];
        PlayerPrefs.SetInt("music", i);
    } 
    
    public void toggleVibrate(){
        GameModeSelect.doVibrate = !GameModeSelect.doVibrate;
        int i = GameModeSelect.doVibrate ? 1 : 0;
        vibrate.sprite = sprites[i + 2];
        PlayerPrefs.SetInt("vibrate", i);
    }

    public void toggleSound(){
        GameModeSelect.doSoundEffects = !GameModeSelect.doSoundEffects;
        int i = GameModeSelect.doSoundEffects ? 1 : 0;
        sound.sprite = sprites[i + 4];
        PlayerPrefs.SetInt("sound", i);
    }
}
