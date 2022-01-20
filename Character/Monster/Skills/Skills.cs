using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using TMPro;


public enum BuffList
{
    agility, att, def, spAtt, spDef
}

public class Skills : MonoBehaviour
{
    protected Monster monster;
    public Monster enemyMonster;
    Type skills;
    MethodInfo skillMethod;
    public string skillName;
    public string showSkillName;
    public bool fullSkill;
    public bool skillInfo; // �̸� ���� �ؾ� ��

    public UIManager uiManager; // private

    public Dictionary<string, string> nameToKorean; // Skill�� ����ε� string ���� �ѱ۷� �ٲ� ��

    protected float[] buffCount = new float[5]; // ���� �ð�
    protected float[] debuffCount = new float[5]; // ����� �ð�

    public float[] buff = new float[5]; // ����(�Ͻ������� �ö󰡴� ��)
    public float[] debuff = new float[5]; // �����(�Ͻ������� �ö󰡴� ��)
    public bool firstAttack = false; // ���� ���� ��ų


    //��ų
    public List<GameObject> attackSkillEffect = new List<GameObject>(); // ���Ͱ� ���� �� �� ������ ��ƼŬ
    public List<GameObject> buffEffect = new List<GameObject>(); // ���Ͱ� ������ �ɰų� ������� ������ �� ������ ��ƼŬ
    public List<GameObject> hitEffect = new List<GameObject>(); // �¾��� �� ������ ��ƼŬ
    public float skillUsingTime; // �ִϸ��̼� ���ӽð�
    public virtual void Start()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
            attackSkillEffect.Add(transform.GetChild(0).GetChild(i).gameObject);
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
            buffEffect.Add(transform.GetChild(1).GetChild(i).gameObject);
        //0 : �븻 / 1 : �� / 2 : �� / 3 : �ڿ�
        for (int i = 0; i < transform.GetChild(2).childCount; i++)
            hitEffect.Add(transform.GetChild(2).GetChild(i).gameObject);


        nameToKorean = new Dictionary<string, string>();
        monster = transform.GetComponentInParent<Monster>();
        skills = GetComponent("Skills").GetType(); // �� Ŭ������ Ÿ���� �����´�.
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        nameToKorean.Add("NomalAttack1", "��ġ��");
        nameToKorean.Add("NomalAttDebuff", "°������");
        nameToKorean.Add("NomalAttack2", "�����ġ��");
        nameToKorean.Add("NomalDefenceUp", "�ܴ�������");
        nameToKorean.Add("NomalSpDefenceUp", "Ư�� ����");
        nameToKorean.Add("NomalAttack3", "���� ġ��");
        nameToKorean.Add("NomalAttack4", "����������");
        nameToKorean.Add("NomalAgilitybuff", "�ż�������");
    }
    public void AddSkill(string _skillName) // monster�� level ������Ƽ�� ���
    {
        skillName = _skillName;
        CallReflection();
    }
    public void CallReflection() // private
    {
        fullSkill = monster.equipSkill.Count > 2;
        skillMethod = skills.GetMethod(skillName);
        for (int i = 0; i < monster.equipSkill.Count; i++)
            Debug.Log(monster.equipSkill[i]);
        if (!fullSkill) // ��ų�� 3�� �̸����� ������ ������
        {
            monster.equipSkill.Add(skillName); // ������ List<string>�� �־��ش�
        }
        else
        {
            skillInfo = true; // ��ų�� ������ ���� �� �� �ֵ����ߴ�.
            skillMethod.Invoke(GetComponent("Skills"), null); // skillMetod���� ������ �޼��带 �����Ų��.
            uiManager.skillChange = true;
            //StartCoroutine(uiManager.ChangeSkillPanel());
        }
    }
    // 7�� °
    public void UseSkill()
    {
        if (monster.playerMonster)
        {
            skillName = uiManager.selectSkill;
            skillMethod = skills.GetMethod(uiManager.selectSkill);
            skillMethod.Invoke(GetComponent("Skills"), null);
        }
        else // ���� �� �� �߿��� �������� ����
        {
            //int randomNum = UnityEngine.Random.Range(0, 3);
            //skillName = monster.equipSkill[randomNum];
            //skillMethod = skills.GetMethod(monster.equipSkill[randomNum]);
            skillMethod = skills.GetMethod(uiManager.enemySetSkill);
            skillMethod.Invoke(GetComponent("Skills"), null);
        }
        BuffDisabled(); // ��ų�� �������� ���� ����
    }

    //��������(2�ϰ� ������)
    public void BuffDisabled() // ��Ʋ�Ŵ������� ��� 
    {
        for (int i = 0; i < buffCount.Length; i++)
        {
            if (buffCount[i] >= 1) // ����, �������ų�� ����ϸ� 
                buffCount[i]++;
            if (debuffCount[i] >= 1) // �� ���� ī��Ʈ�� �ö󰣴�.
                debuffCount[i]++;
            if (buffCount[i] > 3) // ���� ����
            {
                buffCount[i] = 0;
                buff[i] = 0;
                Debug.Log("��������!");
            }
            if (debuffCount[i] > 3)
            {
                debuffCount[i] = 0;
                debuff[i] = 0;
                Debug.Log("���������!");
            }
        }
    }
    public IEnumerator SpecialAttackCo(int attKind, float damage) // �ٸ� �Ӽ������� ���
    {
        if (!skillInfo && BattleManger.battle)
        {
            monster.ani.SetTrigger("isAttack"); // ���� ��� �ߵ�
            skillUsingTime = monster.ani.GetCurrentAnimatorStateInfo(0).length; // ���� ��� ���ӽð�
            StartCoroutine(attackSkillEffect[attKind].ObjectSwitch(2)); // Ȯ�� �޼��带 �̿��ؼ� 2�ʵڿ� ������ ����
        
            yield return new WaitForSecondsRealtime(skillUsingTime);

            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, damage, true); 
                Debug.Log(skillName + "�� ����!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, damage, true);
                Debug.Log(" FireStrike �� ����!");
            }
        }
    }

    public IEnumerator NomalAttackCo(float damage)
    {
        if (!skillInfo && BattleManger.battle)
        {
            monster.ani.SetTrigger("isAttack"); // ���� ��� �ߵ�
            skillUsingTime = monster.ani.GetCurrentAnimatorStateInfo(0).length; // ���� ��� ���ӽð�

            // �븻 ��ų�� ��ƼŬ�� ����.
            yield return new WaitForSecondsRealtime(skillUsingTime);

            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, damage, false);
                Debug.Log(skillName + "�� ����!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, damage, false);
                Debug.Log(" FireStrike �� ����!");
            }
        }
    }
    public IEnumerator BuffEffect(bool isBuff)
    {
        if (!skillInfo)
        {
            monster.ani.SetTrigger("isAttack"); // ���� ��� �ߵ�
            skillUsingTime = monster.ani.GetCurrentAnimatorStateInfo(0).length; // ���� ��� ���ӽð�
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
            else
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
            if (isBuff)
                StartCoroutine(monster.skill.buffEffect[0].ObjectSwitch(2));
            else
                StartCoroutine(enemyMonster.skill.buffEffect[1].ObjectSwitch(2));
            yield return new WaitForSecondsRealtime(skillUsingTime);
        }
    }
    public void NomalAttack1() // 1����
    {
        StartCoroutine(NomalAttackCo(40));
    }
    public void NomalAttack2() // 6����
    {
        StartCoroutine(NomalAttackCo(50));
    }
    public void NomalAttack3() // 14����
    {
        StartCoroutine(NomalAttackCo(65));
    }
    public void NomalAttack4() // 17����
    {
        StartCoroutine(NomalAttackCo(80));
    }

    public void NomalAttDebuff() // 3����
    {
        StartCoroutine(BuffEffect(false));
        if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {

            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                debuffCount[(int)BuffList.att]++;
                debuff[(int)BuffList.att] = enemyMonster.att * 0.3f;
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                debuffCount[(int)BuffList.att]++;
                debuff[(int)BuffList.att] = enemyMonster.att * 0.3f;
            }
            StartCoroutine(uiManager.AttackState(true, null, enemyMonster.monsterName, "���ݷ��� ���� �ߴ�!"));
        }
    }
    public void NomalDefenceUp() // 9����
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                buffCount[(int)BuffList.def]++; // ������ �ɸ� ī��Ʈ�� �ȴ�. 3��°�� ����
                buff[(int)BuffList.def] = monster.def * 0.5f; // �Ͻ������� ��µǴ� �� // �������� ���� : enemyMonster.def * 0.3f
                Debug.Log(transform.parent.name+"���� ���~");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.def]++;
                buff[(int)BuffList.def] = monster.def * 0.5f;
                Debug.Log(transform.parent.name + "���� ���~");
            }
            StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "������ ���� �ߴ�!"));
        }
    }
    public void NomalSpDefenceUp() // 12����
    {
        StartCoroutine(BuffEffect(true));
        if(!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                buffCount[(int)BuffList.spDef]++; // ������ �ɸ� ī��Ʈ�� �ȴ�. 3��°�� ����
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f; // �Ͻ������� ��µǴ� �� 
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spDef]++;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
            StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "Ư�� ������ ���� �ߴ�!"));
        }
    }
    public void NomalAgilitybuff() // 20����
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                buffCount[(int)BuffList.agility]++;
                buff[(int)BuffList.agility] = enemyMonster.agility * 0.5f;
                Debug.Log(transform.parent.name + "�� ���ǵ� ���" + buff[(int)BuffList.agility]);
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.agility]++;
                buff[(int)BuffList.agility] = enemyMonster.agility * 0.5f;
                Debug.Log(transform.parent.name + "�� ���ǵ� ���" + buff[(int)BuffList.agility]);
            }
            StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "�������� �����ؼ� �� ���� ������!"));
        }
    }
    //#endregion
}

