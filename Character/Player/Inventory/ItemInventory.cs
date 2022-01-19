using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInventory : Inventory
{
    public List<Items> haveItem = new List<Items>(); // ��� �������� �ʱ�ȭ
    public List<int> itemCount = new List<int>(); // ������ ������ ��
    List<GameObject> itemObj = new List<GameObject>(); // ������ ��ü
    public Transform items; // �������� ���� �־��ش�.
    Transform itemList; // �κ��丮 �ȿ� ������ ����Ʈ
    List<TextMeshProUGUI> itemAmount = new List<TextMeshProUGUI>(); // ������ ���� ���� textǥ��
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
    public void AddItem(int itemNum, int amount) // �������� �߰� ����(Store���� ���)
    {
        itemObj[itemNum].SetActive(true);
        itemCount[itemNum] += amount;
    }
    public void ShowItemList() // ������ ����Ʈ�� ���� ��(�������� ���� ���� ����)
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