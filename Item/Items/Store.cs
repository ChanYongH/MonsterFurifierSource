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

    //아이템
    GameObject itemListObj;
    public List<int> itemListCount = new List<int>();
    public List<TextMeshProUGUI> listCount = new List<TextMeshProUGUI>();

    //플레이어
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
        //꺼져 있으면 getchild로 가져와야 함
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

    public void PurchaseDecision(bool select) // 구매 버튼
    {
        decisionCanvas.SetActive(select);
        for (int i = 0; i < decisionCanvas.transform.childCount; i++)
            decisionCanvas.transform.GetChild(i).gameObject.SetActive(select);
        if (select)
        {
            buyButton = EventSystem.current.currentSelectedGameObject;
            buyButton.SetActive(false);
            int saleCount = 0;
            // 예 선택
            for (int i = 0; i < itemListCount.Count; i++)
                saleCount += itemListCount[i];
            if (saleCount <= 0) // 추가(선택 한 게 없을 때)
            {
                dialog.text = "구매하실 물품을 선택 해주세요!";
                StartCoroutine(SaleCanvasCo()); // 정말 구매 할 건지 물어봄
                return;
            }
            for (int i = 0; i < itemListCount.Count; i++)
            {
                if (itemListCount[i] > 0) // 수량 체크
                {
                    for (int j = 0; j < itemListCount[i]; j++)
                        // 최종 가격을 물품의 수량 만큼 더해준다. 
                        totalPrice += itemListObj.transform.GetChild(i).GetComponent<Items>().itemPrice;
                }
            }
            //decisionCanvas.SetActive(select);
            dialog.text = "총 가격은 " + totalPrice + "$ 입니다. 정말 구매 하시겠습니까 ?";
            //for (int i = 0; i < decisionCanvas.transform.childCount; i++)
            //    decisionCanvas.transform.GetChild(i).gameObject.SetActive(select);
        }
        else // 아니오 선택
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

    public void PurchaseVolumeAdd(int index) // 구매량
    {
        itemListCount[index]++;
        if (itemListCount[index] >= 9)
            itemListCount[index] = 9;
        listCount[index].text = itemListCount[index].ToString();
    }
    public void PurchaseVolumeMinus(int index) // 구매량
    {
        itemListCount[index]--;
        if (itemListCount[index] <= 0)
            itemListCount[index] = 0;
        listCount[index].text = itemListCount[index].ToString();
    }

    public void SaleItem() // 예를 누르면 이쪽 함수로 들어 옴
    {
        
        for (int i = 0; i < itemListCount.Count; i++) 
        {
            if (itemListCount[i] > 0)
                // 만약 추가 된게 있으면 일단 아이템을 추가 해준다.
                itemInven.AddItem(itemInven.haveItem[i].itemNumber, itemListCount[i]);
        }
        // 플레이어가 보유 한 돈 체크
        if (playerInWorld.money >= totalPrice) 
        {
            playerInWorld.Money -= totalPrice;
            dialog.text = "구매 성공!";
            playerMoney.text = playerInWorld.money.ToString() + "G";
        }
        else
        {
            for (int i = 0; i < itemListCount.Count; i++) // 돈이 부족하면 추가된 아이템을 제거 해줌
                itemInven.itemCount[i] -= itemListCount[i];
            dialog.text = "구매 할 돈이 없습니다.";
        }
        StartCoroutine(SaleCanvasCo()); // 구매 성공 및 실패 메세지 출력
        for (int i = 0; i < itemListCount.Count; i++)
        {
            itemListCount[i] = 0; // 수량 초기화
            listCount[i].text = itemListCount[i].ToString(); // 수량 메세지 출력 초기화
        }
        totalPrice = 0; // 최종 가격 초기화
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
        dialog.text = "정말 구매 하시겠습니까 ?";
    }
    public void StoreExit()
    {
        for (int i = 0; i < itemListCount.Count; i++)
        {
            itemListCount[i] = 0; // 수량 초기화
            listCount[i].text = itemListCount[i].ToString(); // 수량 메세지 출력 초기화
        }
        store.SetActive(false);
        decisionCanvas.SetActive(false);
        playerInWorld.cameraRotateSpeed = 4;
        playerInWorld.speed = 1;
    }
}
