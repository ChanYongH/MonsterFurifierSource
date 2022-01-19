using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInventory : Inventory
{
    public List<Items> haveItem = new List<Items>(); // 모든 아이템을 초기화
    public List<int> itemCount = new List<int>(); // 소지한 아이템 수
    List<GameObject> itemObj = new List<GameObject>(); // 아이템 객체
    public Transform items; // 아이템을 직접 넣어준다.
    Transform itemList; // 인벤토리 안에 아이템 리스트
    List<TextMeshProUGUI> itemAmount = new List<TextMeshProUGUI>(); // 아이템 소지 갯수 text표시
    void Start()
    {
        itemList = transform.GetChild(1).transform;
        for (int i = 0; i < itemList.childCount; i++)
        {
            itemObj.Add(itemList.GetChild(i).gameObject);
            itemCount.Add(0);
            haveItem.Add(itemList.GetChild(i).GetComponent<Items>());
            itemAmount.Add(haveItem[i].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>());
        }
    }
    public void AddItem(int itemNum, int amount) // 아이템을 추가 해줌(Store에서 사용)
    {
        itemObj[itemNum].SetActive(true);
        itemCount[itemNum] += amount;
    }
    public void ShowItemList() // 아이템 리스트를 보여 줌(아이템을 열때 마다 실행)
    {
        for (int i = 0; i < haveItem.Count; i++)
        {
            if (itemCount[i] > 0)
                itemAmount[i].text = itemCount[i].ToString();
            else
                itemObj[i].SetActive(false);
        }
    }
}


//List<TextMeshProUGUI> itemName = new List<TextMeshProUGUI>();
//itemName.Add(itemList.GetComponentInChildren<TextMeshProUGUI>());