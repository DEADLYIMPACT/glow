using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsPong : MonoBehaviour {
    public Image background;
    public RectTransform back, border;

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
        GameManagerPong.Instance.isShowingInst = false;
    }

    public void exitInst(){
        StartCoroutine(fadeOut(background, back, border, 0.3f));
    }

    public void showInst(){
        StartCoroutine(fadeIn(background, back, border, 0.3f));
    }
}
