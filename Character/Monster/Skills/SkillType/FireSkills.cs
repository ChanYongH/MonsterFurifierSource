using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkills : Skills
{
    public override void Start()
    {
        base.Start();
        nameToKorean.Add("FireAttack1", "�Ҳ� Ƣ���");
        nameToKorean.Add("FireSpDefDebuff", "�⸧ �ױ�");
        nameToKorean.Add("FireAttack2", "�� �ձ�");
        nameToKorean.Add("FireSpAttBuff", "�⸧ ����");
        nameToKorean.Add("FireAttack3", "���̾� ��");
        nameToKorean.Add("FireSpDefBuff", "ȭ�� ����");
        nameToKorean.Add("FireAttack4", "���׿� ��");
    }
    public void FireAttack1() // 5����
    {
        StartCoroutine(SpecialAttackCo(0, 40));
    }
    public void FireAttack2() // 11����
    {
        StartCoroutine(SpecialAttackCo(1, 60));
    }
    public void FireAttack3() // 17����
    {
        monster.endurance -= monster.agility;
        StartCoroutine(SpecialAttackCo(2, 90));
    }
    public void FireAttack4() // 22����
    {
        StartCoroutine(SpecialAttackCo(3, 100));
    }
    public void FireSpDefDebuff() // 8����
    {
        StartCoroutine(BuffEffect(false));
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
                debuffCount[(int)BuffList.spDef]++;
                debuff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                debuffCount[(int)BuffList.spDef]++;
                debuff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, enemyMonster.monsterName, "Ư�� ������ ���� �ߴ�!"));
    }
    public void FireSpAttBuff()  // 14����
    {
        StartCoroutine(BuffEffect(true));
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
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.3f;
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "Ư�� ���ݷ��� ���� �ߴ�!"));
    }
    public void FireSpDefBuff() // 19����
    {
        StartCoroutine(BuffEffect(true));
        if (skillInfo)
        {
            Debug.Log("��ų �̸� " + skillName);
            Debug.Log("��ų ȿ�� : �� �Ӽ� ����� 50");
        }
        else if (!skillInfo && BattleManger.battle)
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spDef]++;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spDef]++;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "Ư�� ������ ���� �ߴ�!"));
    }
}
