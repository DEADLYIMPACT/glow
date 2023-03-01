using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class Shop : MonoBehaviour {
    public int n;
    Image skin, self;
    Text text;

    private void Awake(){
        skin = transform.GetChild(0).GetComponent<Image>();
        text = transform.GetComponentInChildren<Text>();
        self = GetComponent<Image>();
    }

    public void clicked(){
        if(ShopManager.Instance.isOwned[n]){
            ShopManager.Instance.selectBall(n);
            self.color = ShopManager.Instance.selectedColor;
            GetComponent<Button>().interactable = false;
        }
        else {
            ShopManager.Instance.buySkin(n);
            skin.transform.localPosition = Vector2.zero;
            text.gameObject.SetActive(false);
            self.color = ShopManager.Instance.selectedColor;
            GetComponent<Button>().interactable = false;
        }
    }

    public void fixSkin(){
        skin.sprite = GameModeSelect.Instance.skins[n];
        if(ShopManager.Instance.isOwned[n]){
            skin.transform.localPosition = Vector2.zero;
            text.gameObject.SetActive(false);
            if(GameModeSelect.Instance.selectedBall == n){
                self.color = ShopManager.Instance.selectedColor;
                GetComponent<Button>().interactable = false;
            }
            else
                self.color = ShopManager.Instance.ownedColor;
            
        } else {
            int price = ShopManager.Instance.prices[n];
            transform.GetComponentInChildren<Text>().text = price + "";
            if(price <= GameModeSelect.coins)
                self.color = ShopManager.Instance.canBuyColor;
            else{
                self.color = ShopManager.Instance.cantBuyColor;
                GetComponent<Button>().interactable = false;
            }
        }
    }
}
