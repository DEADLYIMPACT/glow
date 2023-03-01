using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
    public Settings settings;
    public Stats stats;

    public void showSettings(){
        settings.gameObject.SetActive(true);
        settings.setSettings();
        if(GameModeSelect.doSoundEffects)
            GameModeSelect.Instance.audioSource.Play();
        StartCoroutine(fadeIn(settings.background, settings.back, settings.border, 0.3f));
    }
    
    public void exitSettings(){
        StartCoroutine(fadeOut(settings.background, settings.back, settings.border, 0.3f));
    }

    public void showStats(){
        stats.gameObject.SetActive(true);
        stats.setStats();
        if(GameModeSelect.doSoundEffects)
            GameModeSelect.Instance.audioSource.Play();
        StartCoroutine(fadeIn(stats.background, stats.back, stats.border, 0.3f));
    }

    public void exitStats(){
        StartCoroutine(fadeOut(stats.background, stats.back, stats.border, 0.3f));
    }

    public void rate(){
        Application.OpenURL ("market://details?id=com.DeadlyImpact.Glow");
    }

    IEnumerator fadeIn(Image myImage, RectTransform back, RectTransform border, float duration){
        float counter = 0;
        Color spriteColor = myImage.color;

        while (counter < duration){
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 0.75f, counter / duration);
            myImage.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            float size = Mathf.Lerp(0, 1, counter / duration);
            back.localScale = new Vector2(size, size);
            border.localScale = new Vector2(size, size);
            yield return null;
        }
    }

    IEnumerator fadeOut(Image myImage, RectTransform back, RectTransform border, float duration){
        float counter = 0;
        Color spriteColor = myImage.color;

        while (counter < duration){
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0.75f, 0, counter / duration);
            myImage.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            float size = Mathf.Lerp(1, 0, counter / duration);
            back.localScale = new Vector2(size, size);
            border.localScale = new Vector2(size, size);
            yield return null;
        }
        back.parent.gameObject.SetActive(false);
    }
}
