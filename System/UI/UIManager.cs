using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Reflection;

public class UIManager : Singleton<UIManager>
{
    public static string setPlayerName;


    //Skills
    GameObject skillChangeCanvas;
    TextMeshProUGUI[] skillChangeText = new TextMeshProUGUI[3];
    GameObject changeSkillCanvas;
    TextMeshProUGUI changeSkillText;
    TextMeshProUGUI[] changeSkillListText = new TextMeshProUGUI[3];
    GameObject skillListCanvas;
    TextMeshProUGUI[] skillListText = new TextMeshProUGUI[3];
    TextMeshProUGUI[] skillListEffectText = new TextMeshProUGUI[3];
    public Dictionary<string, string> skillShowEffect = new Dictionary<string, string>();
    public Dictionary<string, Color> skillAttrib = new Dictionary<string, Color>();
    //Battle
    GameObject battleMessageCanvas; // ��Ʋ ��Ȳ�� ĵ������ ǥ�����ִ� �뵵
    Dictionary<string, int> massageDic = new Dictionary<string, int>(); // GetChild�� �������ְ� ����� �ֱ� ���� ���
    List<GameObject> battleMessages = new List<GameObject>(); // ��Ʋ ĵ���� �ȿ� �ִ� ��ư���� �ν��Ͻ�ȭ �ϱ� ���ؼ� ���
    BattleManger battleManager;

    //BattleSequnce
    public Transform sequnceCanvas;
    //Prefabs
    public GameObject playerTurn;
    public GameObject enemyTurn;
    //ObjectPooling
    public Queue<GameObject> playerQueue = new Queue<GameObject>();
    public Queue<GameObject> enemyQueue = new Queue<GameObject>();
    public int maxCount;


    //UI
    public GameObject playerLevelUp;
    List<Button> battle = new List<Button>();
    List<Button> useSkill = new List<Button>();
    List<Button> changeSkillAsk = new List<Button>();
    List<Button> changeSkill = new List<Button>();
    public bool skillChange = false;

    //MonsterUI
    GameObject playerMonsterUI;
    GameObject enemyMonsterUI;
    TextMeshProUGUI[] monsterNames = new TextMeshProUGUI[2];
    public Image[] monsterHpUI = new Image[2]; // �÷��̾�/�� ������ Hp
    public Image monsterExpUI; // �÷��̾� ������ ����ġ
    TextMeshProUGUI[] monsterLevel = new TextMeshProUGUI[2]; // �÷��̾�/�� ������ ����

    //Item
    GameObject itemUseCanvas; // UI ó���� ���ؼ� �����;� ��
    ItemInventory itemInven; // ���� ������ �����;� ��
    GameObject itemUseList;
    List<Button> itemSelect = new List<Button>();
    List<GameObject> itemObj = new List<GameObject>();
    List<Items> items = new List<Items>();
    List<Button> useItem = new List<Button>();
    //Queue<GameObject> itemSequence = new Queue<GameObject>();
    public List<GameObject> itemSequence = new List<GameObject>();
    public int itemIndex = 0; // ������ ��

    //Capture
    GameObject captureCanvas;
    List<GameObject> captureUI = new List<GameObject>();
    List<TextMeshProUGUI> captureMonsterInfo = new List<TextMeshProUGUI>();
    List<TextMeshProUGUI> captureMonsterSkills = new List<TextMeshProUGUI>();
    List<Button> captureButton = new List<Button>();
    MonsterInventory monsterInven;
    //NameChange
    public TMP_InputField nameInput;
    public Button nameChange;



    public bool captureState = false;
    public bool captureProgress = false;

    bool[] monsterEvolution = new bool[2];
    public Skills skill;
    GameObject player;
    public PlayerBattle playerInBattle;
    public PlayerWorld playerInWorld;
    Monster playerMonster;
    Monster enemyMonster;
    public string selectSkill;
    public string enemySetSkill;

    public float flowTime;
    private void Start()
    {
        PlayerWorld.Gobattle += BattleUI;
        PlayerBattle.MonsterChange += MonsterChangeOverrideUI;
        playerInWorld = GameObject.FindGameObjectWithTag("WorldPlayer").GetComponent<PlayerWorld>();
        monsterInven = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetComponentInChildren<MonsterInventory>();
        #region SkillColors
        //��ų �÷�(�Ӽ��� �°�)

        Color redColor;
        ColorUtility.TryParseHtmlString("#F08080", out redColor);
        Color greenColor;
        ColorUtility.TryParseHtmlString("#ADFF2F", out greenColor);
        Color blueColor;
        ColorUtility.TryParseHtmlString("#87CEFA", out blueColor);
        Color brownColor;
        ColorUtility.TryParseHtmlString("#D2B48C", out brownColor);

        skillAttrib.Add("Noma", Color.gray);
        skillAttrib.Add("Fire", redColor);
        skillAttrib.Add("Wate", blueColor);
        skillAttrib.Add("Natu", greenColor);
        skillAttrib.Add("Figh", brownColor);

        // ó���� �� �����̵ȴ�.
        // �� ��° ���� ó���� ����� ������ ��� �����ִ�.
        #endregion

    }
    void BattleUI() // ���� ���� �� �� ���� ����
    {
        if (captureState)
        {
            CaputreSuccess();
            captureState = false;
            captureProgress = false;
            playerInWorld.equipMonster = 0;
            return;
        }
        Time.timeScale = 0.01f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // �۾� �� ���� ���� ���� �� 
        playerMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
        enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
        sequnceCanvas = GameObject.FindGameObjectWithTag("BattleSequnce").transform;
        playerInBattle = GameObject.FindGameObjectWithTag("BattlePlayer").GetComponent<PlayerBattle>();
        playerMonsterUI = GameObject.FindGameObjectWithTag("MonsterUI").transform.GetChild(0).GetChild(0).gameObject;
        enemyMonsterUI = GameObject.FindGameObjectWithTag("MonsterUI").transform.GetChild(0).GetChild(1).gameObject;
        captureCanvas = GameObject.FindGameObjectWithTag("CaptureSucceeded").transform.GetChild(0).gameObject;
        playerMonsterUI.transform.parent.gameObject.SetActive(true); //�÷��̾�, �� ����UI Ȱ��ȭ
        playerMonsterUI.SetActive(true);
        enemyMonsterUI.SetActive(true);
        battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManger>();
        battleMessageCanvas = GameObject.FindGameObjectWithTag("BattleMessageCanvas");
        // �� ���� �ߵ�
        #region Battle 
        if (battle.Count <= 0)
        {
            SetQueue();
            for (int i = 0; i < captureCanvas.transform.childCount; i++)
                captureUI.Add(captureCanvas.transform.GetChild(i).gameObject);

            for (int i = 0; i < captureUI[1].transform.childCount - 1; i++)
                captureMonsterInfo.Add(captureUI[1].transform.GetChild(i).GetComponent<TextMeshProUGUI>());

            for (int i = 0; i < captureUI[1].transform.GetChild(captureUI[1].transform.childCount - 1).transform.childCount; i++)
                captureMonsterSkills.Add(captureUI[1].transform.GetChild(captureUI[1].transform.childCount - 1).transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());

            nameInput = captureUI[2].transform.GetChild(3).GetComponent<TMP_InputField>();
            nameChange = nameInput.transform.GetChild(1).GetComponent<Button>();

            captureButton.Add(captureUI[0].GetComponent<Button>()); // �޼���(������ ��Ҵ�)
            captureButton.Add(captureUI[1].GetComponent<Button>()); // ���� ����

            captureButton.Add(captureUI[2].transform.GetChild(0).GetComponent<Button>()); // �̸� �ٲܷ���
            captureButton.Add(captureUI[2].transform.GetChild(1).GetComponent<Button>()); // �� �ٲܷ���

            captureButton[0].onClick.AddListener(CaptureMonsterShowInfo);
            captureButton[1].onClick.AddListener(AskChangeName);
            captureButton[2].onClick.AddListener(() => AskChangeName2(true));
            captureButton[3].onClick.AddListener(() => AskChangeName2(false));
            nameChange.onClick.AddListener(ChangeName);

            #region BattleDictionary
            massageDic.Add("OnBattle", 0);
            massageDic.Add("Start", 1);
            massageDic.Add("End", 2);
            massageDic.Add("SkillUse", 3);
            massageDic.Add("AttackState", 4);
            massageDic.Add("PlayerLevelUp", 5);
            massageDic.Add("BattleProgress", 6);
            massageDic.Add("BattleNext", 7);
            massageDic.Add("ItemUseStart", 8);
            #endregion
            for (int i = 0; i < battleMessageCanvas.transform.childCount; i++)
                battle.Add(battleMessageCanvas.transform.GetChild(i).GetComponent<Button>());
            battle[massageDic["OnBattle"]].onClick.AddListener(BattleStart);
            battle[massageDic["Start"]].onClick.AddListener(() => BattleSelect(0)); // ���� ��� 
            battle[massageDic["End"]].onClick.AddListener(BattleExit);
            battle[massageDic["BattleNext"]].onClick.AddListener(BattleNext);
            battle[massageDic["ItemUseStart"]].onClick.AddListener(() => BattleSelect(1));
            #region BattleCanvas
            battleMessages.Add(battleMessageCanvas.transform.GetChild(massageDic["OnBattle"]).gameObject);
            battleMessages.Add(battleMessageCanvas.transform.GetChild(massageDic["Start"]).gameObject);
            battleMessages.Add(battleMessageCanvas.transform.GetChild(massageDic["End"]).gameObject);
            battleMessages.Add(battleMessageCanvas.transform.GetChild(massageDic["SkillUse"]).gameObject);
            battleMessages.Add(battleMessageCanvas.transform.GetChild(massageDic["AttackState"]).gameObject);
            battleMessages.Add(battleMessageCanvas.transform.GetChild(massageDic["PlayerLevelUp"]).gameObject);
            battleMessages.Add(battleMessageCanvas.transform.GetChild(massageDic["BattleProgress"]).gameObject);
            battleMessages.Add(battleMessageCanvas.transform.GetChild(massageDic["BattleNext"]).gameObject);
            battleMessages.Add(battleMessageCanvas.transform.GetChild(massageDic["ItemUseStart"]).gameObject);
            #endregion

            #region MonsterUI
            monsterNames[0] = playerMonsterUI.GetComponentInChildren<TextMeshProUGUI>();
            monsterNames[1] = enemyMonsterUI.GetComponentInChildren<TextMeshProUGUI>();
            monsterHpUI[0] = playerMonsterUI.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            monsterHpUI[1] = enemyMonsterUI.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            monsterExpUI = playerMonsterUI.transform.GetChild(2).GetChild(0).GetComponent<Image>();
            monsterLevel[0] = playerMonsterUI.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            monsterLevel[1] = enemyMonsterUI.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            #endregion
        }
        #endregion

        
        #region MonsterUI
        monsterExpUI.fillAmount = playerMonster.exp / playerMonster.levelUpExp; // �� ������ �����ָ� �������ڸ���
                                                                                // �ڱ� hp�� exp�� ���缭 õõ�� �����ϰų� ������
        monsterHpUI[0].fillAmount = playerMonster.hp / playerMonster.maxHp;
        monsterHpUI[1].fillAmount = enemyMonster.hp / enemyMonster.maxHp;
        monsterNames[0].text = playerMonster.monsterName;
        monsterNames[1].text = enemyMonster.monsterName;
        monsterLevel[0].text = "Lv " + playerMonster.level;
        monsterLevel[1].text = "Lv " + enemyMonster.level;
        #endregion
        #region Skills
        skill = playerMonster.gameObject.transform.GetChild(0).GetComponent<Skills>();
        skillChangeCanvas = GameObject.FindGameObjectWithTag("ChangeSkillCanvas");
        //����
        for (int i = 0; i < skillChangeText.Length; i++)
            skillChangeText[i] = skillChangeCanvas.transform.GetChild(0).GetChild(2).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        changeSkillCanvas = GameObject.FindGameObjectWithTag("ChangeSkillCanvas2");
        changeSkillText = changeSkillCanvas.transform.GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        skillListCanvas = GameObject.FindGameObjectWithTag("SkillListCanvas");
        // ��ų�� ���� ��(��� �ʱ�ȭ)
        if (useSkill.Count <= 0)
        {
            for (int i = 0; i < skillListCanvas.transform.GetChild(0).childCount - 1; i++)
            {
                skillListText[i] = skillListCanvas.transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
                skillListEffectText[i] = skillListCanvas.transform.GetChild(0).GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
                changeSkillListText[i] = changeSkillCanvas.transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
                useSkill.Add(skillListCanvas.transform.GetChild(0).GetChild(i).GetComponent<Button>());
            }
            // for���� ���� ������ �� ����??? ���ٶ�??

            useSkill[0].onClick.AddListener(() => UIUseSkill(0));
            useSkill[1].onClick.AddListener(() => UIUseSkill(1));
            useSkill[2].onClick.AddListener(() => UIUseSkill(2));

            changeSkillAsk.Add(skillChangeCanvas.transform.GetChild(0).GetChild(0).GetComponent<Button>());
            changeSkillAsk.Add(skillChangeCanvas.transform.GetChild(0).GetChild(1).GetComponent<Button>());
            changeSkillAsk[0].onClick.AddListener(() => ChangeSkillBool(true));
            changeSkillAsk[1].onClick.AddListener(() => ChangeSkillBool(false));
            for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
                changeSkill.Add(changeSkillCanvas.transform.GetChild(0).GetChild(i).GetComponent<Button>());
            changeSkill[0].onClick.AddListener(() => ChanageSkill(0));
            changeSkill[1].onClick.AddListener(() => ChanageSkill(1));
            changeSkill[2].onClick.AddListener(() => ChanageSkill(2));

            //��ų ����Ʈ
            #region
            skillShowEffect.Add("NomalAttack1", "����� 40");
            skillShowEffect.Add("NomalAttDebuff", "�� ���ݷ� ��ȭ");
            skillShowEffect.Add("NomalAttack2", "����� 50");
            skillShowEffect.Add("NomalDefenceUp", "���� ����");
            skillShowEffect.Add("NomalSpDefenceUp", "Ư�� ���� ����");
            skillShowEffect.Add("NomalAttack3", "����� 65");
            skillShowEffect.Add("NomalAttack4", "����� 80");
            skillShowEffect.Add("NomalAgilitybuff", "������ ����");
            skillShowEffect.Add("FireAttack1", "Ư�� ����� 40");
            skillShowEffect.Add("FireSpDefDebuff", "�� Ư�� ���� ��ȭ");
            skillShowEffect.Add("FireAttack2", "Ư�� ����� 60");
            skillShowEffect.Add("FireSpAttBuff", "Ư�� ���ݷ� ����");
            skillShowEffect.Add("FireAttack3", "Ư�� ����� 90(������ �Ҹ�*2)");
            skillShowEffect.Add("FireSpDefBuff", "Ư�� ���� ����");
            skillShowEffect.Add("FireAttack4", "Ư�� ����� 100");
            skillShowEffect.Add("WaterAttack1", "Ư�� ����� 40");
            skillShowEffect.Add("WaterAgilitybuff", "������ ����");
            skillShowEffect.Add("WaterAttack2", "Ư�� ����� 50");
            skillShowEffect.Add("WaterSpAttbuff", "Ư�� ����� ����");
            skillShowEffect.Add("WaterAttack3", "Ư�� ����� 60(����Ÿ��)");
            skillShowEffect.Add("WaterEnduranceRecovery", "������ ��� ȸ��");
            skillShowEffect.Add("WaterAttack4", "Ư�� ����� 70(������ ����)");
            skillShowEffect.Add("NatureAttack1", "Ư�� ����� 40");
            skillShowEffect.Add("NatureHpRecovery", "ü�� ȸ��");
            skillShowEffect.Add("NatureAttack2", "Ư�� ����� 50");
            skillShowEffect.Add("NatureDoubleDefBuff", "����, Ư�� ���� ����");
            skillShowEffect.Add("NatureAttack3", "Ư�� ����� 40(HPȸ��)");
            skillShowEffect.Add("NatureSpAttBuff", "Ư�� ����� ���� ����");
            skillShowEffect.Add("NatureAttack4", "Ư�� ����� 80");
            skillShowEffect.Add("FighterAttack1", "����� 60");
            skillShowEffect.Add("FighterAttBuff", "���ݷ� ����");
            skillShowEffect.Add("FighterAttack2", "����� 80");
            skillShowEffect.Add("FighterDefDebuff", "���� ����");
            skillShowEffect.Add("FighterAttack3", "����� 150(�ൿ�� ��� �Ҹ�)");
            skillShowEffect.Add("FighterAttack4", "����� 100");
            skillShowEffect.Add("FighterAttDefBuff", "����, Ư�� ���� ����");
            #endregion
            #endregion
        #region ItemUse
            itemUseCanvas = GameObject.FindGameObjectWithTag("ItemUseCanvas").transform.GetChild(0).gameObject;
            itemInven = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetComponentInChildren<ItemInventory>();
            itemSelect.Add(itemUseCanvas.transform.GetChild(1).GetComponent<Button>());
            itemSelect.Add(itemUseCanvas.transform.GetChild(2).GetComponent<Button>());
            itemUseList = itemUseCanvas.transform.GetChild(3).gameObject;
            for (int i = 0; i < itemUseList.transform.childCount; i++)
            {
                itemObj.Add(itemUseList.transform.GetChild(i).gameObject);
                useItem.Add(itemObj[i].GetComponent<Button>());
                items.Add(itemObj[i].GetComponentInChildren<Items>());
            }

            itemSelect[0].onClick.AddListener(() => ItemSelect(true));
            itemSelect[1].onClick.AddListener(() => ItemSelect(false));
            useItem[0].onClick.AddListener(() => UseItemUI(0));
            useItem[1].onClick.AddListener(() => UseItemUI(1));
            useItem[2].onClick.AddListener(() => UseItemUI(2));
            useItem[3].onClick.AddListener(() => UseItemUI(3));


        #endregion

            // �ڷΰ���
            skillListCanvas.transform.GetChild(0).GetChild(3).GetComponent<Button>().
            onClick.AddListener(ActionBack);
            itemUseCanvas.transform.GetChild(5).GetComponent<Button>().
            onClick.AddListener(ActionBack);
        }
        //nameChange.onClick.AddListener(ChangeName);
        playerMonsterUI.GetComponent<Image>().color = skillAttrib[playerMonster.
        GetType().Name.Substring(0, 4)];
        enemyMonsterUI.GetComponent<Image>().color = skillAttrib[enemyMonster.
        GetType().Name.Substring(0, 4)];
        //playerMonsterUI.GetComponent<Image>().fillAmount = playerMonster.hp / playerMonster.maxHp;
        //enemyMonsterUI.GetComponent<Image>().fillAmount = enemyMonster.hp / enemyMonster.maxHp;

        OnBattle();
    }
    //----------------------------------------���� ����--------------------------------------------------
    #region MonsterChange
    void MonsterChangeOverrideUI()
    {
        playerMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
        skill = playerMonster.gameObject.transform.GetChild(0).GetComponent<Skills>();
    }

    public IEnumerator MonsterChangeCo()
    {
        //�÷��̾��� ��� ������ �������� 0���� ������ 
        bool isLethargy = playerInBattle.monsters[0].GetComponent<Monster>().endurance <= 0 &&
        playerInBattle.monsters[1].GetComponent<Monster>().endurance <= 0 && playerInBattle.monsters[2].GetComponent<Monster>().endurance <= 0;

        yield return new WaitForSecondsRealtime(2);
        battleMessages[massageDic["BattleProgress"]].SetActive(true);
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "��(��) �ǿ��� ��� �Ҿ���!";
        yield return new WaitForSecondsRealtime(3);
        if (!isLethargy) //�÷��̾� ������ �������� �� ���� �̻� �̶� 0���� ������ ����
        {
            PlayerBattle.MonsterChange.Invoke(); // ��������Ʈ ����
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                "���� ���Ͱ� �ٽ� ������ ��� ����";
            yield return new WaitForSecondsRealtime(2);
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                playerMonster.monsterName + "��(��) �ٽ� ������ �����Ѵ�!";
            yield return new WaitForSecondsRealtime(2);
            battleMessages[massageDic["BattleProgress"]].SetActive(false);
            BattleStart();
        }
        else
        {
            BattleResult(0, 0, false); //�÷��̾��� ��� ������ �������� 0���� ������ �й�
        }
    }
    #endregion
    //-----------------------------------------���� �κ�---------------------------------------------------
    #region BattleSequnce
    void SetQueue()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject player = Instantiate(playerTurn, sequnceCanvas.GetChild(0));
            GameObject enemy = Instantiate(enemyTurn, sequnceCanvas.GetChild(1));
            player.SetActive(false);
            enemy.SetActive(false);
            playerQueue.Enqueue(player);
            enemyQueue.Enqueue(enemy);
        }
    }
    IEnumerator SetSequnce(bool first)
    {
        if (!first) // ó�� �ߵ��ϴ°� �ƴϸ�(������Ʈ�� ���� ���ִ� ������ ��)
        {
            for (int i = 0; i < sequnceCanvas.GetChild(0).childCount; i++)
            {
                sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
                sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
        }
        int playerEndu = playerMonster.endurance;
        int enemyEndu = enemyMonster.endurance;
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.22f);
            if (playerEndu >= enemyEndu && playerEndu > 0) // ������ ��
            {
                if (playerQueue.Count <= 0) // ť�� ���̻� ������
                    break;
                playerQueue.Peek().SetActive(true);
                playerQueue.Dequeue();
                playerEndu -= playerMonster.agility; // ������ - ������
            }
            else if (playerEndu < enemyEndu && enemyEndu > 0)
            {
                if (enemyQueue.Count <= 0)
                    break;
                enemyQueue.Peek().SetActive(true);
                enemyQueue.Dequeue();
                enemyEndu -= enemyMonster.agility;
            }
            else
                break;
        }
    }
    #endregion
    #region Battle
    // 1�� °
    public void OnBattle()
    {

        for (int i = 0; i < sequnceCanvas.GetChild(0).childCount; i++) //
        {
            sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
            sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        battleMessages[massageDic["OnBattle"]].SetActive(true);
        if (captureProgress)
        {
            battleMessages[massageDic["OnBattle"]].
            GetComponentInChildren<TextMeshProUGUI>().text =
            "��ȹ�� ���� �ߴ�!";
            captureProgress = false;
        }
        else
            battleMessages[massageDic["OnBattle"]].
            GetComponentInChildren<TextMeshProUGUI>().text =
            " ' " + enemyMonster.monsterName + " ' " + "�� �������� ���Դ�!";

    }
    // (BattleFlow)2�� ° 
    public void BattleStart() // ���� ���� 
    {
        StartCoroutine(SetSequnce(false)); // ��Ʋ ������ On
        battleMessages[massageDic["OnBattle"]].SetActive(false);
        battleMessages[massageDic["Start"]].SetActive(true);
        battleMessages[massageDic["ItemUseStart"]].SetActive(true);
        BattleManger.battle = true;
    }

    // (BattleFlow)3�� °
    public void BattleSelect(int select) // 0 == �ο�� / 1 == ������ ��� / 2 == �������� (��ư �Ҵ�)
    {
        if (select == 0) // ���� ����
            TurnAction();
        if (select == 1) // �����ۻ��
            ItemUse();
        //if (select == 2)
        //    ����
        battleMessages[massageDic["Start"]].SetActive(false);
        battleMessages[massageDic["ItemUseStart"]].SetActive(false);
    }
    public void BattleResult(float getExp, float getMoney, bool match)
    {
        playerMonster.Exp += enemyMonster.Exp;
        playerInWorld.money += enemyMonster.money;
        monsterEvolution[0] = playerMonster.Level == 11 && playerMonster.evolution[0];
        monsterEvolution[1] = playerMonster.Level == 11 && playerMonster.evolution[1];

        battleMessages[massageDic["BattleProgress"]].SetActive(true);
        if (match) // �¸��ϸ�
        {
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                enemyMonster.monsterName + " ��ȭ ����!" + System.Environment.NewLine + "ȹ�� ����ġ : " + getExp.ToString()
                + System.Environment.NewLine + "ȹ���� �� : " + getMoney.ToString();
            if (playerMonster.levelUp)
            {
                StartCoroutine(PlayerMonsterLevelUp());
            }
            else
                StartCoroutine(BattleEndCo());
        }
        else // �й��ϸ�
        {
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                "��ȭ�� ���� �Ͽ����ϴ�!";
            StartCoroutine(BattleEndCo());
        }
    }
    // ���� ��
    public IEnumerator BattleEndCo()
    {
        //playerMonsterUI.transform.parent.gameObject.SetActive(false); //�÷��̾�, �� ����UI Ȱ��ȭ

        battleMessages[massageDic["AttackState"]].SetActive(false);
        enemyMonsterUI.SetActive(false);
        yield return new WaitForSecondsRealtime(flowTime * 2);
        for (int i = 0; i < sequnceCanvas.GetChild(0).childCount; i++)
        {
            sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
            sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        battleMessages[massageDic["BattleProgress"]].SetActive(false);
        battleMessages[massageDic["End"]].SetActive(true);
        battleMessages[massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
                "���� ������";

    }
    public void BattleExit()
    {
        playerMonsterUI.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // �۾� �� ���� ���� ���� �� 
        for (int i = 0; i < sequnceCanvas.GetChild(0).childCount; i++)
        {
            sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
            sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        battleMessages[massageDic["End"]].SetActive(false);
        battleMessages[massageDic["PlayerLevelUp"]].SetActive(false);
        skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(false);
        changeSkillCanvas.transform.GetChild(0).gameObject.SetActive(false);
        BattleManger.battle = false;
        if (!enemyMonster.playerMonster)
            enemyMonster.tag = "EnemyMonster";

        enemyMonster.transform.position = enemyMonster.monsterWorldPos.transform.position;
        PlayerWorld.battleOut.Invoke();
    }

    void ActionBack()
    {
        for (int i = 0; i < itemSequence.Count; i++) // ������ ���� �ʱ�ȭ(������ ù ��° �������� ���� ������ �ϱ�����)
            itemSequence[i].SetActive(false);
        skillListCanvas.transform.GetChild(0).gameObject.SetActive(false);
        itemUseCanvas.SetActive(false);
        BattleStart();
    }
    #endregion
    //------------------------------------����(������)(��ȭ)--------------------------------------------- 
    #region PlayerMonsterEvent
    public IEnumerator PlayerMonsterLevelUp()
    {
        battleMessages[massageDic["AttackState"]].SetActive(false);
        playerMonsterUI.SetActive(false);
        enemyMonsterUI.SetActive(false);
        monsterLevel[0].text = "Lv " + playerMonster.Level;
        monsterLevel[1].text = "Lv " + enemyMonster.Level;
        yield return new WaitForSecondsRealtime(2);
        for (int i = 0; i < sequnceCanvas.GetChild(0).childCount; i++) // ���� ����UI ����
        {
            sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
            sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "�� ������ �ö���!";
        yield return new WaitForSecondsRealtime(2);
        battleMessages[massageDic["BattleProgress"]].SetActive(false);
        battleMessages[massageDic["BattleNext"]].SetActive(true); // �ش� ������Ʈ�� ������ �������� �Ѿ 
        battleMessages[massageDic["BattleNext"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "�� ���� ���� : " + playerMonster.Level + System.Environment.NewLine + "(��ư�� ������ �������� �Ѿ�ϴ�.)";
        battleMessages[massageDic["PlayerLevelUp"]].SetActive(true);
        TextMeshProUGUI levelUpText = battleMessages[massageDic["PlayerLevelUp"]].GetComponentInChildren<TextMeshProUGUI>();
        levelUpText.text = playerMonster.monsterName + "�� ü�� : " +
            (playerMonster.Hp - playerMonster.increase[0]) + " =>" + playerMonster.Hp + "(+" + playerMonster.increase[0] + ")";
        levelUpText.text += System.Environment.NewLine + playerMonster.monsterName + "�� ���ݷ� : " +
            (playerMonster.att - playerMonster.increase[1]) + " =>" + playerMonster.att + "(+" + playerMonster.increase[1] + ")";
        levelUpText.text += System.Environment.NewLine + playerMonster.monsterName + "�� ���� : " +
            (playerMonster.def - playerMonster.increase[2]) + " =>" + playerMonster.def + "(+" + playerMonster.increase[2] + ")";
        levelUpText.text += System.Environment.NewLine + playerMonster.monsterName + "�� Ư�� ���ݷ� : " +
            (playerMonster.spAtt - playerMonster.increase[3]) + " =>" + playerMonster.spAtt + "(+" + playerMonster.increase[3] + ")";
        levelUpText.text += System.Environment.NewLine + playerMonster.monsterName + "�� Ư�� ���� : " +
            (playerMonster.spDef - playerMonster.increase[4]) + " =>" + playerMonster.spDef + "(+" + playerMonster.increase[4] + ")";
        playerMonster.levelUp = false;
    }
    public void BattleNext() // ��ȭ / ��ų
    {
        battleMessages[massageDic["BattleNext"]].SetActive(false);
        if (monsterEvolution[0])
        {
            battleMessages[massageDic["BattleProgress"]].SetActive(true);
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            "����?" + playerMonster.monsterName + "�� �����??!!";
            StartCoroutine(MonsterEvolution());
            return;
        } // ��ȭ �� �� 
        else if (monsterEvolution[1])
        {

        }
        if (skillChange) // ��ų ���� �Ұ� ������
        {
            ChangeSkillPanel();
            battleMessages[massageDic["PlayerLevelUp"]].SetActive(false);
            battleMessages[massageDic["BattleProgress"]].SetActive(false);
        }
        else // ���⿡ ��ų�� �߰��ƽ��ϴ� �־��ָ� ��
        {
            if (playerMonster.getSkillState) // ��ų�� ����ٴ� ���� �˷��ֱ� ���� ���� bool�� 
            {
                battleMessages[massageDic["BattleProgress"]].SetActive(true);
                battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                   playerMonster.monsterName + "�� " + " ' " + playerMonster.skill.nameToKorean[playerMonster.skill.skillName]
                   + " ' " + "��(��) ȹ���ߴ�!";
                playerMonster.getSkillState = false;
            }
            StartCoroutine(BattleEndCo());
            battleMessages[massageDic["PlayerLevelUp"]].SetActive(false);

            //battleMessages[massageDic["End"]].SetActive(true);
            //battleMessages[massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
            //    "���� ������";
        }
    }
    IEnumerator MonsterEvolution()
    {
        battleMessages[massageDic["PlayerLevelUp"]].SetActive(false);
        yield return new WaitForSecondsRealtime(1);

        // ��ȭ �ϴ� ��� �����ָ� ��.
        yield return new WaitForSecondsRealtime(6);
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "�� �ް�" + playerMonster.monsterName + "�� ��ȭ�ߴ�!";
        yield return new WaitForSecondsRealtime(4);
        battleMessages[massageDic["BattleProgress"]].SetActive(false);
        ChangeSkillPanel(); // ��ų ����
    }
    #endregion
    //---------------------------------------��ų ��� �κ�------------------------------------------------ 
    #region SkillUse
    // 4�� °
    public void TurnAction() // �ο�⸦ ������
    {
        skillListCanvas.transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < skillListCanvas.transform.GetChild(0).childCount - 1; i++)
            skillListCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < playerMonster.equipSkill.Count; i++) // �����ϰ� �ִ� ��ų�� ���� ��
        {
            skillListCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            skillListText[i].text = playerMonster.skill.nameToKorean[playerMonster.equipSkill[i]];
            skillListEffectText[i].text = skillShowEffect[playerMonster.equipSkill[i]];
            useSkill[i].gameObject.GetComponent<Image>().color = skillAttrib[playerMonster.equipSkill[i].
            Substring(0, 4)];
        }

    }
    // 5�� °
    public void UIUseSkill(int select) // ��ų�� �����ϸ�
    {
        int randNum = UnityEngine.Random.Range(0, 3);
        selectSkill = playerMonster.equipSkill[select];
        if (enemyMonster.equipSkill.Count > 2)
            enemySetSkill = enemyMonster.equipSkill[randNum];
        else
            enemySetSkill = enemyMonster.equipSkill[0]; // 3���� �ƴ� �� ������ ù ��° ����
        if (selectSkill == "WaterAttack3") // ���� Ÿ�� ����
            playerMonster.skill.firstAttack = true;
        skillListCanvas.transform.GetChild(0).gameObject.SetActive(false);
        battleManager.Battle();
    }
    public IEnumerator EnemyUseSkillCo(Monster monster, string monsterSkillName) // ~�� ~�� ����ߴ�
    {
        battleMessages[massageDic["SkillUse"]].SetActive(true);
        battleMessages[massageDic["SkillUse"]].GetComponentInChildren<TextMeshProUGUI>().
            text = monster.monsterName + "�� " + monsterSkillName + "�� ����ߴ�!";
        monster.skill.UseSkill();
        yield return new WaitForSecondsRealtime(flowTime);
        battleMessages[massageDic["SkillUse"]].SetActive(false);
    }
    public IEnumerator AttackState(bool buff, string state, string monsterName = null, string effectInfo = null) // ���� ����
    {
        Debug.Log("AttackState In");
        yield return new WaitForSecondsRealtime(flowTime);
        Debug.Log("AttackState In2");
        battleMessages[massageDic["AttackState"]].SetActive(true);
        if (!buff)
            battleMessages[massageDic["AttackState"]].GetComponentInChildren<TextMeshProUGUI>().
                text = "ȿ���� " + state;
        else
            battleMessages[massageDic["AttackState"]].GetComponentInChildren<TextMeshProUGUI>().
                text = monsterName + "�� " + effectInfo;
        yield return new WaitForSecondsRealtime(flowTime);
        Debug.Log("���ý�����Ʈ �����°� �ߵ��� ?");
        battleMessages[massageDic["AttackState"]].SetActive(false);

    }
    #endregion
    //---------------------------------------��ų ���� �κ�------------------------------------------------- 
    #region SkillChange
    public void AskChangeSkill()
    {
        Debug.Log("��ų����!");
        changeSkillCanvas.transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
            changeSkillCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        changeSkillText.text = "�ؾ� ���� ��ų�� ���� ������ 3���� ��ų �߿� ���� ���ּ���";
        for (int i = 0; i < changeSkillListText.Length; i++)
            changeSkillListText[i].text = playerMonster.skill.nameToKorean[playerMonster.equipSkill[i]];
    }
    public void ChanageSkill(int select)
    {
        for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
            changeSkillCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(false);
        changeSkillText.text = "��ų ������ �����߽��ϴ�!" + System.Environment.NewLine + playerMonster.skill.nameToKorean[playerMonster.equipSkill[select]]
            + System.Environment.NewLine + "������";
        playerMonster.equipSkill.Insert(select, skill.skillName);
        playerMonster.equipSkill.RemoveAt(select + 1);
        changeSkillText.text += System.Environment.NewLine + playerMonster.skill.nameToKorean[playerMonster.equipSkill[select]];
        skill.skillInfo = false;
        battleMessages[massageDic["End"]].SetActive(true);
        battleMessages[massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
            "���� ������";
    }
    public void ChangeSkillPanel() // ù ��°
    {
        skillChangeText[0].text = playerMonster.skill.nameToKorean[playerMonster.skill.skillName];
        skillChangeText[1].text = skillShowEffect[playerMonster.skill.skillName];
        skillChangeText[2].text = "�̹� ��ų�� 3�� ���� �ϰ� �ֽ��ϴ�.";
        skillChangeText[2].text += System.Environment.NewLine + "������ ��� ��ų�� �ؾ������ " + playerMonster.skill.nameToKorean[playerMonster.skill.skillName] + "�� ���ðڽ��ϱ�?";
        skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(true);
        skillChangeCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        skillChangeCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        skillChange = false;
    }
    public void ChangeSkillBool(bool select)
    {
        if (select)
        {
            AskChangeSkill();
            skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(true);
            skillChangeCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            skillChangeCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            skillChangeText[0].text = System.Environment.NewLine + System.Environment.NewLine + "��ų�� �ٲ����ʽ��ϴ�.";
            skillChangeText[1].text = null;
            skillChangeText[2].text = null;
            Debug.Log("�ٲ����ʽ��ϴ�");
            skill.skillInfo = false;
            battleMessages[massageDic["End"]].SetActive(true);
            battleMessages[massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
                "���� ������";
        }

    }
    #endregion

    //--------------------------------------������ ��� �κ�------------------------------------------------
    #region ItemUse

    public void ItemUse()
    {
        battleMessages[massageDic["Start"]].SetActive(false);
        battleMessages[massageDic["OnBattle"]].SetActive(false);
        battleMessages[massageDic["ItemUseStart"]].SetActive(false);
        itemUseCanvas.SetActive(true);
        // �������� �ƹ��͵� ������ ���� ������ ǥ������ �ʰ� �� 
        int itemNull = 0;
        for (int i = 0; i < itemInven.itemCount.Count; i++)
            itemNull += itemInven.itemCount[i];
        if (itemNull == 0)
        {
            itemUseList.SetActive(false);
            itemUseCanvas.transform.GetChild(4).gameObject.SetActive(true);
            return;
        }
        else
        {
            itemUseCanvas.transform.GetChild(4).gameObject.SetActive(false);
            itemUseList.SetActive(true);
        }
        // ������ ���� ǥ��
        itemIndex = 0;
        if (itemSequence.Count > 0)
            itemSequence.Clear();
        for (int i = 0; i < itemUseList.transform.childCount; i++)
            itemObj[i].SetActive(false);


        for (int i = 0; i < itemUseList.transform.childCount; i++)
        {
            if (itemInven.itemCount[i] > 0)
            {
                itemObj[i].transform.GetChild(0).GetChild(2).GetComponentInChildren<TextMeshProUGUI>()
                        .text = itemInven.itemCount[i].ToString();
                itemSequence.Add(itemObj[i]);
            }
        }
        itemSequence[0].SetActive(true); // ù ��° ������ Ȱ��ȭ
    }

    public void ItemSelect(bool left)
    {
        if (!left) // ������
        {
            if (itemSequence.Count > itemIndex + 1) // �������� ������ ������ ���� ���õ� ������ index���� ũ�� ����
            {
                itemSequence[itemIndex].SetActive(false); // ���� �� ���� ������ ������ ��Ȱ��ȭ
                itemIndex++;
                itemSequence[itemIndex].SetActive(true); // ���� �� ���� ������ Ȱ��ȭ
            }
            else // �������� ������ ������ ���� ���õ� ������ index���� ������ 0���� ����� �ش�. 
            {
                itemSequence[itemIndex].SetActive(false);
                itemIndex = 0;
                itemSequence[itemIndex].SetActive(true);
            }
        }
        else
        {
            if (itemIndex != 0) // ���� ���õ� �������� 0�̾ƴϸ� index�� ����
            {
                itemSequence[itemIndex].SetActive(false);
                itemIndex--;
                itemSequence[itemIndex].SetActive(true);
            }
            else // 0�̸� List�� �� �ڿ� �ִ� index�� �����´�
            {
                itemSequence[itemIndex].SetActive(false);
                itemIndex = itemSequence.Count - 1;
                itemSequence[itemIndex].SetActive(true);
            }

        }
    }

    public void UseItemUI(int index)
    {
        if (index == 0)
        {
            if (enemyMonster.Hp <= (enemyMonster.MaxHp / 2))
            {
                items[index].UseItem(playerMonster, enemyMonster, this, playerInWorld);
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                StartCoroutine(CaptureCo());
                itemInven.itemCount[index]--; // ������ �κ��丮���� �������� ���� ���ش�.
                itemUseCanvas.SetActive(false);
            }
            else
                StartCoroutine(BattleOnlyMessage("���͸� ��ȭ �����ּ���!"));
        }
        else
        {
            items[index].UseItem(playerMonster);
            StartCoroutine(UseItemCo(index)); // ������ UIó��
            itemInven.itemCount[index]--; // ������ �κ��丮���� �������� ���� ���ش�.
            itemUseCanvas.SetActive(false);
        }
    }
    IEnumerator UseItemCo(int index)
    {
        battleMessages[massageDic["BattleProgress"]].SetActive(true);
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "�� " + items[index].itemName + "�� ����ߴ�!";
        yield return new WaitForSecondsRealtime(flowTime);
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "�� " + items[index].useItemText;
        yield return new WaitForSecondsRealtime(flowTime);
        battleMessages[massageDic["BattleProgress"]].SetActive(false);
        BattleStart();
    }

    IEnumerator BattleOnlyMessage(string message)
    {
        captureUI[0].transform.parent.gameObject.SetActive(true);
        captureUI[0].SetActive(true); // UI�� ���ϴܿ� �־ ��� ĵ������ ����� �� �ֱ⶧���� ���
        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text = message;
        yield return new WaitForSecondsRealtime(2);
        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text = null;
        captureUI[0].SetActive(false);
        captureUI[0].transform.parent.gameObject.SetActive(false);

    }

    public IEnumerator CaptureCo() // n�� �ڿ� �ٽ� ���� ȭ������ ���ư�
    {
        playerInWorld.captureProgress = true; // ��ȹ �ϴ� ���߿� �Ѿ��� �ٲ��� ���ϰ�
        for (int i = 0; i < playerInWorld.bullets[3].maxCount; i++) // ���� �Ѿ� ����
            enemyMonster.hormone.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
        // Ÿ�̸�
        int timer = (int)items[0].itemDuration; // �������� ���ӽð��� ������
        while (true)
        {
            enemyMonster.hormone.transform.GetComponentInChildren<TextMeshProUGUI>().text =
            "���� �ð� : " + timer;
            yield return new WaitForSeconds(1);
            timer--; // 1�ʿ� �� ���� ���� 
            if (timer == 0) // text�󿡼� ���ڰ� 0�� �Ǹ� �ش��ϴ� �ؽ�Ʈ�� ���
                enemyMonster.hormone.transform.GetComponentInChildren<TextMeshProUGUI>().text =
            "���� �ð� : �ð��ʰ�!";
            if (timer < 0 || captureState) // �ð��� �ٵǸ� ��������orȣ������ ���߸� ��������
                break;
        }
        PlayerWorld.Gobattle.Invoke(); // �ٽ� ��Ʋ�� ���ư��� ��������Ʈ ����
        enemyMonster.hormone.SetActive(false);
        playerInWorld.captureProgress = false;
    }

    void CaputreSuccess()
    {
        for (int i = 0; i < sequnceCanvas.GetChild(0).childCount; i++)
        {
            sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
            sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        enemyMonster.gameObject.SetActive(false);
        enemyMonsterUI.SetActive(false);
        captureCanvas.SetActive(true);
        captureUI[0].SetActive(true);
        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text =
        enemyMonster.monsterName + "�� ��ȹ�ߴ�!";

    }

    void CaptureMonsterShowInfo()
    {

        captureUI[0].SetActive(false);
        captureUI[1].SetActive(true);
        captureMonsterInfo[0].text = "���� �̸� : " + enemyMonster.monsterName;
        captureMonsterInfo[1].text = "�Ӽ� : " + "��";
        captureMonsterInfo[2].text = "ü��        : " + enemyMonster.maxHp +
        System.Environment.NewLine + "���ݷ�      : " + enemyMonster.att +
        System.Environment.NewLine + "����      : " + enemyMonster.def +
        System.Environment.NewLine + "Ư�� ���ݷ� : " + enemyMonster.spAtt +
        System.Environment.NewLine + "Ư�� ���� : " + enemyMonster.spDef;
        for (int i = 0; i < captureMonsterSkills.Count; i++)
            captureMonsterSkills[i].text = enemyMonster.skill.nameToKorean[enemyMonster.equipSkill[i]];
    }

    void AskChangeName()
    {
        captureUI[1].SetActive(false);
        captureUI[2].SetActive(true);
        captureUI[2].transform.GetChild(0).gameObject.SetActive(true);
        captureUI[2].transform.GetChild(1).gameObject.SetActive(true);
        captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
        "������ �̸��� ���� �Ͻðڽ��ϱ�?";

    }

    void AskChangeName2(bool yeah)
    {
        captureUI[2].transform.GetChild(0).gameObject.SetActive(false);
        captureUI[2].transform.GetChild(1).gameObject.SetActive(false);
        if (yeah) // �̸��� �ٲ۴�
        {
            captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
            "������ �̸��� �����ּ���!";
            captureUI[2].transform.GetChild(3).gameObject.SetActive(true);

        }
        else // �̸��� �ٲ��� �ʴ´�
        {
            captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
            "������ �̸��� �������� �ʽ��ϴ�.";
            StartCoroutine(CaptureCanvasOffCo(flowTime * 2)); // ��ȹ ���� UI�� ��� ������ ���� �����Ⱑ Ȱ��ȭ �ȴ�. 
        }
    }

    public void ChangeName() // ��ư Ŭ��
    {
        captureUI[2].transform.GetChild(3).gameObject.SetActive(false);
        enemyMonster.monsterName = nameInput.text; // InputField�� �Է��� ���� ���� �̸����� �ٲ��ش�.
        captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
        "������ �̸��� " + nameInput.text + "�� �ٲ��!";
        enemyMonster.monsterNameUI.text = enemyMonster.monsterName; // ���� �������� text�� �ٲ��ش�.
        nameInput.text = null;

        StartCoroutine(CaptureCanvasOffCo(flowTime * 2)); // ��ȹ ���� UI�� ��� ������ ���� �����Ⱑ Ȱ��ȭ �ȴ�. 
    }

    IEnumerator CaptureCanvasOffCo(float time)
    {
        //Debug.Log("�� 3�� ���?");
        playerMonsterUI.SetActive(false);
        yield return new WaitForSecondsRealtime(time);
        monsterInven.GetMonster(enemyMonster); // ���͸� ���� �κ��丮�� �߰� ����
        captureCanvas.SetActive(false);
        for (int i = 0; i < captureUI.Count; i++)
            captureUI[i].SetActive(false);
        // ���� ����� �Ѿ��.(��ȹ�� �����ϸ� ����ġ ȹ��, �� ȹ��, �������̳� ��ų�� ���� �� ����)
        BattleResult(enemyMonster.exp, enemyMonster.money, true);
    }

    #endregion

}
