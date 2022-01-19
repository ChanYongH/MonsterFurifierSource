//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class ItemManager : MonoBehaviour
//{
//    UIManager uiManager;

//    //Item
//    GameObject itemUseCanvas; // UI ó���� ���ؼ� �����;� ��
//    GameObject itemUseList;
//    List<Button> itemSelect = new List<Button>();
//    List<GameObject> itemObj = new List<GameObject>();
//    List<Items> items = new List<Items>();
//    List<Button> useItem = new List<Button>();
//    //Queue<GameObject> itemSequence = new Queue<GameObject>();
//    public List<GameObject> itemSequence = new List<GameObject>();
//    public int itemIndex = 0; // ������ ��

//    //Capture
//    GameObject captureCanvas;
//    List<GameObject> captureUI = new List<GameObject>();
//    List<TextMeshProUGUI> captureMonsterInfo = new List<TextMeshProUGUI>();
//    List<TextMeshProUGUI> captureMonsterSkills = new List<TextMeshProUGUI>();
//    List<Button> captureButton = new List<Button>();
//    //NameChange
//    public TMP_InputField nameInput;
//    public Button nameChange;



//    public bool captureState = false;
//    public bool captureProgress = false;
//    // Start is called before the first frame update
//    void Start()
//    {
//        uiManager = FindObjectOfType<UIManager>();
//        PlayerWorld.Gobattle += ItemEventStart;
//    }

//    void ItemEventStart()
//    {
//        #region ItemUse
//        itemUseCanvas = GameObject.FindGameObjectWithTag("ItemUseCanvas").transform.GetChild(0).gameObject;
//        uiManager.itemInven = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetComponentInChildren<ItemInventory>();
//        itemSelect.Add(itemUseCanvas.transform.GetChild(1).GetComponent<Button>());
//        itemSelect.Add(itemUseCanvas.transform.GetChild(2).GetComponent<Button>());
//        itemUseList = itemUseCanvas.transform.GetChild(3).gameObject;
//        for (int i = 0; i < itemUseList.transform.childCount; i++)
//        {
//            itemObj.Add(itemUseList.transform.GetChild(i).gameObject);
//            useItem.Add(itemObj[i].GetComponent<Button>());
//            items.Add(itemObj[i].GetComponentInChildren<Items>());
//        }
//        itemSelect[0].onClick.AddListener(() => ItemSelect(true));
//        itemSelect[1].onClick.AddListener(() => ItemSelect(false));
//        useItem[0].onClick.AddListener(() => UseItemUI(0));
//        useItem[1].onClick.AddListener(() => UseItemUI(1));
//        useItem[2].onClick.AddListener(() => UseItemUI(2));
//        useItem[3].onClick.AddListener(() => UseItemUI(3));
//        #endregion

//        captureCanvas = GameObject.FindGameObjectWithTag("CaptureSucceeded").transform.GetChild(0).gameObject;
//        for (int i = 0; i < captureCanvas.transform.childCount; i++)
//            captureUI.Add(captureCanvas.transform.GetChild(i).gameObject);

//        for (int i = 0; i < captureUI[1].transform.childCount - 1; i++)
//            captureMonsterInfo.Add(captureUI[1].transform.GetChild(i).GetComponent<TextMeshProUGUI>());

//        for (int i = 0; i < captureUI[1].transform.GetChild(captureUI[1].transform.childCount - 1).transform.childCount; i++)
//            captureMonsterSkills.Add(captureUI[1].transform.GetChild(captureUI[1].transform.childCount - 1).transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());

//        nameInput = captureUI[2].transform.GetChild(3).GetComponent<TMP_InputField>();
//        nameChange = nameInput.transform.GetChild(1).GetComponent<Button>();

//        captureButton.Add(captureUI[0].GetComponent<Button>()); // �޼���(������ ��Ҵ�)
//        captureButton.Add(captureUI[1].GetComponent<Button>()); // ���� ����

//        captureButton.Add(captureUI[2].transform.GetChild(0).GetComponent<Button>()); // �̸� �ٲܷ���
//        captureButton.Add(captureUI[2].transform.GetChild(1).GetComponent<Button>()); // �� �ٲܷ���

//        captureButton[0].onClick.AddListener(CaptureMonsterShowInfo);
//        captureButton[1].onClick.AddListener(AskChangeName);
//        captureButton[2].onClick.AddListener(() => AskChangeName2(true));
//        captureButton[3].onClick.AddListener(() => AskChangeName2(false));
//        nameChange.onClick.AddListener(ChangeName);
//    }

//    //--------------------------------------������ ��� �κ�---------------------------------------------
//    #region ItemUse

//    public void ItemUse()
//    {
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["Start"]].SetActive(false);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["OnBattle"]].SetActive(false);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["ItemUseStart"]].SetActive(false);
//        itemUseCanvas.SetActive(true);
//        // �������� �ƹ��͵� ������ ���� ������ ǥ������ �ʰ� �� 
//        int itemNull = 0;
//        for (int i = 0; i < uiManager.itemInven.itemCount.Count; i++)
//            itemNull += uiManager.itemInven.itemCount[i];
//        if (itemNull == 0)
//        {
//            itemUseList.SetActive(false);
//            itemUseCanvas.transform.GetChild(4).gameObject.SetActive(true);
//            return;
//        }
//        else
//        {
//            itemUseCanvas.transform.GetChild(4).gameObject.SetActive(false);
//            itemUseList.SetActive(true);
//        }
//        // ������ ���� ǥ��
//        itemIndex = 0;
//        if (itemSequence.Count > 0)
//            itemSequence.Clear();
//        for (int i = 0; i < itemUseList.transform.childCount; i++)
//            itemObj[i].SetActive(false);


//        for (int i = 0; i < itemUseList.transform.childCount; i++)
//        {
//            if (uiManager.itemInven.itemCount[i] > 0)
//            {
//                itemObj[i].transform.GetChild(0).GetChild(2).GetComponentInChildren<TextMeshProUGUI>()
//                        .text = uiManager.itemInven.itemCount[i].ToString();
//                itemSequence.Add(itemObj[i]);
//            }
//        }
//        itemSequence[0].SetActive(true); // ù ��° ������ Ȱ��ȭ
//    }

//    public void ItemSelect(bool left)
//    {
//        if (!left) // ������
//        {
//            if (itemSequence.Count > itemIndex + 1) // �������� ������ ������ ���� ���õ� ������ index���� ũ�� ����
//            {
//                itemSequence[itemIndex].SetActive(false); // ���� �� ���� ������ ������ ��Ȱ��ȭ
//                itemIndex++;
//                itemSequence[itemIndex].SetActive(true); // ���� �� ���� ������ Ȱ��ȭ
//            }
//            else // �������� ������ ������ ���� ���õ� ������ index���� ������ 0���� ����� �ش�. 
//            {
//                itemSequence[itemIndex].SetActive(false);
//                itemIndex = 0;
//                itemSequence[itemIndex].SetActive(true);
//            }
//        }
//        else
//        {
//            if (itemIndex != 0) // ���� ���õ� �������� 0�̾ƴϸ� index�� ����
//            {
//                itemSequence[itemIndex].SetActive(false);
//                itemIndex--;
//                itemSequence[itemIndex].SetActive(true);
//            }
//            else // 0�̸� List�� �� �ڿ� �ִ� index�� �����´�
//            {
//                itemSequence[itemIndex].SetActive(false);
//                itemIndex = itemSequence.Count - 1;
//                itemSequence[itemIndex].SetActive(true);
//            }

//        }
//    }

//    public void UseItemUI(int index)
//    {
//        if (index == 0)
//        {
//            if (uiManager.enemyMonster.Hp <= (uiManager.enemyMonster.MaxHp / 2))
//            {
//                items[index].UseItem(uiManager.playerMonster, uiManager.enemyMonster, this, uiManager.playerInWorld);
//                uiManager.enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
//                StartCoroutine(CaptureCo());
//                uiManager.itemInven.itemCount[index]--; // ������ �κ��丮���� �������� ���� ���ش�.
//                itemUseCanvas.SetActive(false);
//            }
//            else
//                StartCoroutine(BattleOnlyMessage("���͸� ��ȭ �����ּ���!"));
//        }
//        else
//        {
//            items[index].UseItem(uiManager.playerMonster);
//            StartCoroutine(UseItemCo(index)); // ������ UIó��
//            uiManager.itemInven.itemCount[index]--; // ������ �κ��丮���� �������� ���� ���ش�.
//            itemUseCanvas.SetActive(false);
//        }
//    }
//    IEnumerator UseItemCo(int index) // �̷��� ��Ʋ�� ���ߵǳ�?
//    {
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["BattleProgress"]].SetActive(true);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "�� " + items[index].itemName + "�� ����ߴ�!";
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "�� " + items[index].useItemText;
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["BattleProgress"]].SetActive(false);
//        uiManager.battleManager.BattleStart();
//    }

//    IEnumerator BattleOnlyMessage(string message)
//    {
//        captureUI[0].transform.parent.gameObject.SetActive(true);
//        captureUI[0].SetActive(true); // UI�� ���ϴܿ� �־ ��� ĵ������ ����� �� �ֱ⶧���� ���
//        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text = message;
//        yield return new WaitForSecondsRealtime(2);
//        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text = null;
//        captureUI[0].SetActive(false);
//        captureUI[0].transform.parent.gameObject.SetActive(false);

//    }

//    public IEnumerator CaptureCo() // n�� �ڿ� �ٽ� ���� ȭ������ ���ư�
//    {
//        //for (int i = 0; i < sequnceCanvas.GetChild(0).childCount; i++) //
//        //{
//        //    sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
//        //    sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
//        //}
//        uiManager.playerInWorld.captureProgress = true; // ��ȹ �ϴ� ���߿� �Ѿ��� �ٲ��� ���ϰ�
//        for (int i = 0; i < uiManager.playerInWorld.bullets[3].maxCount; i++) // ���� �Ѿ� ����
//            uiManager.enemyMonster.hormone.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
//        // Ÿ�̸�
//        int timer = (int)items[0].itemDuration; // �������� ���ӽð��� ������
//        while (true)
//        {
//            uiManager.enemyMonster.hormone.transform.GetComponentInChildren<TextMeshProUGUI>().text =
//            "���� �ð� : " + timer;
//            yield return new WaitForSeconds(1);
//            timer--; // 1�ʿ� �� ���� ���� 
//            if (timer == 0) // text�󿡼� ���ڰ� 0�� �Ǹ� �ش��ϴ� �ؽ�Ʈ�� ���
//                uiManager.enemyMonster.hormone.transform.GetComponentInChildren<TextMeshProUGUI>().text =
//            "���� �ð� : �ð��ʰ�!";
//            if (timer < 0 || captureState) // �ð��� �ٵǸ� ��������orȣ������ ���߸� ��������
//                break;
//        }
//        PlayerWorld.Gobattle.Invoke();
//        uiManager.enemyMonster.hormone.SetActive(false);
//        uiManager.playerInWorld.captureProgress = false;
//    }

//    public void CaputreSuccess()
//    {
//        for (int i = 0; i < uiManager.battleManager.sequnceCanvas.GetChild(0).childCount; i++)
//        {
//            uiManager.battleManager.sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
//            uiManager.battleManager.sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
//        }
//        //enemyMonster.isDead = true;
//        //enemyMonster.playerMonster = true;
//        //enemyMonster.tag = "PlayerMonster";
//        uiManager.enemyMonster.gameObject.SetActive(false);
//        //battleManager.enemyMonsterUI.SetActive(false);
//        captureCanvas.SetActive(true);
//        captureUI[0].SetActive(true);
//        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text =
//        uiManager.enemyMonster.monsterName + "�� ��ȹ�ߴ�!";

//    }

//    void CaptureMonsterShowInfo()
//    {

//        captureUI[0].SetActive(false);
//        captureUI[1].SetActive(true);
//        captureMonsterInfo[0].text = "���� �̸� : " + uiManager.enemyMonster.monsterName;
//        captureMonsterInfo[1].text = "�Ӽ� : " + "��";
//        captureMonsterInfo[2].text = "ü��        : " + uiManager.enemyMonster.maxHp +
//        System.Environment.NewLine + "���ݷ�      : " + uiManager.enemyMonster.att +
//        System.Environment.NewLine + "����      : " + uiManager.enemyMonster.def +
//        System.Environment.NewLine + "Ư�� ���ݷ� : " + uiManager.enemyMonster.spAtt +
//        System.Environment.NewLine + "Ư�� ���� : " + uiManager.enemyMonster.spDef;
//        for (int i = 0; i < captureMonsterSkills.Count; i++)
//            captureMonsterSkills[i].text = uiManager.enemyMonster.skill.nameToKorean[uiManager.enemyMonster.equipSkill[i]];
//    }

//    void AskChangeName()
//    {
//        captureUI[1].SetActive(false);
//        captureUI[2].SetActive(true);
//        captureUI[2].transform.GetChild(0).gameObject.SetActive(true);
//        captureUI[2].transform.GetChild(1).gameObject.SetActive(true);
//        captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
//        "������ �̸��� ���� �Ͻðڽ��ϱ�?";

//    }

//    void AskChangeName2(bool yeah)
//    {
//        captureUI[2].transform.GetChild(0).gameObject.SetActive(false);
//        captureUI[2].transform.GetChild(1).gameObject.SetActive(false);
//        if (yeah) // �̸��� �ٲ۴�
//        {
//            captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
//            "������ �̸��� �����ּ���!";
//            captureUI[2].transform.GetChild(3).gameObject.SetActive(true);

//        }
//        else // �̸��� �ٲ��� �ʴ´�
//        {
//            captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
//            "������ �̸��� �������� �ʽ��ϴ�.";
//            StartCoroutine(CaptureCanvasOffCo(uiManager.flowTime * 2)); // ��ȹ ���� UI�� ��� ������ ���� �����Ⱑ Ȱ��ȭ �ȴ�. 
//        }
//    }

//    public void ChangeName() // ��ư Ŭ��
//    {
//        captureUI[2].transform.GetChild(3).gameObject.SetActive(false);
//        uiManager.enemyMonster.monsterName = nameInput.text; // InputField�� �Է��� ���� ���� �̸����� �ٲ��ش�.
//        captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
//        "������ �̸��� " + nameInput.text + "�� �ٲ��!";
//        uiManager.enemyMonster.monsterNameUI.text = uiManager.enemyMonster.monsterName; // ���� �������� text�� �ٲ��ش�.
//        nameInput.text = null;

//        StartCoroutine(CaptureCanvasOffCo(uiManager.flowTime * 2)); // ��ȹ ���� UI�� ��� ������ ���� �����Ⱑ Ȱ��ȭ �ȴ�. 
//    }

//    IEnumerator CaptureCanvasOffCo(float time)
//    {
//        //Debug.Log("�� 3�� ���?");
//        uiManager.monsterInven.GetMonster(uiManager.enemyMonster); // ���͸� ���� �κ��丮�� �߰� ����
//        yield return new WaitForSecondsRealtime(time);
//        captureCanvas.SetActive(false);
//        for (int i = 0; i < captureUI.Count; i++)
//            captureUI[i].SetActive(false);
//        // ���� ����� �Ѿ��.(��ȹ�� �����ϸ� ����ġ ȹ��, �� ȹ��, �������̳� ��ų�� ���� �� ����)
//        uiManager.battleManager.BattleResult(uiManager.enemyMonster.exp, uiManager.enemyMonster.money, true);
//    }

//    #endregion
//}
