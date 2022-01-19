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
//        //��ų ����Ʈ
//        #region
//        skillShowEffect.Add("NomalAttack1", "����� 40");
//        skillShowEffect.Add("NomalAttDebuff", "�� ���ݷ� ��ȭ");
//        skillShowEffect.Add("NomalAttack2", "����� 50");
//        skillShowEffect.Add("NomalDefenceUp", "���� ����");
//        skillShowEffect.Add("NomalSpDefenceUp", "Ư�� ���� ����");
//        skillShowEffect.Add("NomalAttack3", "����� 65");
//        skillShowEffect.Add("NomalAttack4", "����� 80");
//        skillShowEffect.Add("NomalAgilitybuff", "������ ����");
//        skillShowEffect.Add("FireAttack1", "Ư�� ����� 40");
//        skillShowEffect.Add("FireSpDefDebuff", "�� Ư�� ���� ��ȭ");
//        skillShowEffect.Add("FireAttack2", "Ư�� ����� 60");
//        skillShowEffect.Add("FireSpAttBuff", "Ư�� ���ݷ� ����");
//        skillShowEffect.Add("FireAttack3", "Ư�� ����� 90(������ �Ҹ�*2)");
//        skillShowEffect.Add("FireSpDefBuff", "Ư�� ���� ����");
//        skillShowEffect.Add("FireAttack4", "Ư�� ����� 100");
//        skillShowEffect.Add("WaterAttack1", "Ư�� ����� 40");
//        skillShowEffect.Add("WaterAgilitybuff", "������ ����");
//        skillShowEffect.Add("WaterAttack2", "Ư�� ����� 50");
//        skillShowEffect.Add("WaterSpAttbuff", "Ư�� ����� ����");
//        skillShowEffect.Add("WaterAttack3", "Ư�� ����� 60(����Ÿ��)");
//        skillShowEffect.Add("WaterEnduranceRecovery", "������ ��� ȸ��");
//        skillShowEffect.Add("WaterAttack4", "Ư�� ����� 70(������ ����)");
//        skillShowEffect.Add("NatureAttack1", "Ư�� ����� 40");
//        skillShowEffect.Add("NatureHpRecovery", "ü�� ȸ��");
//        skillShowEffect.Add("NatureAttack2", "Ư�� ����� 50");
//        skillShowEffect.Add("NatureDoubleDefBuff", "����, Ư�� ���� ����");
//        skillShowEffect.Add("NatureAttack3", "Ư�� ����� 40(HPȸ��)");
//        skillShowEffect.Add("NatureSpAttBuff", "Ư�� ����� ���� ����");
//        skillShowEffect.Add("NatureAttack4", "Ư�� ����� 80");
//        skillShowEffect.Add("FighterAttack1", "����� 60");
//        skillShowEffect.Add("FighterAttBuff", "���ݷ� ����");
//        skillShowEffect.Add("FighterAttack2", "����� 80");
//        skillShowEffect.Add("FighterDefDebuff", "���� ����");
//        skillShowEffect.Add("FighterAttack3", "����� 150(�ൿ�� ��� �Ҹ�)");
//        skillShowEffect.Add("FighterAttack4", "����� 100");
//        skillShowEffect.Add("FighterAttDefBuff", "����, Ư�� ���� ����");
//        #endregion
//        #region SkillColors
//        //��ų �÷�(�Ӽ��� �°�)

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

//        // ó���� �� �����̵ȴ�.
//        // �� ��° ���� ó���� ����� ������ ��� �����ִ�.
//        #endregion        
//        //��ų UI �ʱ�ȭ
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
//        // ��ų�� ���� ��(��� �ʱ�ȭ)
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

//        //---------------------------------------��ų ��� �κ�--------------------------------------------- 
//        #region SkillUse
//        // 4�� °
//    public void TurnAction() // �ο�⸦ ������
//    {
//        skillListCanvas.transform.GetChild(0).gameObject.SetActive(true);
//        for (int i = 0; i < skillListCanvas.transform.GetChild(0).childCount - 1; i++)
//            skillListCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
//        for (int i = 0; i < uiManager.playerMonster.equipSkill.Count; i++) // �����ϰ� �ִ� ��ų�� ���� ��
//        {
//            skillListCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
//            skillListText[i].text = uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.equipSkill[i]];
//            skillListEffectText[i].text = skillShowEffect[uiManager.playerMonster.equipSkill[i]];
//            useSkill[i].gameObject.GetComponent<Image>().color = skillAttrib[uiManager.playerMonster.equipSkill[i].
//            Substring(0, 4)];
//        }

//    }
//    // 5�� °
//    public void UIUseSkill(int select) // ��ų�� �����ϸ�
//    {
//        int randNum = UnityEngine.Random.Range(0, 3);
//        selectSkill = uiManager.playerMonster.equipSkill[select];
//        if (uiManager.enemyMonster.equipSkill.Count > 2)
//            enemySetSkill = uiManager.enemyMonster.equipSkill[randNum];
//        else
//            enemySetSkill = uiManager.enemyMonster.equipSkill[0]; // 3���� �ƴ� �� ������ ù ��° ����
//        if (selectSkill == "WaterAttack3") // ���� Ÿ�� ����
//            uiManager.playerMonster.skill.firstAttack = true;
//        skillListCanvas.transform.GetChild(0).gameObject.SetActive(false);
//        battleManager.Battle();
//    }
//    public IEnumerator EnemyUseSkillCo(string monsterName, string skillName) // ~�� ~�� ����ߴ�
//    {
//        battleManager.battleMessages[battleManager.massageDic["SkillUse"]].SetActive(true);
//        battleManager.battleMessages[battleManager.massageDic["SkillUse"]].GetComponentInChildren<TextMeshProUGUI>().
//            text = monsterName + "�� " + skillName + "�� ����ߴ�!";
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        battleManager.battleMessages[battleManager.massageDic["SkillUse"]].SetActive(false);
//    }
//    public IEnumerator AttackState(bool buff, string state, string monsterName = null, string effectInfo = null) // ���� ����
//    {
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        battleManager.battleMessages[battleManager.massageDic["AttackState"]].SetActive(true);
//        if (!buff)
//            battleManager.battleMessages[battleManager.massageDic["AttackState"]].GetComponentInChildren<TextMeshProUGUI>().
//                text = "ȿ���� " + state;
//        else
//            battleManager.battleMessages[battleManager.massageDic["AttackState"]].GetComponentInChildren<TextMeshProUGUI>().
//                text = monsterName + "�� " + effectInfo;
//        yield return new WaitForSecondsRealtime(uiManager.flowTime);
//        Debug.Log("���ý�����Ʈ �����°� �ߵ��� ?");
//        battleManager.battleMessages[battleManager.massageDic["AttackState"]].SetActive(false);

//    }
//    #endregion
//    //---------------------------------------��ų ���� �κ�--------------------------------------------- 
//    #region SkillChange
//    public void AskChangeSkill()
//    {
//        Debug.Log("��ų����!");
//        changeSkillCanvas.transform.GetChild(0).gameObject.SetActive(true);
//        for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
//            changeSkillCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
//        changeSkillText.text = "�ؾ� ���� ��ų�� ���� ������ 3���� ��ų �߿� ���� ���ּ���";
//        for (int i = 0; i < changeSkillListText.Length; i++)
//            changeSkillListText[i].text = uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.equipSkill[i]];
//    }
//    public void ChanageSkill(int select)
//    {
//        for (int i = 0; i < changeSkillCanvas.transform.GetChild(0).childCount - 1; i++)
//            changeSkillCanvas.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
//        skillChangeCanvas.transform.GetChild(0).gameObject.SetActive(false);
//        changeSkillText.text = "��ų ������ �����߽��ϴ�!" + System.Environment.NewLine + uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.equipSkill[select]]
//            + System.Environment.NewLine + "������";
//        uiManager.playerMonster.equipSkill.Insert(select, uiManager.playerSkills.skillName);
//        uiManager.playerMonster.equipSkill.RemoveAt(select + 1);
//        changeSkillText.text += System.Environment.NewLine + uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.equipSkill[select]];
//        uiManager.playerSkills.skillInfo = false;
//        battleManager.battleMessages[battleManager.massageDic["End"]].SetActive(true);
//        battleManager.battleMessages[battleManager.massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            "���� ������";
//    }
//    public void ChangeSkillPanel() // ù ��°
//    {
//        skillChangeText[0].text = uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.skill.skillName];
//        skillChangeText[1].text = skillShowEffect[uiManager.playerMonster.skill.skillName];
//        skillChangeText[2].text = "�̹� ��ų�� 3�� ���� �ϰ� �ֽ��ϴ�.";
//        skillChangeText[2].text += System.Environment.NewLine + "������ ��� ��ų�� �ؾ������ " + uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.skill.skillName] + "�� ���ðڽ��ϱ�?";
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
//            skillChangeText[0].text = System.Environment.NewLine + System.Environment.NewLine + "��ų�� �ٲ����ʽ��ϴ�.";
//            skillChangeText[1].text = null;
//            skillChangeText[2].text = null;
//            Debug.Log("�ٲ����ʽ��ϴ�");
//            uiManager.playerSkills.skillInfo = false;
//            battleManager.battleMessages[battleManager.massageDic["End"]].SetActive(true);
//            battleManager.battleMessages[battleManager.massageDic["End"]].GetComponentInChildren<TextMeshProUGUI>().text =
//                "���� ������";
//        }

//    }
//    #endregion

//    //------------------------------------����(������)(��ȭ)--------------------------------------------- 
//    #region PlayerMonsterEvent
//    public IEnumerator PlayerMonsterLevelUp()
//    {
//        yield return new WaitForSecondsRealtime(2);
//        battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "�� ������ �ö���!";
//        yield return new WaitForSecondsRealtime(2);
//        battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(false);
//        battleManager.battleMessages[battleManager.massageDic["BattleNext"]].SetActive(true); // �ش� ������Ʈ�� ������ �������� �Ѿ 
//        battleManager.battleMessages[battleManager.massageDic["BattleNext"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "�� ���� ���� : " + uiManager.playerMonster.Level + System.Environment.NewLine + "(��ư�� ������ �������� �Ѿ�ϴ�.)";
//        battleManager.battleMessages[battleManager.massageDic["PlayerLevelUp"]].SetActive(true);
//        TextMeshProUGUI levelUpText = battleManager.battleMessages[battleManager.massageDic["PlayerLevelUp"]].GetComponentInChildren<TextMeshProUGUI>();
//        levelUpText.text = uiManager.playerMonster.monsterName + "�� ü�� : " +
//            (uiManager.playerMonster.Hp - uiManager.playerMonster.Increase[0]) + " =>" + uiManager.playerMonster.Hp + "(+" + uiManager.playerMonster.Increase[0] + ")";
//        levelUpText.text += System.Environment.NewLine + uiManager.playerMonster.monsterName + "�� ���ݷ� : " +
//            (uiManager.playerMonster.att - uiManager.playerMonster.Increase[1]) + " =>" + uiManager.playerMonster.att + "(+" + uiManager.playerMonster.Increase[1] + ")";
//        levelUpText.text += System.Environment.NewLine + uiManager.playerMonster.monsterName + "�� ���� : " +
//            (uiManager.playerMonster.def - uiManager.playerMonster.Increase[2]) + " =>" + uiManager.playerMonster.def + "(+" + uiManager.playerMonster.Increase[2] + ")";
//        levelUpText.text += System.Environment.NewLine + uiManager.playerMonster.monsterName + "�� Ư�� ���ݷ� : " +
//            (uiManager.playerMonster.spAtt - uiManager.playerMonster.Increase[3]) + " =>" + uiManager.playerMonster.spAtt + "(+" + uiManager.playerMonster.Increase[3] + ")";
//        levelUpText.text += System.Environment.NewLine + uiManager.playerMonster.monsterName + "�� Ư�� ���� : " +
//            (uiManager.playerMonster.spDef - uiManager.playerMonster.Increase[4]) + " =>" + uiManager.playerMonster.spDef + "(+" + uiManager.playerMonster.Increase[4] + ")";
//        uiManager.playerMonster.levelUp = false;
//    }
//    public void BattleNext() // ��ȭ / ��ų
//    {
//        battleManager.battleMessages[battleManager.massageDic["BattleNext"]].SetActive(false);
//        if (monsterEvolution[0])
//        {
//            battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(true);
//            battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            "����?" + uiManager.playerMonster.monsterName + "�� �����??!!";
//            StartCoroutine(MonsterEvolution());
//            return;
//        } // ��ȭ �� �� 
//        else if (monsterEvolution[1])
//        {

//        }
//        if (skillChange) // ��ų ���� �Ұ� ������
//        {
//            ChangeSkillPanel();
//            battleManager.battleMessages[battleManager.massageDic["PlayerLevelUp"]].SetActive(false);
//            battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(false);
//        }
//        else // ���⿡ ��ų�� �߰��ƽ��ϴ� �־��ָ� ��
//        {
//            if (uiManager.playerMonster.getSkillState) // ��ų�� ����ٴ� ���� �˷��ֱ� ���� ���� bool�� 
//            {
//                battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(true);
//                battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//                   uiManager.playerMonster.monsterName + "�� " + " ' " + uiManager.playerMonster.skill.nameToKorean[uiManager.playerMonster.skill.skillName]
//                   + " ' " + "��(��) ȹ���ߴ�!";
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

//        // ��ȭ �ϴ� ��� �����ָ� ��.
//        yield return new WaitForSecondsRealtime(6);
//        battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].GetComponentInChildren<TextMeshProUGUI>().text =
//            uiManager.playerMonster.monsterName + "�� �ް�" + uiManager.playerMonster.monsterName + "�� ��ȭ�ߴ�!";
//        yield return new WaitForSecondsRealtime(4);
//        battleManager.battleMessages[battleManager.massageDic["BattleProgress"]].SetActive(false);
//        ChangeSkillPanel(); // ��ų ����
//    }
//    #endregion

//}
