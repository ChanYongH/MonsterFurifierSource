using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPC
{
    GameObject store;
    Store storeClass;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        store = GameObject.FindGameObjectWithTag("Store").transform.GetChild(0).gameObject;
        storeClass = store.transform.parent.GetComponent<Store>();
    }
     
    public void GoShop()
    {
        count = 0;
        dialogCanvas.SetActive(false);
        store.SetActive(true);
        if(storeClass.buyButton != null)
            storeClass.buyButton.SetActive(true);
        storeClass.playerMoney.text = storeClass.playerInWorld.money.ToString() + "G";
        storeClass.playerUI.playerMoney.text = storeClass.playerInWorld.money.ToString() + "G";
    }

}
