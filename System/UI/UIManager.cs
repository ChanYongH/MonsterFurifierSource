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
    GameObject battleMessageCanvas; // 배틀 상황을 캔버스로 표시해주는 용도
    Dictionary<string, int> massageDic = new Dictionary<string, int>(); // GetChild를 가독성있게 만들어 주기 위해 사용
    List<GameObject> battleMessages = new List<GameObject>(); // 배틀 캔버스 안에 있는 버튼들을 인스턴스화 하기 위해서 사용
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
    public Image[] monsterHpUI = new Image[2]; // 플레이어/적 몬스터의 Hp
    public Image monsterExpUI; // 플레이어 몬스터의 경험치
    TextMeshProUGUI[] monsterLevel = new TextMeshProUGUI[2]; // 플레이어/적 몬스터의 레벨

    //Item
    GameObject itemUseCanvas; // UI 처리를 위해서 가져와야 함
    ItemInventory itemInven; // 가진 갯수를 가져와야 함
    GameObject itemUseList;
    List<Button> itemSelect = new List<Button>();
    List<GameObject> itemObj = new List<GameObject>();
    List<Items> items = new List<Items>();
    List<Button> useItem = new List<Button>();
    //Queue<GameObject> itemSequence = new Queue<GameObject>();
    public List<GameObject> itemSequence = new List<GameObject>();
    public int itemIndex = 0; // 아이템 수

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
        //스킬 컬러(속성에 맞게)

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

        // 처음엔 잘 적용이된다.
        // 두 번째 부턴 처음에 적용된 색깔이 계속 남아있다.
        #endregion

    }
    void BattleUI() // 전투 시작 할 때 마다 실행
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
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // 작아 질 수록 빨리 연산 됨 
        playerMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
        enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
        sequnceCanvas = GameObject.FindGameObjectWithTag("BattleSequnce").transform;
        playerInBattle = GameObject.FindGameObjectWithTag("BattlePlayer").GetComponent<PlayerBattle>();
        playerMonsterUI = GameObject.FindGameObjectWithTag("MonsterUI").transform.GetChild(0).GetChild(0).gameObject;
        enemyMonsterUI = GameObject.FindGameObjectWithTag("MonsterUI").transform.GetChild(0).GetChild(1).gameObject;
        captureCanvas = GameObject.FindGameObjectWithTag("CaptureSucceeded").transform.GetChild(0).gameObject;
        playerMonsterUI.transform.parent.gameObject.SetActive(true); //플레이어, 적 몬스터UI 활성화
        playerMonsterUI.SetActive(true);
        enemyMonsterUI.SetActive(true);
        battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManger>();
        battleMessageCanvas = GameObject.FindGameObjectWithTag("BattleMessageCanvas");
        // 한 번만 발동
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

            captureButton.Add(captureUI[0].GetComponent<Button>()); // 메세지(누구를 잡았다)
            captureButton.Add(captureUI[1].GetComponent<Button>()); // 몬스터 정보

            captureButton.Add(captureUI[2].transform.GetChild(0).GetComponent<Button>()); // 이름 바꿀래용
            captureButton.Add(captureUI[2].transform.GetChild(1).GetComponent<Button>()); // 안 바꿀래용

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
            battle[massageDic["Start"]].onClick.AddListener(() => BattleSelect(0)); // 람다 사용 
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
        monsterExpUI.fillAmount = playerMonster.exp / playerMonster.levelUpExp; // 이 설정을 안해주면 시작하자마자
                                                                                // 자기 hp나 exp에 맞춰서 천천히 증가하거나 감소함
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
        //수정
        for (int i = 0; i < skillChangeText.Length; i++)
            skillChangeText[i] = skillChangeCanvas.transform.GetChild(0).GetChild(2).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        changeSkillCanvas = GameObject.FindGameObjectWithTag("ChangeSkillCanvas2");
        changeSkillText = changeSkillCanvas.transform.GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        skillListCanvas = GameObject.FindGameObjectWithTag("SkillListCanvas");
        // 스킬이 있을 때(계속 초기화)
        if (useSkill.Count <= 0)
        {
            for (int i = 0; i < skillListCanvas.transform.GetChild(0).childCount - 1; i++)
            {
                skillListText[i] = skillListCanvas.transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
                skillListEffectText[i] = skillListCanvas.transform.GetChild(0).GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
                changeSkillListText[i] = changeSkillCanvas.transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
                useSkill.Add(skillListCanvas.transform.GetChild(0).GetChild(i).GetComponent<Button>());
            }
            // for문을 쓰면 오류가 남 왜지??? 람다라서??

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

            //스킬 리스트
            #region
            skillShowEffect.Add("NomalAttack1", "대미지 40");
            skillShowEffect.Add("NomalAttDebuff", "적 공격력 약화");
            skillShowEffect.Add("NomalAttack2", "대미지 50");
            skillShowEffect.Add("NomalDefenceUp", "방어력 증가");
            skillShowEffect.Add("NomalSpDefenceUp", "특수 방어력 증가");
            skillShowEffect.Add("NomalAttack3", "대미지 65");
            skillShowEffect.Add("NomalAttack4", "대미지 80");
            skillShowEffect.Add("NomalAgilitybuff", "날렵함 감소");
            skillShowEffect.Add("FireAttack1", "특수 대미지 40");
            skillShowEffect.Add("FireSpDefDebuff", "적 특수 방어력 약화");
            skillShowEffect.Add("FireAttack2", "특수 대미지 60");
            skillShowEffect.Add("FireSpAttBuff", "특수 공격력 증가");
            skillShowEffect.Add("FireAttack3", "특수 대미지 90(날렵함 소모량*2)");
            skillShowEffect.Add("FireSpDefBuff", "특수 방어력 증가");
            skillShowEffect.Add("FireAttack4", "특수 대미지 100");
            skillShowEffect.Add("WaterAttack1", "특수 대미지 40");
            skillShowEffect.Add("WaterAgilitybuff", "날렵함 감소");
            skillShowEffect.Add("WaterAttack2", "특수 대미지 50");
            skillShowEffect.Add("WaterSpAttbuff", "특수 대미지 증가");
            skillShowEffect.Add("WaterAttack3", "특수 대미지 60(선제타격)");
            skillShowEffect.Add("WaterEnduranceRecovery", "지구력 모두 회복");
            skillShowEffect.Add("WaterAttack4", "특수 대미지 70(날렵함 감소)");
            skillShowEffect.Add("NatureAttack1", "특수 대미지 40");
            skillShowEffect.Add("NatureHpRecovery", "체력 회복");
            skillShowEffect.Add("NatureAttack2", "특수 대미지 50");
            skillShowEffect.Add("NatureDoubleDefBuff", "방어력, 특수 방어력 증가");
            skillShowEffect.Add("NatureAttack3", "특수 대미지 40(HP회복)");
            skillShowEffect.Add("NatureSpAttBuff", "특수 대미지 대폭 증가");
            skillShowEffect.Add("NatureAttack4", "특수 대미지 80");
            skillShowEffect.Add("FighterAttack1", "대미지 60");
            skillShowEffect.Add("FighterAttBuff", "공격력 증가");
            skillShowEffect.Add("FighterAttack2", "대미지 80");
            skillShowEffect.Add("FighterDefDebuff", "방어력 감소");
            skillShowEffect.Add("FighterAttack3", "대미지 150(행동력 모두 소모)");
            skillShowEffect.Add("FighterAttack4", "대미지 100");
            skillShowEffect.Add("FighterAttDefBuff", "방어력, 특수 방어력 증가");
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

            // 뒤로가기
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
    //----------------------------------------몬스터 변경--------------------------------------------------
    #region MonsterChange
    void MonsterChangeOverrideUI()
    {
        playerMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
        skill = playerMonster.gameObject.transform.GetChild(0).GetComponent<Skills>();
    }

    public IEnumerator MonsterChangeCo()
    {
        //플레이어의 모든 몬스터의 지구력이 0보다 작으면 
        bool isLethargy = playerInBattle.monsters[0].GetComponent<Monster>().endurance <= 0 &&
        playerInBattle.monsters[1].GetComponent<Monster>().endurance <= 0 && playerInBattle.monsters[2].GetComponent<Monster>().endurance <= 0;

        yield return new WaitForSecondsRealtime(2);
        battleMessages[massageDic["BattleProgress"]].SetActive(true);
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "이(가) 의욕을 모두 잃었다!";
        yield return new WaitForSecondsRealtime(3);
        if (!isLethargy) //플레이어 몬스터의 지구력이 한 마리 이상 이라도 0보다 높으면 실행
        {
            PlayerBattle.MonsterChange.Invoke(); // 델리게이트 실행
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                "대충 몬스터가 다시 들어오는 모션 끝남";
            yield return new WaitForSecondsRealtime(2);
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                playerMonster.monsterName + "이(가) 다시 전투에 참여한다!";
            yield return new WaitForSecondsRealtime(2);
            battleMessages[massageDic["BattleProgress"]].SetActive(false);
            BattleStart();
        }
        else
        {
            BattleResult(0, 0, false); //플레이어의 모든 몬스터의 지구력이 0보다 작으면 패배
        }
    }
    #endregion
    //-----------------------------------------전투 부분---------------------------------------------------
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
        if (!first) // 처음 발동하는게 아니면(오브젝트를 전부 꺼주는 역할을 함)
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
            if (playerEndu >= enemyEndu && playerEndu > 0) // 지구력 비교
            {
                if (playerQueue.Count <= 0) // 큐가 더이상 없으면
                    break;
                playerQueue.Peek().SetActive(true);
                playerQueue.Dequeue();
                playerEndu -= playerMonster.agility; // 지구력 - 날렵함
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
    // 1번 째
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
            "포획을 실패 했다!";
            captureProgress = false;
        }
        else
            battleMessages[massageDic["OnBattle"]].
            GetComponentInChildren<TextMeshProUGUI>().text =
            " ' " + enemyMonster.monsterName + " ' " + "의 몸속으로 들어왔다!";

    }
    // (BattleFlow)2번 째 
    public void BattleStart() // 전투 시작 
    {
        StartCoroutine(SetSequnce(false)); // 배틀 시퀀스 On
        battleMessages[massageDic["OnBattle"]].SetActive(false);
        battleMessages[massageDic["Start"]].SetActive(true);
        battleMessages[massageDic["ItemUseStart"]].SetActive(true);
        BattleManger.battle = true;
    }

    // (BattleFlow)3번 째
    public void BattleSelect(int select) // 0 == 싸우기 / 1 == 아이템 사용 / 2 == 도망가기 (버튼 할당)
    {
        if (select == 0) // 전투 시작
            TurnAction();
        if (select == 1) // 아이템사용
            ItemUse();
        //if (select == 2)
        //    도망
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
        if (match) // 승리하면
        {
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                enemyMonster.monsterName + " 정화 성공!" + System.Environment.NewLine + "획득 경험치 : " + getExp.ToString()
                + System.Environment.NewLine + "획득한 돈 : " + getMoney.ToString();
            if (playerMonster.levelUp)
            {
                StartCoroutine(PlayerMonsterLevelUp());
            }
            else
                StartCoroutine(BattleEndCo());
        }
        else // 패배하면
        {
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                "정화에 실패 하였습니다!";
            StartCoroutine(BattleEndCo());
        }
    }
    // 전투 끝
    public IEnumerator BattleEndCo()
    {
        //playerMonsterUI.transform.parent.gameObject.SetActive(false); //플레이어, 적 몬스터UI 활성화

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
                "전투 나가기";

    }
    public void BattleExit()
    {
        playerMonsterUI.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // 작아 질 수록 빨리 연산 됨 
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
        for (int i = 0; i < itemSequence.Count; i++) // 아이템 순서 초기화(무조건 첫 번째 아이템이 먼저 나오게 하기위해)
            itemSequence[i].SetActive(false);
        skillListCanvas.transform.GetChild(0).gameObject.SetActive(false);
        itemUseCanvas.SetActive(false);
        BattleStart();
    }
    #endregion
    //------------------------------------몬스터(레벨업)(진화)--------------------------------------------- 
    #region PlayerMonsterEvent
    public IEnumerator PlayerMonsterLevelUp()
    {
        battleMessages[massageDic["AttackState"]].SetActive(false);
        playerMonsterUI.SetActive(false);
        enemyMonsterUI.SetActive(false);
        monsterLevel[0].text = "Lv " + playerMonster.Level;
        monsterLevel[1].text = "Lv " + enemyMonster.Level;
        yield return new WaitForSecondsRealtime(2);
        for (int i = 0; i < sequnceCanvas.GetChild(0).childCount; i++) // 전투 순서UI 끄기
        {
            sequnceCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
            sequnceCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "의 레벨이 올랐다!";
        yield return new WaitForSecondsRealtime(2);
        battleMessages[massageDic["BattleProgress"]].SetActive(false);
        battleMessages[massageDic["BattleNext"]].SetActive(true); // 해당 오브젝트를 누르면 다음으로 넘어감 
        battleMessages[massageDic["BattleNext"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "의 현재 레벨 : " + playerMonster.Level + System.Environment.NewLine + "(버튼을 누르면 다음으로 넘어갑니다.)";
        battleMessages[massageDic["PlayerLevelUp"]].SetActive(true);
        TextMeshProUGUI levelUpText = battleMessages[massageDic["PlayerLevelUp"]].GetComponentInChildren<TextMeshProUGUI>();
        levelUpText.text = playerMonster.monsterName + "의 체력 : " +
            (playerMonster.Hp - playerMonster.increase[0]) + " =>" + playerMonster.Hp + "(+" + playerMonster.increase[0] + ")";
        levelUpText.text += System.Environment.NewLine + playerMonster.monsterName + "의 공격력 : " +
            (playerMonster.att - playerMonster.increase[1]) + " =>" + playerMonster.att + "(+" + playerMonster.increase[1] + ")";
        levelUpText.text += System.Environment.NewLine + playerMonster.monsterName + "의 방어력 : " +
            (playerMonster.def - playerMonster.increase[2]) + " =>" + playerMonster.def + "(+" + playerMonster.increase[2] + ")";
        levelUpText.text += System.Environment.NewLine + playerMonster.monsterName + "의 특수 공격력 : " +
            (playerMonster.spAtt - playerMonster.increase[3]) + " =>" + playerMonster.spAtt + "(+" + playerMonster.increase[3] + ")";
        levelUpText.text += System.Environment.NewLine + playerMonster.monsterName + "의 특수 방어력 : " +
            (playerMonster.spDef - playerMonster.increase[4]) + " =>" + playerMonster.spDef + "(+" + playerMonster.increase[4] + ")";
        playerMonster.levelUp = false;
    }
    public void BattleNext() // 진화 / 스킬
    {
        battleMessages[massageDic["BattleNext"]].SetActive(false);
        if (monsterEvolution[0])
        {
            battleMessages[massageDic["BattleProgress"]].SetActive(true);
            battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            "오잉?" + playerMonster.monsterName + "의 모습이??!!";
            StartCoroutine(MonsterEvolution());
            return;
        } // 진화 할 때 
        else if (monsterEvolution[1])
        {

        }
        if (skillChange) // 스킬 변경 할게 있으면
        {
            ChangeSkillPanel();
            battleMessages[massageDic["PlayerLevelUp"]].SetActive(false);
            battleMessages[massageDic["BattleProgress"]].SetActive(false);
        }
        else // 여기에 스킬이 추가됐습니다 넣어주면 됨
        {
            if (playerMonster.getSkillState) // 스킬을 얻었다는 것을 알려주기 위해 넣은 bool값 
            {
                battleMessages[massageDic["BattleProgress"]].SetActive(true);
                battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
                   playerMonster.monsterName + "는 " + " ' " + playerMonster.skill.nameToKorean[playerMonster.skill.skillName]
                   + " ' " + "을(를) 획득했다!";
                playerMonster.getSkillState = false;
            }
            StartCoroutine(BattleEndCo());
            battleMessages[massageDic["PlayerLevelUp"]].SetActive(false);

            //battleMessages[massageDic["End"]].SetActive(true);
            //battleMessages[massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
            //    "전투 나가기";
        }
    }
    IEnumerator MonsterEvolution()
    {
        battleMessages[massageDic["PlayerLevelUp"]].SetActive(false);
        yield return new WaitForSecondsRealtime(1);

        // 진화 하는 모습 보여주면 됨.
        yield return new WaitForSecondsRealtime(6);
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "이 메가" + playerMonster.monsterName + "로 진화했다!";
        yield return new WaitForSecondsRealtime(4);
        battleMessages[massageDic["BattleProgress"]].SetActive(false);
        ChangeSkillPanel(); // 스킬 변경
    }
    #endregion
    //---------------------------------------스킬 사용 부분------------------------------------------------ 
    #region SkillUse
    // 4번 째
    public void TurnAction() // 싸우기를 누르면
    {
        skillListCanvas.transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < skillListCanvas.transform.GetChild(0).childCount - 1; i++)
            skillListCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < playerMonster.equipSkill.Count; i++) // 장착하고 있는 스킬을 보여 줌
        {
            skillListCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            skillListText[i].text = playerMonster.skill.nameToKorean[playerMonster.equipSkill[i]];
            skillListEffectText[i].text = skillShowEffect[playerMonster.equipSkill[i]];
            useSkill[i].gameObject.GetComponent<Image>().color = skillAttrib[playerMonster.equipSkill[i].
            Substring(0, 4)];
        }

    }
    // 5번 째
    public void UIUseSkill(int select) // 스킬을 선택하면
    {
        int randNum = UnityEngine.Random.Range(0, 3);
        selectSkill = playerMonster.equipSkill[select];
        if (enemyMonster.equipSkill.Count > 2)
            enemySetSkill = enemyMonster.equipSkill[randNum];
        else
            enemySetSkill = enemyMonster.equipSkill[0]; // 3개가 아닐 땐 무조건 첫 번째 공격
        if (selectSkill == "WaterAttack3") // 선제 타격 구현
            playerMonster.skill.firstAttack = true;
        skillListCanvas.transform.GetChild(0).gameObject.SetActive(false);
        battleManager.Battle();
    }
    public IEnumerator EnemyUseSkillCo(Monster monster, string monsterSkillName) // ~가 ~를 사용했다
    {
        battleMessages[massageDic["SkillUse"]].SetActive(true);
        battleMessages[massageDic["SkillUse"]].GetComponentInChildren<TextMeshProUGUI>().
            text = monster.monsterName + "은 " + monsterSkillName + "을 사용했다!";
        monster.skill.UseSkill();
        yield return new WaitForSecondsRealtime(flowTime);
        battleMessages[massageDic["SkillUse"]].SetActive(false);
    }
    public IEnumerator AttackState(bool buff, string state, string monsterName = null, string effectInfo = null) // 공격 상태
    {
        Debug.Log("AttackState In");
        yield return new WaitForSecondsRealtime(flowTime);
        Debug.Log("AttackState In2");
        battleMessages[massageDic["AttackState"]].SetActive(true);
        if (!buff)
            battleMessages[massageDic["AttackState"]].GetComponentInChildren<TextMeshProUGUI>().
                text = "효과가 " + state;
        else
            battleMessages[massageDic["AttackState"]].GetComponentInChildren<TextMeshProUGUI>().
                text = monsterName + "의 " + effectInfo;
        yield return new WaitForSecondsRealtime(flowTime);
        Debug.Log("어택스테이트 꺼지는거 발동함 ?");
        battleMessages[massageDic["AttackState"]].SetActive(false);

    }
    #endregion
    //---------------------------------------스킬 변경 부분------------------------------------------------- 
    #region SkillChange
    public void AskChangeSkill()
    {
        Debug.Log("스킬변경!");
        changeSkillCanvas.transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
            changeSkillCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        changeSkillText.text = "잊어 버릴 스킬을 지금 장착한 3개의 스킬 중에 선택 해주세요";
        for (int i = 0; i < changeSkillListText.Length; i++)
            changeSkillListText[i].text = playerMonster.skill.nameToKorean[playerMonster.equipSkill[i]];
    }
    public void ChanageSkill(int select)
    {
        for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
            changeSkillCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(false);
        changeSkillText.text = "스킬 변경을 성공했습니다!" + System.Environment.NewLine + playerMonster.skill.nameToKorean[playerMonster.equipSkill[select]]
            + System.Environment.NewLine + "▽▽▽▽▽";
        playerMonster.equipSkill.Insert(select, skill.skillName);
        playerMonster.equipSkill.RemoveAt(select + 1);
        changeSkillText.text += System.Environment.NewLine + playerMonster.skill.nameToKorean[playerMonster.equipSkill[select]];
        skill.skillInfo = false;
        battleMessages[massageDic["End"]].SetActive(true);
        battleMessages[massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
            "전투 나가기";
    }
    public void ChangeSkillPanel() // 첫 번째
    {
        skillChangeText[0].text = playerMonster.skill.nameToKorean[playerMonster.skill.skillName];
        skillChangeText[1].text = skillShowEffect[playerMonster.skill.skillName];
        skillChangeText[2].text = "이미 스킬을 3개 소유 하고 있습니다.";
        skillChangeText[2].text += System.Environment.NewLine + "이전에 배운 스킬을 잊어버리고 " + playerMonster.skill.nameToKorean[playerMonster.skill.skillName] + "을 배우시겠습니까?";
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
            skillChangeText[0].text = System.Environment.NewLine + System.Environment.NewLine + "스킬을 바꾸지않습니다.";
            skillChangeText[1].text = null;
            skillChangeText[2].text = null;
            Debug.Log("바꾸지않습니다");
            skill.skillInfo = false;
            battleMessages[massageDic["End"]].SetActive(true);
            battleMessages[massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
                "전투 나가기";
        }

    }
    #endregion

    //--------------------------------------아이템 사용 부분------------------------------------------------
    #region ItemUse

    public void ItemUse()
    {
        battleMessages[massageDic["Start"]].SetActive(false);
        battleMessages[massageDic["OnBattle"]].SetActive(false);
        battleMessages[massageDic["ItemUseStart"]].SetActive(false);
        itemUseCanvas.SetActive(true);
        // 아이템을 아무것도 가지고 있지 않으면 표시하지 않게 함 
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
        // 보유한 갯수 표시
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
        itemSequence[0].SetActive(true); // 첫 번째 아이템 활성화
    }

    public void ItemSelect(bool left)
    {
        if (!left) // 오른쪽
        {
            if (itemSequence.Count > itemIndex + 1) // 아이템을 보유한 갯수가 현재 선택된 아이템 index보다 크면 실행
            {
                itemSequence[itemIndex].SetActive(false); // 누른 후 전에 보였던 아이템 비활성화
                itemIndex++;
                itemSequence[itemIndex].SetActive(true); // 누른 후 현재 아이템 활성화
            }
            else // 아이템을 보유한 갯수가 현재 선택된 아이템 index보다 작으면 0으로 만들어 준다. 
            {
                itemSequence[itemIndex].SetActive(false);
                itemIndex = 0;
                itemSequence[itemIndex].SetActive(true);
            }
        }
        else
        {
            if (itemIndex != 0) // 현재 선택된 아이템이 0이아니면 index를 뺀다
            {
                itemSequence[itemIndex].SetActive(false);
                itemIndex--;
                itemSequence[itemIndex].SetActive(true);
            }
            else // 0이면 List의 맨 뒤에 있는 index를 가져온다
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
                itemInven.itemCount[index]--; // 아이템 인벤토리에서 아이템을 제거 해준다.
                itemUseCanvas.SetActive(false);
            }
            else
                StartCoroutine(BattleOnlyMessage("몬스터를 약화 시켜주세요!"));
        }
        else
        {
            items[index].UseItem(playerMonster);
            StartCoroutine(UseItemCo(index)); // 아이템 UI처리
            itemInven.itemCount[index]--; // 아이템 인벤토리에서 아이템을 제거 해준다.
            itemUseCanvas.SetActive(false);
        }
    }
    IEnumerator UseItemCo(int index)
    {
        battleMessages[massageDic["BattleProgress"]].SetActive(true);
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "이 " + items[index].itemName + "을 사용했다!";
        yield return new WaitForSecondsRealtime(flowTime);
        battleMessages[massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
            playerMonster.monsterName + "은 " + items[index].useItemText;
        yield return new WaitForSecondsRealtime(flowTime);
        battleMessages[massageDic["BattleProgress"]].SetActive(false);
        BattleStart();
    }

    IEnumerator BattleOnlyMessage(string message)
    {
        captureUI[0].transform.parent.gameObject.SetActive(true);
        captureUI[0].SetActive(true); // UI가 최하단에 있어서 모든 캔버스를 덮어씌울 수 있기때문에 사용
        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text = message;
        yield return new WaitForSecondsRealtime(2);
        captureUI[0].GetComponentInChildren<TextMeshProUGUI>().text = null;
        captureUI[0].SetActive(false);
        captureUI[0].transform.parent.gameObject.SetActive(false);

    }

    public IEnumerator CaptureCo() // n초 뒤에 다시 전투 화면으로 돌아감
    {
        playerInWorld.captureProgress = true; // 포획 하는 도중엔 총알을 바꾸지 못하게
        for (int i = 0; i < playerInWorld.bullets[3].maxCount; i++) // 남은 총알 갯수
            enemyMonster.hormone.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
        // 타이머
        int timer = (int)items[0].itemDuration; // 아이템의 지속시간을 가져옴
        while (true)
        {
            enemyMonster.hormone.transform.GetComponentInChildren<TextMeshProUGUI>().text =
            "남은 시간 : " + timer;
            yield return new WaitForSeconds(1);
            timer--; // 1초에 한 번씩 깎임 
            if (timer == 0) // text상에서 숫자가 0이 되면 해당하는 텍스트를 출력
                enemyMonster.hormone.transform.GetComponentInChildren<TextMeshProUGUI>().text =
            "남은 시간 : 시간초과!";
            if (timer < 0 || captureState) // 시간이 다되면 빠져나감or호르몬을 맞추면 빠져나감
                break;
        }
        PlayerWorld.Gobattle.Invoke(); // 다시 배틀로 돌아가는 델리게이트 실행
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
        enemyMonster.monsterName + "을 포획했다!";

    }

    void CaptureMonsterShowInfo()
    {

        captureUI[0].SetActive(false);
        captureUI[1].SetActive(true);
        captureMonsterInfo[0].text = "몬스터 이름 : " + enemyMonster.monsterName;
        captureMonsterInfo[1].text = "속성 : " + "불";
        captureMonsterInfo[2].text = "체력        : " + enemyMonster.maxHp +
        System.Environment.NewLine + "공격력      : " + enemyMonster.att +
        System.Environment.NewLine + "방어력      : " + enemyMonster.def +
        System.Environment.NewLine + "특수 공격력 : " + enemyMonster.spAtt +
        System.Environment.NewLine + "특수 방어력 : " + enemyMonster.spDef;
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
        "몬스터의 이름을 변경 하시겠습니까?";

    }

    void AskChangeName2(bool yeah)
    {
        captureUI[2].transform.GetChild(0).gameObject.SetActive(false);
        captureUI[2].transform.GetChild(1).gameObject.SetActive(false);
        if (yeah) // 이름을 바꾼다
        {
            captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
            "몬스터의 이름을 정해주세요!";
            captureUI[2].transform.GetChild(3).gameObject.SetActive(true);

        }
        else // 이름을 바꾸지 않는다
        {
            captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
            "몬스터의 이름을 변경하지 않습니다.";
            StartCoroutine(CaptureCanvasOffCo(flowTime * 2)); // 포획 관련 UI가 모두 꺼지고 전투 나가기가 활성화 된다. 
        }
    }

    public void ChangeName() // 버튼 클릭
    {
        captureUI[2].transform.GetChild(3).gameObject.SetActive(false);
        enemyMonster.monsterName = nameInput.text; // InputField에 입력한 값을 몬스터 이름으로 바꿔준다.
        captureUI[2].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
        "몬스터의 이름을 " + nameInput.text + "로 바꿨다!";
        enemyMonster.monsterNameUI.text = enemyMonster.monsterName; // 몬스터 아이콘의 text도 바꿔준다.
        nameInput.text = null;

        StartCoroutine(CaptureCanvasOffCo(flowTime * 2)); // 포획 관련 UI가 모두 꺼지고 전투 나가기가 활성화 된다. 
    }

    IEnumerator CaptureCanvasOffCo(float time)
    {
        //Debug.Log("왜 3번 출력?");
        playerMonsterUI.SetActive(false);
        yield return new WaitForSecondsRealtime(time);
        monsterInven.GetMonster(enemyMonster); // 몬스터를 몬스터 인벤토리에 추가 해줌
        captureCanvas.SetActive(false);
        for (int i = 0; i < captureUI.Count; i++)
            captureUI[i].SetActive(false);
        // 전투 결과로 넘어간다.(포획에 성공하면 경험치 획득, 돈 획득, 레벨업이나 스킬을 얻을 수 있음)
        BattleResult(enemyMonster.exp, enemyMonster.money, true);
    }

    #endregion

}
