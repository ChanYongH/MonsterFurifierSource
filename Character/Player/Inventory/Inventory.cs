using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    GameObject itemInventory;
    GameObject monsterInventory;
    Image itemButton;
    Image monsterButton;
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        itemInventory = transform.GetChild(0).GetChild(0).gameObject;
        monsterInventory = transform.GetChild(0).GetChild(1).gameObject;
        itemButton = transform.GetChild(0).GetChild(2).GetComponent<Image>();
        monsterButton = transform.GetChild(0).GetChild(3).GetComponent<Image>();
    }
    public void ShowInven(bool item) // 인벤토리의 전반적인 내용
    {
        if (item) // 아이템 인벤토리 활성화 
        {
            monsterInventory.SetActive(false); 
            itemInventory.SetActive(true);
            itemButton.color = Color.gray;
            monsterButton.color = Color.white;
        }
        else // 몬스터 인벤토리 활성화
        {
            itemInventory.SetActive(false);
            monsterInventory.SetActive(true);
            itemButton.color = Color.white;
            monsterButton.color = Color.gray;
        }
    }
}
