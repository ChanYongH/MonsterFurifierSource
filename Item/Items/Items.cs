using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public GameObject itemInfoObj;
    public string itemInfo;
    public string itemName;
    public string useItemText;
    public int itemPrice;
    public int itemNumber;
    public ItemInventory itemInven;
    public bool inBattle; // 배틀 상태면 다른 형태로 보여줘야 하기때문에 bool값을 줌
    RectTransform rectSize;
    public float itemDuration = 0;

    public List<TextMeshProUGUI> itemTexts = new List<TextMeshProUGUI>();
    
    // Start is called before the first frame update
    void Start()
    {
        itemInfoObj = GameObject.Find("ItemInfo"); 
        itemTexts.Add(transform.GetChild(0).GetComponent<TextMeshProUGUI>()); // 아이템 이름
        //itemTexts.Add(itemInfoObj.GetComponentInChildren<TextMeshProUGUI>()); // 아이템 정보
        rectSize = GetComponent<RectTransform>();

        itemTexts[0].text = itemName;
        
        if(inBattle)
        {
            itemTexts.Add(transform.GetChild(3).GetComponent<TextMeshProUGUI>());
            itemTexts[1].text = itemInfo;
            rectSize.anchoredPosition = new Vector2(78.5f, 42f);
            rectSize.localScale = new Vector3(0.1f, 0.25f, 0.16f);
        }
        else if (itemPrice <= 0)
        {
            itemTexts.Add(itemInfoObj.GetComponentInChildren<TextMeshProUGUI>());
            itemTexts[1].text = null;
            itemTexts.Add(transform.GetChild(2).GetComponent<TextMeshProUGUI>()); // 가지고 있는 갯수
            itemInven = transform.parent.GetComponentInParent<ItemInventory>();
            //itemTexts[2].text = inven.itemCount[itemNumber].ToString();
     
            //itemTexts[2].text = itemPrice.ToString() + "$";
            Debug.Log("0보다 낮군");
        }
        else
        {
            itemTexts.Add(itemInfoObj.GetComponentInChildren<TextMeshProUGUI>());
            itemTexts[1].text = null;
            itemTexts.Add(transform.GetChild(1).GetComponent<TextMeshProUGUI>()); // 아이템 가격
            itemTexts[2].text = itemPrice.ToString() + "$";
            //itemTexts.Add(transform.GetChild(2).GetComponent<TextMeshProUGUI>());
        }
    }

    public void OnMouseOver()
    {
        itemTexts[1].text = itemInfo;
    }
    public void OnMouseExit()
    {
        itemTexts[1].text = null;
    }

    public virtual void UseItem(Monster pMonster, Monster eMonster = null, UIManager uIManager = null, PlayerWorld playerInWorld = null)
    {
        
    }

    public virtual void UseItemGeneric<T, U, V>(T classObj, U classObj2, V classObj3) where T : class where U : class where V : class
    {

    }
}
