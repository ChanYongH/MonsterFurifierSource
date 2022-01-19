using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MonsterInventory : Inventory
{
    List<Monster> monsters = new List<Monster>();
    Transform monsterList;
    List<GameObject> monsterObj = new List<GameObject>();
    public Monster clickMonster;
    //List<Button> monsterInfoButton = new List<Button>();
    //몬스터 정보
    GameObject monsterDetail;
    GameObject monsterSkills;
    GameObject skillView;
    public List<TextMeshProUGUI> detailText = new List<TextMeshProUGUI>(); // 1: 능력치 2: 이름 3: 속성
    public List<TextMeshProUGUI> skillText = new List<TextMeshProUGUI>();
    TextMeshProUGUI skillViewText;

    //장착 몬스터 변경
    public GameObject equipMonsterChange;
    GameObject[] equipMonsterObj = new GameObject[3];
    TextMeshProUGUI[] equipMonsterNames = new TextMeshProUGUI[3];
    public GameObject[] monsterChangeButton = new GameObject[3];
    bool changeEquipMonster = false;
    int equipMonsterNum;

    Button showInfoButton;
    
    UIManager uiManager;
    PlayerBattle playerInBattle;
    PlayerUIManager playerUI;
    AddScroll addScroll;
    // Start is called before the first frame update
    void Start()
    {
        addScroll = transform.GetChild(0).GetComponent<AddScroll>();
        playerUI = FindObjectOfType<PlayerUIManager>();
        playerInBattle = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetComponent<PlayerBattle>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        monsterList = transform.GetChild(0).GetComponentInChildren<GridLayoutGroup>().transform;
        monsterDetail = transform.parent.GetChild(4).gameObject;
        monsterSkills = monsterDetail.transform.GetChild(0).GetChild(6).gameObject; // 스킬
        skillView = monsterSkills.transform.GetChild(2).gameObject;
        skillViewText = skillView.GetComponentInChildren<TextMeshProUGUI>();
        equipMonsterChange = transform.parent.GetChild(5).gameObject;
        for (int i = 2; i < monsterDetail.transform.GetChild(0).childCount-2; i++)
            detailText.Add(monsterDetail.transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>());
        for (int i = 0; i < monsterSkills.transform.GetChild(1).childCount; i++)
            skillText.Add(monsterSkills.transform.GetChild(1).GetChild(i).GetComponentInChildren<TextMeshProUGUI>());
        for (int i = 5; i < equipMonsterChange.transform.GetChild(0).childCount - 1; i++)
            monsterChangeButton[i-5] = equipMonsterChange.transform.GetChild(0).GetChild(i).gameObject;

        for (int i = 0; i < equipMonsterNames.Length; i++)
        {
            equipMonsterObj[i] = equipMonsterChange.transform.GetChild(0).GetChild(i + 1).gameObject;
            equipMonsterNames[i] = equipMonsterObj[i].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        }

    }
    //---------------------------------------몬스터 포획 시 발생하는 함수들-----------------------------------------------
    #region GetMonster


    public void GetMonster(Monster monster)
    {
        monsters.Add(monster); // List<Monster>
        int lastNum = monsters.Count - 1; // List의 마지막int값을 캐싱 해줌
        monsters[lastNum].transform.parent.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(ShowMonsters);
        monsters[lastNum].tag = "PlayerMonster";
        monsters[lastNum].playerMonster = true;
        monsterObj.Add(monsters[lastNum].transform.parent.gameObject);
        monsterObj[lastNum].transform.SetParent(monsterList); //몬스터 객체 자체를 몬스터 인벤토리에 넣어준다.
        monsterObj[lastNum].SetActive(true);
        monsterObj[lastNum].transform.localScale = new Vector3(1, 1, 1);
        monsterObj[lastNum].transform.localPosition = new Vector3(0, 0, 0);
        addScroll.SetContentSize(); // 30개 이상이 되면 자동으로 스크롤이 생김
        if (playerInBattle.monsters != null)
        {
            if (playerInBattle.monsters.Count < 3) // 플레이어가 장착한 몬스터가 3개 미만이면 자동으로 추가 해준다.
                AutoEquipMonster(monsterObj[lastNum], monsters[lastNum]);
        }
    }
    public void AutoEquipMonster(GameObject monstersObj, Monster monster)
    {
        monstersObj.gameObject.SetActive(false); // 자동으로 장착이 되면 몬스터 객체의 부모(최상위 오브젝트)를 꺼준다.
        monster.transform.SetParent(playerInBattle.transform.parent.GetChild(1).GetChild(0)); // 몬스터 자동 장착
        monster.gameObject.SetActive(true); // 랜더러 텍스처를 이용해서 보여줄 수 있게 하려했다.
        playerInBattle.SetEquipMonster(); // for문으로 껐다가 켰다가 해주는 역할(갱신)
        if (monsters.Count > 0) // 몬스터가 추가 되면 레이어에 넣어준다.(렌더러 텍스처로 처리 하기 위해 해주는 작업) 
        {
            for (int i = 0; i < monsters.Count; i++)
                monsters[i].gameObject.layer = i + 6;
        }

        // 장착한 몬스터만 옮겨줌
        if (playerInBattle.monsters.Count > 0) // 몬스터가 추가 되자마자 지정한 위치로 옮겨서 몬스터가 레이어에 보이게 해놓는다.
        {
            for (int i = 0; i < playerUI.equipMonsters.Length; i++)
                playerUI.equipMonsters[i].gameObject.SetActive(false);
            for (int i = 0; i < playerInBattle.monsters.Count; i++)
            {
                playerInBattle.monsters[i].transform.position = new Vector3(1495.19995f, 6000.60986f, -88.6800003f);
                playerInBattle.monsters[i].transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
                playerInBattle.monsters[i].transform.localEulerAngles = new Vector3(0, 0, 0); 
                playerUI.equipMonsters[i].gameObject.SetActive(true);
                Debug.Log(playerInBattle.monsters[i].GetComponent<Monster>().GetType().Name);
                playerUI.monsterAttribute[i].color = uiManager.skillAttrib[playerInBattle.monsters[i].
                    GetComponent<Monster>().GetType().Name.Substring(0, 4)]; // 색 변경
                    
                    //GetType().Name.Substring(0, 4)];
            }
        }
    }
    #endregion

    public void ShowMonsters() // 몬스터 정보를 보거나 장착한 몬스터를 바꿔 줄 때 실행 함
    {
        // 클릭한 객체의 몬스터 클래스를 가져와서 정보를 가져옴
        GameObject monsterObject = EventSystem.current.currentSelectedGameObject.transform.parent.parent.gameObject; 
        clickMonster = monsterObject.transform.GetChild(1).GetComponent<Monster>();
        if(!changeEquipMonster)
        {
            #region MonsterShowInfo
            monsterDetail.SetActive(true);
            clickMonster.transform.GetChild(2).gameObject.SetActive(true); // 카메라
            Skills cmSkill = clickMonster.transform.GetChild(0).GetComponent<Skills>();
            detailText[0].text = "체력 : " + clickMonster.hp + System.Environment.NewLine +
                                 "공격력 : " + clickMonster.att + System.Environment.NewLine +
                                 "방어력 : " + clickMonster.def + System.Environment.NewLine +
                                 "특수 공격력 : " + clickMonster.spAtt + System.Environment.NewLine +
                                 "특수 방어력 : " + clickMonster.spDef;
            detailText[1].text = clickMonster.monsterName;
            detailText[2].text = "불";
            detailText[3].text = clickMonster.monsterExplain;
            clickMonster.gameObject.SetActive(true);
            for (int i = 0; i < clickMonster.equipSkill.Count; i++)
                skillText[i].text = cmSkill.nameToKorean[clickMonster.equipSkill[i]];
            #endregion
        }
        else // 장착한 몬스터를 바꿔 줄 때 실행
        {
            #region ChangeEquipMonsterButton
            monsterObject.layer = 3;
            
            for (int i = 0; i < playerInBattle.monsters.Count; i++)
                monsterChangeButton[i].SetActive(false);
            changeEquipMonster = false;
            playerInBattle.monsters.RemoveAt(equipMonsterNum);
            playerInBattle.monsters.Insert(equipMonsterNum, clickMonster.gameObject);
            Monster equipMonster = playerInBattle.transform.GetChild(0).GetChild(equipMonsterNum).GetComponent<Monster>();
            Transform putInven = null;
            for (int i = 0; i < monsterList.childCount; i++)
            {
                if (monsterList.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text ==
                    equipMonster.monsterName)
                    putInven = monsterList.GetChild(i).transform;
            }   
            equipMonster.gameObject.transform.SetParent(putInven); // 기존에 장착하던 몬스터
            equipMonster.transform.parent.gameObject.SetActive(true);
            equipMonster.transform.parent.transform.eulerAngles = new Vector3(0, 0, 0);
            equipMonster.gameObject.layer = 3;
            clickMonster.transform.parent.gameObject.SetActive(false);
            clickMonster.gameObject.SetActive(true); // 장착하는 몬스터
            clickMonster.transform.SetParent(playerInBattle.transform.parent.GetChild(1).GetChild(0));
            // 장착했던 몬스터의 인덱스를 가져와서 그 자리(하이어라키)에 넣음
            clickMonster.transform.SetSiblingIndex(equipMonsterNum);
            clickMonster.transform.position = new Vector3(1495.19995f, 6000.60986f, -88.6800003f);
            clickMonster.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
            clickMonster.transform.localEulerAngles = new Vector3(0, 0, 0);
            if (playerInBattle.monsters.Count > 0)
            {
                for (int i = 0; i < playerInBattle.monsters.Count; i++)
                {
                    playerInBattle.monsters[i].gameObject.layer = i + 6;
                    playerUI.monsterAttribute[i].color = uiManager.skillAttrib[playerInBattle.monsters[i].
                        GetComponent<Monster>().GetType().Name.Substring(0, 4)]; // 색 변경
                }
            }
            //clickMonster.gameObject.layer = 6 + equipMonsterNum;
            ChangeMonsterUI(true);
            #endregion
        }
    }

    //----------------------------------------몬스터에 대한 자세한 정보 보기----------------------------------------------
    #region DetailShowInfo

    void MonstersInWorld()
    {
        clickMonster.transform.position = new Vector3(1495.19995f, 6000.60986f, -88.6800003f);
        clickMonster.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
        clickMonster.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void ExitDetailInfo()
    {
        clickMonster.gameObject.SetActive(false);
        monsterDetail.SetActive(false);
        clickMonster.transform.GetChild(3).gameObject.SetActive(false);
    }
    public void SkillOverView()
    {
        Monster monsterSkill = EventSystem.current.currentSelectedGameObject.GetComponent<Monster>();
        skillViewText.text = uiManager.skillShowEffect[monsterSkill.equipSkill[0]];
        skillView.SetActive(true);
    }
    #endregion

    //----------------------------------------------장착한 몬스터 변경----------------------------------------------------
    #region ChangeEquipMonster

    public void ChangeMonster() // 장착한 몬스터UI에서 변경하기를 누르면 버튼이 활성화
    {
        changeEquipMonster = true;
        for (int i = 0; i < playerInBattle.monsters.Count; i++)
            monsterChangeButton[i].SetActive(true);
    }


    public void ChangeMonsterUI(bool state) // (몬스터 인벤토리에서 변경하기 클릭 / 나가기)
    {
        equipMonsterChange.SetActive(state); 
        for (int i = 0; i < equipMonsterObj.Length; i++)
            equipMonsterObj[i].SetActive(false);
        for (int i = 0; i < playerInBattle.monsters.Count; i++)
        {
            equipMonsterNames[i].text = playerInBattle.monsters[i].GetComponent<Monster>().monsterName;
            equipMonsterObj[i].SetActive(true);
        }
        if (!state)
        {
            for (int i = 0; i < monsterChangeButton.Length; i++)
                monsterChangeButton[i].SetActive(false);
            changeEquipMonster = false;
        }
        
    }
    public void EquipMonsterSelect(int index) // 변경 버튼을 누르면 인덱스가 저장되고 후에 인덱스값을 받아서 장착된 몬스터를 제거 해줌 
    {
        if (changeEquipMonster)
        {
            equipMonsterNum = index;
            equipMonsterChange.SetActive(false);
            for (int i = 0; i < monsterChangeButton.Length; i++)
                monsterChangeButton[i].SetActive(true);
        }
        else
            return;
    }

    #endregion

}
