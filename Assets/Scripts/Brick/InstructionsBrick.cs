using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsBrick : MonoBehaviour {
    public Image background;
    public RectTransform back1, back2, border;

    IEnumerator fadeIn(Image myImage, RectTransform back, RectTransform border, float duration){
        float counter = 0;
        Color spriteColor = myImage.color;
        back1.gameObject.SetActive(true);
        while (counter < duration){
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 0.75f, counter / duration);
            myImage.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            float size = Mathf.Lerp(0, 1, counter / duration);
            back.localScale = new Vector2(size, size);
            border.localScale = new Vector2(size, size);
            yield return null;
        }
        back2.gameObject.SetActive(true);
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
        GameManagerBrick.Instance.isShowingInst = false;
    }

    public void nextPage(){
        if(back1.gameObject.activeInHierarchy){
            back1.gameObject.SetActive(false);
            back2.localScale = new Vector2(1, 1);
        }
        else
            StartCoroutine(fadeOut(background, back2, border, 0.3f));
    }

    public void showInst(){
        back2.gameObject.SetActive(false);
        StartCoroutine(fadeIn(background, back1, border, 0.3f));
    }
}
