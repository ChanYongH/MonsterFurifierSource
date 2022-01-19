//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class ItemManager : MonoBehaviour
//{
//    UIManager uiManager;

//    //Item
//    GameObject itemUseCanvas; // UI 처리를 위해서 가져와야 함
//    GameObject itemUseList;
//    List<Button> itemSelect = new List<Button>();
//    List<GameObject> itemObj = new List<GameObject>();
//    List<Items> items = new List<Items>();
//    List<Button> useItem = new List<Button>();
//    //Queue<GameObject> itemSequence = new Queue<GameObject>();
//    public List<GameObject> itemSequence = new List<GameObject>();
//    public int itemIndex = 0; // 아이템 수

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

//        captureButton.Add(captureUI[0].GetComponent<Button>()); // 메세지(누구를 잡았다)
//        captureButton.Add(captureUI[1].GetComponent<Button>()); // 몬스터 정보

//        captureButton.Add(captureUI[2].transform.GetChild(0).GetComponent<Button>()); // 이름 바꿀래용
//        captureButton.Add(captureUI[2].transform.GetChild(1).GetComponent<Button>()); // 안 바꿀래용

//        captureButton[0].onClick.AddListener(CaptureMonsterShowInfo);
//        captureButton[1].onClick.AddListener(AskChangeName);
//        captureButton[2].onClick.AddListener(() => AskChangeName2(true));
//        captureButton[3].onClick.AddListener(() => AskChangeName2(false));
//        nameChange.onClick.AddListener(ChangeName);
//    }

//    //--------------------------------------아이템 사용 부분---------------------------------------------
//    #region ItemUse

//    public void ItemUse()
//    {
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["Start"]].SetActive(false);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["OnBattle"]].SetActive(false);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["ItemUseStart"]].SetActive(false);
//        itemUseCanvas.SetActive(true);
//        // 아이템을 아무것도 가지고 있지 않으면 표시하지 않게 함 
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
//        // 보유한 갯수 표시
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
//        itemSequence[0].SetActive(true); // 첫 번째 아이템 활성화
//    }

//    public void ItemSelect(bool left)
//    {
//        if (!left) // 오른쪽
//        {
//            if (itemSequence.Count > itemIndex + 1) // 아이템을 보유한 갯수가 현재 선택된 아이템 index보다 크면 실행
//            {
//                itemSequence[itemIndex].SetActive(false); // 누른 후 전에 보였던 아이템 비활성화
//                itemIndex++;
//                itemSequence[itemIndex].SetActive(true); // 누른 후 현재 아이템 활성화
//            }
//            else // 아이템을 보유한 갯수가 현재 선택된 아이템 index보다 작으면 0으로 만들어 준다. 
//            {
//                itemSequence[itemIndex].SetActive(false);
//                itemIndex = 0;
//                itemSequence[itemIndex].SetActive(true);
//            }
//        }
//        else
//        {
//            if (itemIndex != 0) // 현재 선택된 아이템이 0이아니면 index를 뺀다
//            {
//                itemSequence[itemIndex].SetActive(false);
//                itemIndex--;
//                itemSequence[itemIndex].SetActive(true);
//            }
//            else // 0이면 List의 맨 뒤에 있는 index를 가져온다
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
//                uiManager.itemInven.itemCount[index]--; // 아이템 인벤토리에서 아이템을 제거 해준다.
//                itemUseCanvas.SetActive(false);
//            }
//            else
//                StartCoroutine(BattleOnlyMessage("몬스터를 약화 시켜주세요!"));
//        }
//        else
//        {
//            items[index].UseItem(uiManager.playerMonster);
//            StartCoroutine(UseItemCo(index)); // 아이템 UI처리
//            uiManager.itemInven.itemCount[index]--; // 아이템 인벤토리에서 아이템을 제거 해준다.
//            itemUseCanvas.SetActive(false);
//        }
//    }
//    IEnumerator UseItemCo(int index) // 이런걸 배틀로 빼야되나?
//    {
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["BattleProgress"]].SetActive(true);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "이 " + items[index].itemName + "을 사용했다!";
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "은 " + items[index].useItemText;
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        uiManager.battleManager.battleMessages[uiManager.battleManager.massageDic["BattleProgress"]].SetActive(false);
//        uiManager.battleManager.BattleStart();
//    }

//    IEnumerator BattleOnlyMessage(string message)
//    {
//        captureUI[0].transform.parent.gameObject.SetActive(true);
//        captureUI[0].SetActive(true); // UI가 최하단에 있어서 모든 캔버스를 덮어씌울 수 있기때문에 사용
//        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text = message;
//        yield return new WaitForSecondsRealtime(2);
//        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text = null;
//        captureUI[0].SetActive(false);
//        captureUI[0].transform.parent.gameObject.SetActive(false);

//    }

//    public IEnumerator CaptureCo() // n초 뒤에 다시 전투 화면으로 돌아감
//    {
//        //for (int i = 0; i < sequnceCanvas.GetChild(0).childCount; i++) //
//        //{
//        //    sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
//        //    sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
//        //}
//        uiManager.playerInWorld.captureProgress = true; // 포획 하는 도중엔 총알을 바꾸지 못하게
//        for (int i = 0; i < uiManager.playerInWorld.bullets[3].maxCount; i++) // 남은 총알 갯수
//            uiManager.enemyMonster.hormone.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
//        // 타이머
//        int timer = (int)items[0].itemDuration; // 아이템의 지속시간을 가져옴
//        while (true)
//        {
//            uiManager.enemyMonster.hormone.transform.GetComponentInChildren<TextMeshProUGUI>().text =
//            "남은 시간 : " + timer;
//            yield return new WaitForSeconds(1);
//            timer--; // 1초에 한 번씩 깎임 
//            if (timer == 0) // text상에서 숫자가 0이 되면 해당하는 텍스트를 출력
//                uiManager.enemyMonster.hormone.transform.GetComponentInChildren<TextMeshProUGUI>().text =
//            "남은 시간 : 시간초과!";
//            if (timer < 0 || captureState) // 시간이 다되면 빠져나감or호르몬을 맞추면 빠져나감
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
//        uiManager.enemyMonster.monsterName + "을 포획했다!";

//    }

//    void CaptureMonsterShowInfo()
//    {

//        captureUI[0].SetActive(false);
//        captureUI[1].SetActive(true);
//        captureMonsterInfo[0].text = "몬스터 이름 : " + uiManager.enemyMonster.monsterName;
//        captureMonsterInfo[1].text = "속성 : " + "불";
//        captureMonsterInfo[2].text = "체력        : " + uiManager.enemyMonster.maxHp +
//        System.Environment.NewLine + "공격력      : " + uiManager.enemyMonster.att +
//        System.Environment.NewLine + "방어력      : " + uiManager.enemyMonster.def +
//        System.Environment.NewLine + "특수 공격력 : " + uiManager.enemyMonster.spAtt +
//        System.Environment.NewLine + "특수 방어력 : " + uiManager.enemyMonster.spDef;
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
//        "몬스터의 이름을 변경 하시겠습니까?";

//    }

//    void AskChangeName2(bool yeah)
//    {
//        captureUI[2].transform.GetChild(0).gameObject.SetActive(false);
//        captureUI[2].transform.GetChild(1).gameObject.SetActive(false);
//        if (yeah) // 이름을 바꾼다
//        {
//            captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
//            "몬스터의 이름을 정해주세요!";
//            captureUI[2].transform.GetChild(3).gameObject.SetActive(true);

//        }
//        else // 이름을 바꾸지 않는다
//        {
//            captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
//            "몬스터의 이름을 변경하지 않습니다.";
//            StartCoroutine(CaptureCanvasOffCo(uiManager.flowTime * 2)); // 포획 관련 UI가 모두 꺼지고 전투 나가기가 활성화 된다. 
//        }
//    }

//    public void ChangeName() // 버튼 클릭
//    {
//        captureUI[2].transform.GetChild(3).gameObject.SetActive(false);
//        uiManager.enemyMonster.monsterName = nameInput.text; // InputField에 입력한 값을 몬스터 이름으로 바꿔준다.
//        captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
//        "몬스터의 이름을 " + nameInput.text + "로 바꿨다!";
//        uiManager.enemyMonster.monsterNameUI.text = uiManager.enemyMonster.monsterName; // 몬스터 아이콘의 text도 바꿔준다.
//        nameInput.text = null;

//        StartCoroutine(CaptureCanvasOffCo(uiManager.flowTime * 2)); // 포획 관련 UI가 모두 꺼지고 전투 나가기가 활성화 된다. 
//    }

//    IEnumerator CaptureCanvasOffCo(float time)
//    {
//        //Debug.Log("왜 3번 출력?");
//        uiManager.monsterInven.GetMonster(uiManager.enemyMonster); // 몬스터를 몬스터 인벤토리에 추가 해줌
//        yield return new WaitForSecondsRealtime(time);
//        captureCanvas.SetActive(false);
//        for (int i = 0; i < captureUI.Count; i++)
//            captureUI[i].SetActive(false);
//        // 전투 결과로 넘어간다.(포획에 성공하면 경험치 획득, 돈 획득, 레벨업이나 스킬을 얻을 수 있음)
//        uiManager.battleManager.BattleResult(uiManager.enemyMonster.exp, uiManager.enemyMonster.money, true);
//    }

//    #endregion
//}
