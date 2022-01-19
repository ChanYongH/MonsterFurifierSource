using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSkills : Skills
{
    public override void Start()
    {
        base.Start();
        #region SkillNameDictionary
        nameToKorean.Add("WaterAttack1", "�� Ƣ���");
        nameToKorean.Add("WaterAgilitybuff", "���찡����");
        nameToKorean.Add("WaterAttack2", "�� �ձ�");
        nameToKorean.Add("WaterSpAttbuff", "����������");
        nameToKorean.Add("WaterAttack3", "����� ��");
        nameToKorean.Add("WaterEnduranceRecovery", "���� ����");
        nameToKorean.Add("WaterAttack4", "����� ĳ��");
        #endregion
    }
    public void WaterAttack1() // 5����
    {
        StartCoroutine(SpecialAttackCo(0, 40));
    }
    public void WaterAttack2() // 11����
    {
        StartCoroutine(SpecialAttackCo(0, 50));
    }
    public void WaterAttack3() // 17���� // ����Ÿ�� ��� �ֱ�
    {
        StartCoroutine(SpecialAttackCo(0, 60));
    }
    public void WaterAttack4() // 22���� // �������� ������ ����Ѵ�.
    {
        StartCoroutine(SpecialAttackCo(0, 75));
        monster.endurance += (monster.agility / 2);
    }
    public void WaterAgilitybuff() // 8����
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
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
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "�������� �����ؼ� �� ���� ������!"));
    }
    public void WaterSpAttbuff() // 14���� // �ӽ�
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
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
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "Ư�� ���ݷ��� ���� �ߴ�!"));
    }
    public void WaterEnduranceRecovery() // 19����
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                monster.endurance = monster.maxEndurance;
                Debug.Log("������ ȸ��!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                monster.endurance = monster.maxEndurance;
                Debug.Log("������ ȸ��!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "�������� ��� ȸ���ߴ�!"));
    }
}
