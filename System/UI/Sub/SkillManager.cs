//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using UnityEngine.UI;

//public class SkillManager : MonoBehaviour
//{
//    UIManager uiManager;
//    BattleManger battleManager;

//    GameObject skillChangeCanvas;
//    TextMeshProUGUI[] skillChangeText = new TextMeshProUGUI[3];
//    GameObject changeSkillCanvas;
//    TextMeshProUGUI changeSkillText;
//    TextMeshProUGUI[] changeSkillListText = new TextMeshProUGUI[3];
//    GameObject skillListCanvas;
//    TextMeshProUGUI[] skillListText = new TextMeshProUGUI[3];
//    TextMeshProUGUI[] skillListEffectText = new TextMeshProUGUI[3];
//    public Dictionary<string, string> skillShowEffect = new Dictionary<string, string>();
//    public Dictionary<string, Color> skillAttrib = new Dictionary<string, Color>();
//    public string selectSkill;
//    public string enemySetSkill;

//    List<Button> useSkill = new List<Button>();
//    List<Button> changeSkillAsk = new List<Button>();
//    List<Button> changeSkill = new List<Button>();
//    public bool skillChange = false;

//    public bool[] monsterEvolution = new bool[2];
//    // Start is called before the first frame update
//    void Start()
//    {
//        PlayerWorld.Gobattle += StartSkillEvent;
//        PlayerWorld.battleOut += EndSkillEvent;
//        uiManager = FindObjectOfType<UIManager>();
//        battleManager = FindObjectOfType<BattleManger>();
//        //스킬 리스트
//        #region
//        skillShowEffect.Add("NomalAttack1", "대미지 40");
//        skillShowEffect.Add("NomalAttDebuff", "적 공격력 약화");
//        skillShowEffect.Add("NomalAttack2", "대미지 50");
//        skillShowEffect.Add("NomalDefenceUp", "방어력 증가");
//        skillShowEffect.Add("NomalSpDefenceUp", "특수 방어력 증가");
//        skillShowEffect.Add("NomalAttack3", "대미지 65");
//        skillShowEffect.Add("NomalAttack4", "대미지 80");
//        skillShowEffect.Add("NomalAgilitybuff", "날렵함 감소");
//        skillShowEffect.Add("FireAttack1", "특수 대미지 40");
//        skillShowEffect.Add("FireSpDefDebuff", "적 특수 방어력 약화");
//        skillShowEffect.Add("FireAttack2", "특수 대미지 60");
//        skillShowEffect.Add("FireSpAttBuff", "특수 공격력 증가");
//        skillShowEffect.Add("FireAttack3", "특수 대미지 90(날렵함 소모량*2)");
//        skillShowEffect.Add("FireSpDefBuff", "특수 방어력 증가");
//        skillShowEffect.Add("FireAttack4", "특수 대미지 100");
//        skillShowEffect.Add("WaterAttack1", "특수 대미지 40");
//        skillShowEffect.Add("WaterAgilitybuff", "날렵함 감소");
//        skillShowEffect.Add("WaterAttack2", "특수 대미지 50");
//        skillShowEffect.Add("WaterSpAttbuff", "특수 대미지 증가");
//        skillShowEffect.Add("WaterAttack3", "특수 대미지 60(선제타격)");
//        skillShowEffect.Add("WaterEnduranceRecovery", "지구력 모두 회복");
//        skillShowEffect.Add("WaterAttack4", "특수 대미지 70(날렵함 감소)");
//        skillShowEffect.Add("NatureAttack1", "특수 대미지 40");
//        skillShowEffect.Add("NatureHpRecovery", "체력 회복");
//        skillShowEffect.Add("NatureAttack2", "특수 대미지 50");
//        skillShowEffect.Add("NatureDoubleDefBuff", "방어력, 특수 방어력 증가");
//        skillShowEffect.Add("NatureAttack3", "특수 대미지 40(HP회복)");
//        skillShowEffect.Add("NatureSpAttBuff", "특수 대미지 대폭 증가");
//        skillShowEffect.Add("NatureAttack4", "특수 대미지 80");
//        skillShowEffect.Add("FighterAttack1", "대미지 60");
//        skillShowEffect.Add("FighterAttBuff", "공격력 증가");
//        skillShowEffect.Add("FighterAttack2", "대미지 80");
//        skillShowEffect.Add("FighterDefDebuff", "방어력 감소");
//        skillShowEffect.Add("FighterAttack3", "대미지 150(행동력 모두 소모)");
//        skillShowEffect.Add("FighterAttack4", "대미지 100");
//        skillShowEffect.Add("FighterAttDefBuff", "방어력, 특수 방어력 증가");
//        #endregion
//        #region SkillColors
//        //스킬 컬러(속성에 맞게)

//        Color redColor;
//        ColorUtility.TryParseHtmlString("#F08080", out redColor);
//        Color greenColor;
//        ColorUtility.TryParseHtmlString("#ADFF2F", out greenColor);
//        Color blueColor;
//        ColorUtility.TryParseHtmlString("#87CEFA", out blueColor);
//        Color brownColor;
//        ColorUtility.TryParseHtmlString("#D2B48C", out brownColor);

//        skillAttrib.Add("Noma", Color.gray);
//        skillAttrib.Add("Fire", redColor);
//        skillAttrib.Add("Wate", blueColor);
//        skillAttrib.Add("Natu", greenColor);
//        skillAttrib.Add("Figh", brownColor);

//        // 처음엔 잘 적용이된다.
//        // 두 번째 부턴 처음에 적용된 색깔이 계속 남아있다.
//        #endregion        
//        //스킬 UI 초기화
//    }

//    void StartSkillEvent()
//    {
//        skillChangeCanvas = GameObject.FindGameObjectWithTag("ChangeSkillCanvas");
//        for (int i = 0; i < skillChangeText.Length; i++)
//            skillChangeText[i] = skillChangeCanvas.transform.GetChild(0).GetChild(2).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
//        changeSkillCanvas = GameObject.FindGameObjectWithTag("ChangeSkillCanvas2");
//        changeSkillText = changeSkillCanvas.transform.GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
//        skillListCanvas = GameObject.FindGameObjectWithTag("SkillListCanvas");
//        #region Skills
//        // 스킬이 있을 때(계속 초기화)
//        if (useSkill.Count <= 0)
//        {
//            for (int i = 0; i < skillListCanvas.transform.GetChild(0).childCount - 1; i++)
//            {
//                skillListText[i] = skillListCanvas.transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
//                skillListEffectText[i] = skillListCanvas.transform.GetChild(0).GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
//                changeSkillListText[i] = changeSkillCanvas.transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
//                useSkill.Add(skillListCanvas.transform.GetChild(0).GetChild(i).GetComponent<Button>());
//            }
//            useSkill[0].onClick.AddListener(() => UIUseSkill(0));
//            useSkill[1].onClick.AddListener(() => UIUseSkill(1));
//            useSkill[2].onClick.AddListener(() => UIUseSkill(2));

//            changeSkillAsk.Add(skillChangeCanvas.transform.GetChild(0).GetChild(0).GetComponent<Button>());
//            changeSkillAsk.Add(skillChangeCanvas.transform.GetChild(0).GetChild(1).GetComponent<Button>());
//            changeSkillAsk[0].onClick.AddListener(() => ChangeSkillBool(true));
//            changeSkillAsk[1].onClick.AddListener(() => ChangeSkillBool(false));
//            for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
//                changeSkill.Add(changeSkillCanvas.transform.GetChild(0).GetChild(i).GetComponent<Button>());
//            changeSkill[0].onClick.AddListener(() => ChanageSkill(0));
//            changeSkill[1].onClick.AddListener(() => ChanageSkill(1));
//            changeSkill[2].onClick.AddListener(() => ChanageSkill(2));
//            #endregion
//        }
//    }

//    void EndSkillEvent()
//    {
//        skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(false);
//        changeSkillCanvas.transform.GetChild(0).gameObject.SetActive(false);
//    }

//        //---------------------------------------스킬 사용 부분--------------------------------------------- 
//        #region SkillUse
//        // 4번 째
//    public void TurnAction() // 싸우기를 누르면
//    {
//        skillListCanvas.transform.GetChild(0).gameObject.SetActive(true);
//        for (int i = 0; i < skillListCanvas.transform.GetChild(0).childCount - 1; i++)
//            skillListCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
//        for (int i = 0; i < uiManager.playerMonster.equipSkill.Count; i++) // 장착하고 있는 스킬을 보여 줌
//        {
//            skillListCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
//            skillListText[i].text = uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.equipSkill[i]];
//            skillListEffectText[i].text = skillShowEffect[uiManager.playerMonster.equipSkill[i]];
//            useSkill[i].gameObject.GetComponent<Image>().color = skillAttrib[uiManager.playerMonster.equipSkill[i].
//            Substring(0, 4)];
//        }

//    }
//    // 5번 째
//    public void UIUseSkill(int select) // 스킬을 선택하면
//    {
//        int randNum = UnityEngine.Random.Range(0, 3);
//        selectSkill = uiManager.playerMonster.equipSkill[select];
//        if (uiManager.enemyMonster.equipSkill.Count > 2)
//            enemySetSkill = uiManager.enemyMonster.equipSkill[randNum];
//        else
//            enemySetSkill = uiManager.enemyMonster.equipSkill[0]; // 3개가 아닐 땐 무조건 첫 번째 공격
//        if (selectSkill == "WaterAttack3") // 선제 타격 구현
//            uiManager.playerMonster.skill.firstAttack = true;
//        skillListCanvas.transform.GetChild(0).gameObject.SetActive(false);
//        battleManager.Battle();
//    }
//    public IEnumerator EnemyUseSkillCo(string monsterName, string skillName) // ~가 ~를 사용했다
//    {
//        battleManager.battleMessages[battleManager.massageDic["SkillUse"]].SetActive(true);
//        battleManager.battleMessages[battleManager.massageDic["SkillUse"]].GetComponentInChildren<TextMeshProUGUI>().
//            text = monsterName + "은 " + skillName + "을 사용했다!";
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        battleManager.battleMessages[battleManager.massageDic["SkillUse"]].SetActive(false);
//    }
//    public IEnumerator AttackState(bool buff, string state, string monsterName = null, string effectInfo = null) // 공격 상태
//    {
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        battleManager.battleMessages[battleManager.massageDic["AttackState"]].SetActive(true);
//        if (!buff)
//            battleManager.battleMessages[battleManager.massageDic["AttackState"]].GetComponentInChildren<TextMeshProUGUI>().
//                text = "효과가 " + state;
//        else
//            battleManager.battleMessages[battleManager.massageDic["AttackState"]].GetComponentInChildren<TextMeshProUGUI>().
//                text = monsterName + "의 " + effectInfo;
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        Debug.Log("어택스테이트 꺼지는거 발동함 ?");
//        battleManager.battleMessages[battleManager.massageDic["AttackState"]].SetActive(false);

//    }
//    #endregion
//    //---------------------------------------스킬 변경 부분--------------------------------------------- 
//    #region SkillChange
//    public void AskChangeSkill()
//    {
//        Debug.Log("스킬변경!");
//        changeSkillCanvas.transform.GetChild(0).gameObject.SetActive(true);
//        for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
//            changeSkillCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
//        changeSkillText.text = "잊어 버릴 스킬을 지금 장착한 3개의 스킬 중에 선택 해주세요";
//        for (int i = 0; i < changeSkillListText.Length; i++)
//            changeSkillListText[i].text = uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.equipSkill[i]];
//    }
//    public void ChanageSkill(int select)
//    {
//        for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
//            changeSkillCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
//        skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(false);
//        changeSkillText.text = "스킬 변경을 성공했습니다!" + System.Environment.NewLine + uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.equipSkill[select]]
//            + System.Environment.NewLine + "▽▽▽▽▽";
//        uiManager.playerMonster.equipSkill.Insert(select, uiManager.playerSkills.skillName);
//        uiManager.playerMonster.equipSkill.RemoveAt(select + 1);
//        changeSkillText.text += System.Environment.NewLine + uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.equipSkill[select]];
//        uiManager.playerSkills.skillInfo = false;
//        battleManager.battleMessages[battleManager.massageDic["End"]].SetActive(true);
//        battleManager.battleMessages[battleManager.massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            "전투 나가기";
//    }
//    public void ChangeSkillPanel() // 첫 번째
//    {
//        skillChangeText[0].text = uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.skill.skillName];
//        skillChangeText[1].text = skillShowEffect[uiManager.playerMonster.skill.skillName];
//        skillChangeText[2].text = "이미 스킬을 3개 소유 하고 있습니다.";
//        skillChangeText[2].text += System.Environment.NewLine + "이전에 배운 스킬을 잊어버리고 " + uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.skill.skillName] + "을 배우시겠습니까?";
//        skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(true);
//        skillChangeCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
//        skillChangeCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
//        skillChange = false;
//    }
//    public void ChangeSkillBool(bool select)
//    {
//        if (select)
//        {
//            AskChangeSkill();
//            skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(false);
//        }
//        else
//        {
//            skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(true);
//            skillChangeCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
//            skillChangeCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
//            skillChangeText[0].text = System.Environment.NewLine + System.Environment.NewLine + "스킬을 바꾸지않습니다.";
//            skillChangeText[1].text = null;
//            skillChangeText[2].text = null;
//            Debug.Log("바꾸지않습니다");
//            uiManager.playerSkills.skillInfo = false;
//            battleManager.battleMessages[battleManager.massageDic["End"]].SetActive(true);
//            battleManager.battleMessages[battleManager.massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
//                "전투 나가기";
//        }

//    }
//    #endregion

//    //------------------------------------몬스터(레벨업)(진화)--------------------------------------------- 
//    #region PlayerMonsterEvent
//    public IEnumerator PlayerMonsterLevelUp()
//    {
//        yield return new WaitForSecondsRealtime(2);
//        battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "의 레벨이 올랐다!";
//        yield return new WaitForSecondsRealtime(2);
//        battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(false);
//        battleManager.battleMessages[battleManager.massageDic["BattleNext"]].SetActive(true); // 해당 오브젝트를 누르면 다음으로 넘어감 
//        battleManager.battleMessages[battleManager.massageDic["BattleNext"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "의 현재 레벨 : " + uiManager.playerMonster.Level + System.Environment.NewLine + "(버튼을 누르면 다음으로 넘어갑니다.)";
//        battleManager.battleMessages[battleManager.massageDic["PlayerLevelUp"]].SetActive(true);
//        TextMeshProUGUI levelUpText = battleManager.battleMessages[battleManager.massageDic["PlayerLevelUp"]].GetComponentInChildren<TextMeshProUGUI>();
//        levelUpText.text = uiManager.playerMonster.monsterName + "의 체력 : " +
//            (uiManager.playerMonster.Hp - uiManager.playerMonster.Increase[0]) + " =>" + uiManager.playerMonster.Hp + "(+" + uiManager.playerMonster.Increase[0] + ")";
//        levelUpText.text += System.Environment.NewLine + uiManager.playerMonster.monsterName + "의 공격력 : " +
//            (uiManager.playerMonster.att - uiManager.playerMonster.Increase[1]) + " =>" + uiManager.playerMonster.att + "(+" + uiManager.playerMonster.Increase[1] + ")";
//        levelUpText.text += System.Environment.NewLine + uiManager.playerMonster.monsterName + "의 방어력 : " +
//            (uiManager.playerMonster.def - uiManager.playerMonster.Increase[2]) + " =>" + uiManager.playerMonster.def + "(+" + uiManager.playerMonster.Increase[2] + ")";
//        levelUpText.text += System.Environment.NewLine + uiManager.playerMonster.monsterName + "의 특수 공격력 : " +
//            (uiManager.playerMonster.spAtt - uiManager.playerMonster.Increase[3]) + " =>" + uiManager.playerMonster.spAtt + "(+" + uiManager.playerMonster.Increase[3] + ")";
//        levelUpText.text += System.Environment.NewLine + uiManager.playerMonster.monsterName + "의 특수 방어력 : " +
//            (uiManager.playerMonster.spDef - uiManager.playerMonster.Increase[4]) + " =>" + uiManager.playerMonster.spDef + "(+" + uiManager.playerMonster.Increase[4] + ")";
//        uiManager.playerMonster.levelUp = false;
//    }
//    public void BattleNext() // 진화 / 스킬
//    {
//        battleManager.battleMessages[battleManager.massageDic["BattleNext"]].SetActive(false);
//        if (monsterEvolution[0])
//        {
//            battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(true);
//            battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            "오잉?" + uiManager.playerMonster.monsterName + "의 모습이??!!";
//            StartCoroutine(MonsterEvolution());
//            return;
//        } // 진화 할 때 
//        else if (monsterEvolution[1])
//        {

//        }
//        if (skillChange) // 스킬 변경 할게 있으면
//        {
//            ChangeSkillPanel();
//            battleManager.battleMessages[battleManager.massageDic["PlayerLevelUp"]].SetActive(false);
//            battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(false);
//        }
//        else // 여기에 스킬이 추가됐습니다 넣어주면 됨
//        {
//            if (uiManager.playerMonster.getSkillState) // 스킬을 얻었다는 것을 알려주기 위해 넣은 bool값 
//            {
//                battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(true);
//                battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//                   uiManager.playerMonster.monsterName + "는 " + " ' " + uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.skill.skillName]
//                   + " ' " + "을(를) 획득했다!";
//                uiManager.playerMonster.getSkillState = false;
//            }
//            StartCoroutine(battleManager.BattleEndCo());
//            battleManager.battleMessages[battleManager.massageDic["PlayerLevelUp"]].SetActive(false);
//        }
//    }
//    IEnumerator MonsterEvolution()
//    {
//        battleManager.battleMessages[battleManager.massageDic["PlayerLevelUp"]].SetActive(false);
//        yield return new WaitForSecondsRealtime(1);

//        // 진화 하는 모습 보여주면 됨.
//        yield return new WaitForSecondsRealtime(6);
//        battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "이 메가" + uiManager.playerMonster.monsterName + "로 진화했다!";
//        yield return new WaitForSecondsRealtime(4);
//        battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(false);
//        ChangeSkillPanel(); // 스킬 변경
//    }
//    #endregion

//}
