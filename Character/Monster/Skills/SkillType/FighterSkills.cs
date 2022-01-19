using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSkills : Skills
{
    public override void Start()
    {
        base.Start();
        nameToKorean.Add("FighterAttack1", "���� ����1");
        nameToKorean.Add("FighterAttBuff", "���� ���� ����");
        nameToKorean.Add("FighterAttack2", "���� ����2");
        nameToKorean.Add("FighterDefDebuff", "���� ��� ��ȭ");
        nameToKorean.Add("FighterAttack3", "���� ����3");
        nameToKorean.Add("FighterAttack4", "���� ����4");
        nameToKorean.Add("FighterAttDefBuff", "���� ���� ��ȭ");
    }
    public void FighterAttack1() // 5����
    {
        //skillName = "FireStrike";
        if (skillInfo)
        {
            Debug.Log("��ų �̸� " + skillName);
            Debug.Log("��ų ȿ�� : �븻 �Ӽ� ����� 50");
        }
        else if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 60, false);
                Debug.Log(skillName + "�� ����!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 60, false);
                Debug.Log(skillName + "�� ����!");
            }
        }
    }
    public void FighterAttBuff() // 8����
    {
        //skillName = "NomalDefenceDebuff";
        if (skillInfo)
        {
            Debug.Log("��ų �̸� " + skillName);
            Debug.Log("��ų ȿ�� : �븻 �Ӽ� ����� 50");
        }
        else if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.agility]++;
                debuff[(int)BuffList.agility] = enemyMonster.agility * 0.5f;
                Debug.Log("Ư�� ���� ����!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.agility]++;
                buff[(int)BuffList.agility] = enemyMonster.agility * 0.5f;
                Debug.Log("Ư�� ���� ����!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "���ݷ��� ���� �ߴ�!"));
    }
    public void FighterAttack2() // 11����
    {
        //skillName = "FireStrike";
        if (skillInfo)
        {
            Debug.Log("��ų �̸� " + skillName);
            Debug.Log("��ų ȿ�� : �븻 �Ӽ� ����� 50");
        }
        else if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 80, false);
                Debug.Log(skillName + "�� ����!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 80, false);
                Debug.Log(" FireStrike �� ����!");
            }
        }
    }
    public void FighterDefDebuff() // 14���� // �ӽ�
    {
        //skillName = "NomalDefenceDebuff";
        if (skillInfo)
        {
            Debug.Log("��ų �̸� " + skillName);
            Debug.Log("��ų ȿ�� : �븻 �Ӽ� ����� 50");
        }
        else if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.3f;
                Debug.Log("Ư�� ���ݷ� ����!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.3f;
                Debug.Log("Ư�� ���ݷ� ����!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, enemyMonster.monsterName, "������ ���� �ߴ�!"));
    }
    public void FighterAttack3() // 17���� ������ ��� �Ҹ�
    {
        //skillName = "FireStrike";
        if (skillInfo)
        {
            Debug.Log("��ų �̸� " + skillName);
            Debug.Log("��ų ȿ�� : �븻 �Ӽ� ����� 50");
        }
        else if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            firstAttack = true;
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 150, false);
                monster.endurance = 0;
                Debug.Log(skillName + "�� ����!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 150, false);
                monster.endurance = 0;
                Debug.Log(" FireStrike �� ����!");
            }
        }
    }
    public void FighterAttack4() // 18����
    {
        //skillName = "FireStrike";
        if (skillInfo)
        {
            Debug.Log("��ų �̸� " + skillName);
            Debug.Log("��ų ȿ�� : �븻 �Ӽ� ����� 50");
        }
        else if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            monster.endurance += (monster.agility / 2);
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 100, false);
                Debug.Log(skillName + "�� ����!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 100, false);
                Debug.Log(skillName + "�� ����!");
            }

        }
    }
    public void FighterAttDefBuff() // 19����
    {
        //skillName = "NomalDefenceDebuff";
        if (skillInfo)
        {
            Debug.Log("��ų �̸� " + skillName);
            Debug.Log("��ų ȿ�� : �븻 �Ӽ� ����� 50");
        }
        else if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.att]++;
                buff[(int)BuffList.att] = enemyMonster.att * 0.3f;
                buff[(int)BuffList.def] = enemyMonster.def * 0.3f;
                Debug.Log("���� �Ӽ� ��, ���!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.att]++;
                buff[(int)BuffList.att] = enemyMonster.att * 0.3f;
                buff[(int)BuffList.def] = enemyMonster.def * 0.3f;
                Debug.Log("���� �Ӽ� ��, ���!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "���ݷ°� ������ ���� �ߴ�!"));
    } 
}
