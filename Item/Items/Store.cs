using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Store : MonoBehaviour
{
    GameObject store;
    public GameObject decisionCanvas;
    public TextMeshProUGUI dialog;
    public GameObject buyButton = null;

    //������
    GameObject itemListObj;
    public List<int> itemListCount = new List<int>();
    public List<TextMeshProUGUI> listCount = new List<TextMeshProUGUI>();

    //�÷��̾�
    public Transform player;
    public PlayerWorld playerInWorld;
    public TextMeshProUGUI playerMoney;
    public PlayerUIManager playerUI;

    public ItemInventory itemInven;
    int totalPrice = 0;
    private void Start()
    {
        playerUI = FindObjectOfType<PlayerUIManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        store = GameObject.FindGameObjectWithTag("Store").transform.GetChild(0).gameObject;
        itemListObj = transform.GetChild(0).GetComponentInChildren<GridLayoutGroup>().gameObject;
        //���� ������ getchild�� �����;� ��
        itemInven = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetComponentInChildren<ItemInventory>();
        playerInWorld = player.GetChild(0).GetComponent<PlayerWorld>();
        dialog = decisionCanvas.GetComponentInChildren<TextMeshProUGUI>();
        for (int i = 0; i < itemListObj.transform.childCount; i++)
        {
            itemListCount.Add(0);
            listCount.Add(itemListObj.transform.GetChild(i).GetChild(5).GetComponentInChildren<TextMeshProUGUI>());
            listCount[i].text = itemListCount[i].ToString();
        }

    }

    public void PurchaseDecision(bool select) // ���� ��ư
    {
        decisionCanvas.SetActive(select);
        for (int i = 0; i < decisionCanvas.transform.childCount; i++)
            decisionCanvas.transform.GetChild(i).gameObject.SetActive(select);
        if (select)
        {
            buyButton = EventSystem.current.currentSelectedGameObject;
            buyButton.SetActive(false);
            int saleCount = 0;
            // �� ����
            for (int i = 0; i < itemListCount.Count; i++)
                saleCount += itemListCount[i];
            if (saleCount <= 0) // �߰�(���� �� �� ���� ��)
            {
                dialog.text = "�����Ͻ� ��ǰ�� ���� ���ּ���!";
                StartCoroutine(SaleCanvasCo()); // ���� ���� �� ���� ���
                return;
            }
            for (int i = 0; i < itemListCount.Count; i++)
            {
                if (itemListCount[i] > 0) // ���� üũ
                {
                    for (int j = 0; j < itemListCount[i]; j++)
                        // ���� ������ ��ǰ�� ���� ��ŭ �����ش�. 
                        totalPrice += itemListObj.transform.GetChild(i).GetComponent<Items>().itemPrice;
                }
            }
            //decisionCanvas.SetActive(select);
            dialog.text = "�� ������ " + totalPrice + "$ �Դϴ�. ���� ���� �Ͻðڽ��ϱ� ?";
            //for (int i = 0; i < decisionCanvas.transform.childCount; i++)
            //    decisionCanvas.transform.GetChild(i).gameObject.SetActive(select);
        }
        else // �ƴϿ� ����
        {
            buyButton.SetActive(true);
            for (int i = 0; i < itemListCount.Count; i++)
            {
                totalPrice = 0;
                itemListCount[i] = 0;
                listCount[i].text = itemListCount[i].ToString();
            }
        }
    }

    public void PurchaseVolumeAdd(int index) // ���ŷ�
    {
        itemListCount[index]++;
        if (itemListCount[index] >= 9)
            itemListCount[index] = 9;
        listCount[index].text = itemListCount[index].ToString();
    }
    public void PurchaseVolumeMinus(int index) // ���ŷ�
    {
        itemListCount[index]--;
        if (itemListCount[index] <= 0)
            itemListCount[index] = 0;
        listCount[index].text = itemListCount[index].ToString();
    }

    public void SaleItem() // ���� ������ ���� �Լ��� ��� ��
    {
        
        for (int i = 0; i < itemListCount.Count; i++) 
        {
            if (itemListCount[i] > 0)
                // ���� �߰� �Ȱ� ������ �ϴ� �������� �߰� ���ش�.
                itemInven.AddItem(itemInven.haveItem[i].itemNumber, itemListCount[i]);
        }
        // �÷��̾ ���� �� �� üũ
        if (playerInWorld.money >= totalPrice) 
        {
            playerInWorld.Money -= totalPrice;
            dialog.text = "���� ����!";
            playerMoney.text = playerInWorld.money.ToString() + "G";
        }
        else
        {
            for (int i = 0; i < itemListCount.Count; i++) // ���� �����ϸ� �߰��� �������� ���� ����
                itemInven.itemCount[i] -= itemListCount[i];
            dialog.text = "���� �� ���� �����ϴ�.";
        }
        StartCoroutine(SaleCanvasCo()); // ���� ���� �� ���� �޼��� ���
        for (int i = 0; i < itemListCount.Count; i++)
        {
            itemListCount[i] = 0; // ���� �ʱ�ȭ
            listCount[i].text = itemListCount[i].ToString(); // ���� �޼��� ��� �ʱ�ȭ
        }
        totalPrice = 0; // ���� ���� �ʱ�ȭ
    }
    IEnumerator SaleCanvasCo()
    {
        decisionCanvas.SetActive(true);
        decisionCanvas.transform.GetChild(0).gameObject.SetActive(true);
        decisionCanvas.transform.GetChild(1).gameObject.SetActive(false);
        decisionCanvas.transform.GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        decisionCanvas.SetActive(false);
        decisionCanvas.transform.GetChild(0).gameObject.SetActive(false);
        buyButton.SetActive(true);
        dialog.text = "���� ���� �Ͻðڽ��ϱ� ?";
    }
    public void StoreExit()
    {
        for (int i = 0; i < itemListCount.Count; i++)
        {
            itemListCount[i] = 0; // ���� �ʱ�ȭ
            listCount[i].text = itemListCount[i].ToString(); // ���� �޼��� ��� �ʱ�ȭ
        }
        store.SetActive(false);
        decisionCanvas.SetActive(false);
        playerInWorld.cameraRotateSpeed = 4;
        playerInWorld.speed = 1;
    }
}
