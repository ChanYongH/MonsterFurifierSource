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
    public void ShowInven(bool item) // �κ��丮�� �������� ����
    {
        if (item) // ������ �κ��丮 Ȱ��ȭ 
        {
            monsterInventory.SetActive(false); 
            itemInventory.SetActive(true);
            itemButton.color = Color.gray;
            monsterButton.color = Color.white;
        }
        else // ���� �κ��丮 Ȱ��ȭ
        {
            itemInventory.SetActive(false);
            monsterInventory.SetActive(true);
            itemButton.color = Color.white;
            monsterButton.color = Color.gray;
        }
    }
}
