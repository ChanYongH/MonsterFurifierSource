using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Reflection;

public class Monster : Character
{
    [SerializeField]
    GameObject player;
    public Transform playerWorldPosOrigin;
    public GameObject playerWorldPos;
    PlayerWorld playerInWorld;
    PlayerBattle playerInBattle;
    protected UIManager uiManager;
    public GameObject monsterWorldPos;
    public GameObject hormone;
    public TextMeshProUGUI monsterNameUI;

    public string monsterName;
    public bool isDead = false;
    public int level;
    public float exp;
    public int personality;
    public float att;
    public float def;
    public float spAtt;
    public float spDef;
    public int maxEndurance;
    public int endurance;
    public int agility;
    public string monsterExplain;
    public bool inBattle = false;
    public bool levelUp = false;
    

    public float[] increase = new float[5];

    //����
    public float levelUpExp = 0.001f; // ���Ǹ� �����ָ� fillamount���� NAN(���ڰ� �ƴ�)������ �߻�

    //��ȭ
    public bool[] evolution = new bool[2];

    public bool playerMonster = false;

    //��ų
    public Skills skill;
    public List<string> equipSkill = new List<string>();
    public bool getSkillState = false; // ��ų�� ����ٴ� ���� �˷��ֱ� ���� ���� bool�� 
                                       // UI���� ó���ϱ� ���ؼ� ����
    public Dictionary<int, string> getSkill = new Dictionary<int, string>();

    //�ִϸ��̼� / ����
    public Animator ani;

    public float Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;

            if (Level.Between(0, 3))
            {
                levelUpExp = (Level * 10 + 7) * 2;
                while (exp >= levelUpExp)
                {
                    exp -= (Level * 10 + 7) * 2;
                    Level++;
                }
            }
            else if(Level.Between(4, 5))
            {
                levelUpExp = (Level * 10 + 7) * 2;
                while (exp >= (Level * 10 + 7) * 2)
                {
                    exp -= (Level * 10 + 7) * 2;
                    Level++;
                }
            }
            else if (Level.Between(6, 9))
            {
                levelUpExp = (Level * 10 + 7) * 3;
                while (exp >= (Level * 10 + 7) *3)
                {
                    exp -= (Level * 10 + 7) * 3;
                    Level++;
                }
            }
            else if (Level.Between(10,15))
            {
                levelUpExp = (Level * 10 + 7) * 3;
                while (exp >= (Level * 10 + 7) * 3)
                {
                    exp -= (Level * 10 + 7) * 3;
                    Level++;
                }
            }
            else if (Level.Between(16, 20))
            {
                levelUpExp = (Level * 10 + 7) * 4;
                while (exp >= (Level * 10 + 7) * 4)
                {
                    exp -= (Level * 10 + 7) * 4;
                    Level++;
                }
            }
            else if (Level.Between(21, 25))
            {
                levelUpExp = (Level * 10 + 7) * 5;
                while (exp >= (Level * 10 + 7) * 5)
                {
                    exp -= (Level * 10 + 7) * 5;
                    Level++;
                }
            }
        }
    }
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            Debug.Log("������!");
            MonsterLevelUp();
            if (playerMonster)
            {
                bool isGetSkill = level == 1 || level == 3 || level == 5 || level == 6 || level == 8 ||
                                  level == 9 || level == 11 || level == 12 || level == 14 ||
                                  level == 17 || level == 18 || level == 19 || level == 20 || level == 22;
                if (isGetSkill)
                {
                    skill.AddSkill(getSkill[level]);
                    getSkillState = true; // ���� �� ������ ����
                }

                if (level == 11 && evolution[0]) // ��ȭ
                    Evolution(false); // 1�� ��ȭ
                if (level == 22 && evolution[1])
                    Evolution(true); // 2�� ��ȭ
            }
        }
    }

    #region CreateSkills
    public void Awake()
    {
        transform.GetChild(0).gameObject.AddComponent(Type.GetType
        (this.GetType().Name.Substring(0, this.GetType().Name.IndexOf('M')) + "Skills"));
        // �� ���Ϳ� �ִ� ������Ʈ�� �̸�(M������ ���ڿ�)���� �����ͼ� Skills�� ���ڿ���
        // ��ģ �� �� ��ģ �̸��� ������Ʈ�� �߰� ���ش�.
    }
    #endregion

    public virtual void Start()
    {
        ani = GetComponent<Animator>();
        SetMonster();
        if (!playerMonster)
        {
            hormone = transform.GetChild(1).gameObject;
            //exp = level * 10 + UnityEngine.Random.Range(5, 10);
            money = level * 50 + 100;
            monsterNameUI = transform.parent.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            monsterNameUI.text = monsterName;
        }
        monsterNameUI.text = monsterName;
        playerWorldPos = GameObject.FindGameObjectWithTag("PlayerWorldPos");
        monsterWorldPos = GameObject.FindGameObjectWithTag("EnemyWorldPos");
        Hp = MaxHp;
        endurance = maxEndurance;
        PlayerWorld.Gobattle += GoBattleMonster;
        PlayerWorld.battleOut += BattleOutMonster;
        skill = transform.GetChild(0).GetComponent<Skills>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerWorldPosOrigin = player.transform.GetChild(0).GetChild(1).transform;
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        if (GameObject.FindGameObjectWithTag("WorldPlayer") != null)
        {
            playerInWorld = GameObject.FindGameObjectWithTag("WorldPlayer").GetComponent<PlayerWorld>();
            playerInBattle = GameObject.FindGameObjectWithTag("BattlePlayer").GetComponent<PlayerBattle>();
        }
        #region AddSkills(Dictionary)
        getSkill.Add(1, "NomalAttack1");
        getSkill.Add(3, "NomalAttDebuff");
        getSkill.Add(6, "NomalAttack2");
        getSkill.Add(9, "NomalDefenceUp");
        getSkill.Add(12, "NomalSpDefenceUp");
        getSkill.Add(14, "NomalAttack3");
        getSkill.Add(17, "NomalAttack4");
        getSkill.Add(20, "NomalAgilitybuff");
        #endregion

    }

    void SetMonster()
    {
        if (!playerMonster)
        {
            MaxHp = (level * UnityEngine.Random.Range(47, 51)) + 100;
            att = (level * UnityEngine.Random.Range(27, 31)) + 150;
            def = (level * UnityEngine.Random.Range(27, 31)) + 100;
            spAtt = (level * UnityEngine.Random.Range(27, 31)) + 150;
            spDef = (level * UnityEngine.Random.Range(27, 31)) + 100;
            maxEndurance = UnityEngine.Random.Range(95, 105);
            agility = UnityEngine.Random.Range(10, 15);
        }
        else
        {
            MaxHp = (level * UnityEngine.Random.Range(47, 51)) + 100;//152515153;
            att = 200;
            def = (level * UnityEngine.Random.Range(27, 31)) + 100;
            spAtt = (level * UnityEngine.Random.Range(27, 31)) + 250;
            spDef = (level * UnityEngine.Random.Range(27, 31)) + 100;
            maxEndurance = 94;//UnityEngine.Random.Range(95, 105);
            agility = UnityEngine.Random.Range(10, 15);
        }
    }

    public void GoBattleMonster() 
    {
        endurance = maxEndurance;
        playerWorldPosOrigin = player.transform.GetChild(0).GetChild(1).transform;
    }

    public void BattleOutMonster()
    {
        if (!isDead && inBattle) // ������ �Ҷ�� ��Ʋ���� ���ƿ� ���͸� ����ؾ��Ѵ�. 
        {
            GameObject originPos = GameObject.FindGameObjectWithTag("EnemyWorldPos");
            transform.position = originPos.transform.position;
            hormone.transform.localPosition = new Vector3(0, 2, 0);
            inBattle = false;
        }
        if(playerMonster)
        {
            for (int i = 0; i < playerInBattle.monsters.Count; i++)
            {
                playerInBattle.monsters[i].transform.position = new Vector3(1495.19995f, 6000.60986f, -88.6800003f);
                playerInBattle.monsters[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                playerInBattle.monsters[i].transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);

            }
        }    
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && playerMonster)
            Level++;
        if (playerMonster && uiManager.monsterHpUI[0] != null)
        {
            uiManager.monsterHpUI[0].fillAmount = Mathf.Lerp(uiManager.monsterHpUI[0].fillAmount, Hp / MaxHp, Time.deltaTime * 200);
            if(uiManager.monsterHpUI[0].fillAmount <= 0.5f)
                uiManager.monsterHpUI[0].color = Color.red;
            else
                uiManager.monsterHpUI[0].color = Color.green;
        }
        if (tag == "EnemyMonsterOnBattle" && uiManager.monsterHpUI[1] != null) 
        {
            uiManager.monsterHpUI[1].fillAmount = Mathf.Lerp(uiManager.monsterHpUI[1].fillAmount, Hp / MaxHp, Time.deltaTime * 200);
            if (uiManager.monsterHpUI[1].fillAmount <= 0.5f)
                uiManager.monsterHpUI[1].color = Color.red;
            else
                uiManager.monsterHpUI[1].color = Color.green; 
        }
        if(playerMonster&& uiManager.monsterExpUI != null)
        {
            if (!isDead)
                uiManager.monsterExpUI.fillAmount = Mathf.Lerp(uiManager.monsterExpUI.fillAmount, exp / levelUpExp, Time.deltaTime * 100);
            else
                uiManager.monsterExpUI.fillAmount = exp / levelUpExp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>() != null)
        {
            gameObject.tag = "EnemyMonsterOnBattle"; // ���� ���� �� ��� �±׸� �ٲ� ��
            if (gameObject.tag == "EnemyMonsterOnBattle")
                inBattle = true;
            playerWorldPos.SetActive(true); // ��ġ ���� Ȱ��ȭ
            monsterWorldPos.SetActive(true);
            // �÷��̾�, ���� ���� ��ġ�� �����Ѵ�.
            playerWorldPos.transform.position = new Vector3(playerWorldPosOrigin.position.x, playerWorldPosOrigin.position.y, playerWorldPosOrigin.position.z);
            monsterWorldPos.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            playerInBattle.equipMonster = playerInWorld.equipMonster;
            PlayerWorld.Gobattle.Invoke(); // ��������Ʈ ����
            other.gameObject.SetActive(false);
        }
    }
    public void MonsterLevelUp()
    {
        increase[0] = UnityEngine.Random.Range(50, 60);
        for(int i = 1; i < increase.Length; i++)
            increase[i] = UnityEngine.Random.Range(30, 40);
        MaxHp += increase[0];
        att += increase[1];
        def += increase[2];
        spAtt += increase[3];
        spDef += increase[4];
        levelUp = true;
    }
    public void Evolution(bool step)
    {
        if (!step)
            Debug.Log("1�� ��ȭ");
        else
            Debug.Log("2�� ��ȭ");
    }
    public override void Dead()
    {
        isDead = true;
        StartCoroutine(DeadCo());
        if (!playerMonster)
        {
            exp = level * 10 + UnityEngine.Random.Range(5, 10);
            money = level * 50 + 100;
        }
        ani.SetBool("isDead", true);
        Debug.Log("����");
    }
    IEnumerator DeadCo()
    {
        yield return new WaitForSecondsRealtime(4);
        gameObject.SetActive(false);
    }

    public virtual void MonsterOnHit(Monster eMonster, float _damage, bool special)
    {
        float damage;
        float monsterDef;
        float monsterAtt;
                                // �÷��̾��� ����                  // �� ���� �����
        // ���� + ����, ����� ó��
        monsterDef = (((def + (skill.buff[(int)BuffList.def]) - (eMonster.skill.debuff[(int)BuffList.def])) * 0.1f));
        // ���ݷ� + ����, ����� ó�� + �����%
        monsterAtt = ((eMonster.att + ((eMonster.skill.buff[(int)BuffList.att]) - (skill.debuff[(int)BuffList.att]))) * 
        (_damage * 0.01f));
        // �Ϲݰ���
        if (!special)
        {
            damage = monsterAtt - monsterDef;
            if (damage < 1)
                damage = 1;
            if (eMonster.GetComponent<FighterMonster>() != null)
            {
                damage *= 1.5f;
                StartCoroutine(uiManager.AttackState(false,"�����ϴ�!"));
            }
            else
                StartCoroutine(uiManager.AttackState(false, "����"));
            StartCoroutine(skill.hitEffect[0].ObjectSwitch(2));
            ani.SetTrigger("OnHit");
            Hp -= damage;
        }
    }
    public override void OnCrisis()
    { }
}
