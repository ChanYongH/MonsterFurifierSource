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
    //���� ����
    GameObject monsterDetail;
    GameObject monsterSkills;
    GameObject skillView;
    public List<TextMeshProUGUI> detailText = new List<TextMeshProUGUI>(); // 1: �ɷ�ġ 2: �̸� 3: �Ӽ�
    public List<TextMeshProUGUI> skillText = new List<TextMeshProUGUI>();
    TextMeshProUGUI skillViewText;

    //���� ���� ����
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
        monsterSkills = monsterDetail.transform.GetChild(0).GetChild(6).gameObject; // ��ų
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
    //---------------------------------------���� ��ȹ �� �߻��ϴ� �Լ���-----------------------------------------------
    #region GetMonster


    public void GetMonster(Monster monster)
    {
        monsters.Add(monster); // List<Monster>
        int lastNum = monsters.Count - 1; // List�� ������int���� ĳ�� ����
        monsters[lastNum].transform.parent.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(ShowMonsters);
        monsters[lastNum].tag = "PlayerMonster";
        monsters[lastNum].playerMonster = true;
        monsterObj.Add(monsters[lastNum].transform.parent.gameObject);
        monsterObj[lastNum].transform.SetParent(monsterList); //���� ��ü ��ü�� ���� �κ��丮�� �־��ش�.
        monsterObj[lastNum].SetActive(true);
        monsterObj[lastNum].transform.localScale = new Vector3(1, 1, 1);
        monsterObj[lastNum].transform.localPosition = new Vector3(0, 0, 0);
        addScroll.SetContentSize(); // 30�� �̻��� �Ǹ� �ڵ����� ��ũ���� ����
        if (playerInBattle.monsters != null)
        {
            if (playerInBattle.monsters.Count < 3) // �÷��̾ ������ ���Ͱ� 3�� �̸��̸� �ڵ����� �߰� ���ش�.
                AutoEquipMonster(monsterObj[lastNum], monsters[lastNum]);
        }
    }
    public void AutoEquipMonster(GameObject monstersObj, Monster monster)
    {
        monstersObj.gameObject.SetActive(false); // �ڵ����� ������ �Ǹ� ���� ��ü�� �θ�(�ֻ��� ������Ʈ)�� ���ش�.
        monster.transform.SetParent(playerInBattle.transform.parent.GetChild(1).GetChild(0)); // ���� �ڵ� ����
        monster.gameObject.SetActive(true); // ������ �ؽ�ó�� �̿��ؼ� ������ �� �ְ� �Ϸ��ߴ�.
        playerInBattle.SetEquipMonster(); // for������ ���ٰ� �״ٰ� ���ִ� ����(����)
        if (monsters.Count > 0) // ���Ͱ� �߰� �Ǹ� ���̾ �־��ش�.(������ �ؽ�ó�� ó�� �ϱ� ���� ���ִ� �۾�) 
        {
            for (int i = 0; i < monsters.Count; i++)
                monsters[i].gameObject.layer = i + 6;
        }

        // ������ ���͸� �Ű���
        if (playerInBattle.monsters.Count > 0) // ���Ͱ� �߰� ���ڸ��� ������ ��ġ�� �Űܼ� ���Ͱ� ���̾ ���̰� �س��´�.
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
                    GetComponent<Monster>().GetType().Name.Substring(0, 4)]; // �� ����
                    
                    //GetType().Name.Substring(0, 4)];
            }
        }
    }
    #endregion

    public void ShowMonsters() // ���� ������ ���ų� ������ ���͸� �ٲ� �� �� ���� ��
    {
        // Ŭ���� ��ü�� ���� Ŭ������ �����ͼ� ������ ������
        GameObject monsterObject = EventSystem.current.currentSelectedGameObject.transform.parent.parent.gameObject; 
        clickMonster = monsterObject.transform.GetChild(1).GetComponent<Monster>();
        if(!changeEquipMonster)
        {
            #region MonsterShowInfo
            monsterDetail.SetActive(true);
            clickMonster.transform.GetChild(2).gameObject.SetActive(true); // ī�޶�
            Skills cmSkill = clickMonster.transform.GetChild(0).GetComponent<Skills>();
            detailText[0].text = "ü�� : " + clickMonster.hp + System.Environment.NewLine +
                                 "���ݷ� : " + clickMonster.att + System.Environment.NewLine +
                                 "���� : " + clickMonster.def + System.Environment.NewLine +
                                 "Ư�� ���ݷ� : " + clickMonster.spAtt + System.Environment.NewLine +
                                 "Ư�� ���� : " + clickMonster.spDef;
            detailText[1].text = clickMonster.monsterName;
            detailText[2].text = "��";
            detailText[3].text = clickMonster.monsterExplain;
            clickMonster.gameObject.SetActive(true);
            for (int i = 0; i < clickMonster.equipSkill.Count; i++)
                skillText[i].text = cmSkill.nameToKorean[clickMonster.equipSkill[i]];
            #endregion
        }
        else // ������ ���͸� �ٲ� �� �� ����
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
            equipMonster.gameObject.transform.SetParent(putInven); // ������ �����ϴ� ����
            equipMonster.transform.parent.gameObject.SetActive(true);
            equipMonster.transform.parent.transform.eulerAngles = new Vector3(0, 0, 0);
            equipMonster.gameObject.layer = 3;
            clickMonster.transform.parent.gameObject.SetActive(false);
            clickMonster.gameObject.SetActive(true); // �����ϴ� ����
            clickMonster.transform.SetParent(playerInBattle.transform.parent.GetChild(1).GetChild(0));
            // �����ߴ� ������ �ε����� �����ͼ� �� �ڸ�(���̾��Ű)�� ����
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
                        GetComponent<Monster>().GetType().Name.Substring(0, 4)]; // �� ����
                }
            }
            //clickMonster.gameObject.layer = 6 + equipMonsterNum;
            ChangeMonsterUI(true);
            #endregion
        }
    }

    //----------------------------------------���Ϳ� ���� �ڼ��� ���� ����----------------------------------------------
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

    //----------------------------------------------������ ���� ����----------------------------------------------------
    #region ChangeEquipMonster

    public void ChangeMonster() // ������ ����UI���� �����ϱ⸦ ������ ��ư�� Ȱ��ȭ
    {
        changeEquipMonster = true;
        for (int i = 0; i < playerInBattle.monsters.Count; i++)
            monsterChangeButton[i].SetActive(true);
    }


    public void ChangeMonsterUI(bool state) // (���� �κ��丮���� �����ϱ� Ŭ�� / ������)
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
    public void EquipMonsterSelect(int index) // ���� ��ư�� ������ �ε����� ����ǰ� �Ŀ� �ε������� �޾Ƽ� ������ ���͸� ���� ���� 
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
