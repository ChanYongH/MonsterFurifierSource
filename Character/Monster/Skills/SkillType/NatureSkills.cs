using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureSkills : Skills
{
    public override void Start()
    {
        base.Start();
        nameToKorean.Add("NatureAttack1", "Ǯ �ձ�");
        nameToKorean.Add("NatureHpRecovery", "�ڿ� ġ��");
        nameToKorean.Add("NatureAttack2", "���� ����");
        nameToKorean.Add("NatureDoubleDefBuff", "�ڿ��� ��ȣ");
        nameToKorean.Add("NatureAttack3", "���� ���");
        nameToKorean.Add("NatureSpAttBuff", "���ڿ��� ��");
        nameToKorean.Add("NatureAttack4", "PPAP");
    }
    public void NatureAttack1() // 5����
    {
        StartCoroutine(SpecialAttackCo(0, 40));
    }
    public void NatureAttack2() // 11����
    {
        StartCoroutine(SpecialAttackCo(1, 50));
    }
    public void NatureAttack3() // 17���� // ��� �����غ���
    {
        StartCoroutine(SpecialAttackCo(2, 70));
    }
    public void NatureAttack4() // 22����
    {
        StartCoroutine(SpecialAttackCo(3, 80));
    }
    public void NatureHpRecovery() // 8���� 40% ȸ��
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                monster.Hp += (monster.MaxHp * 0.4f);
                Debug.Log("Ǯ �Ӽ� Hpȸ��!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                monster.Hp += (monster.MaxHp * 0.4f);
                Debug.Log("Ǯ �Ӽ� Hpȸ��!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "ü���� ȸ�� �ߴ�!"));
    }
    public void NatureDoubleDefBuff()  // 14����
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // ��ų �ߵ� ȿ��
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.def]++;
                buff[(int)BuffList.def] = enemyMonster.def * 0.3f;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
                Debug.Log("Ǯ �Ӽ� ����, Ư�� ���� ��� ����!");
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.def]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.3f;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
                Debug.Log("Ǯ �Ӽ� ����, Ư�� ���� ��� ����!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, enemyMonster.monsterName, "���°� Ư�� ������ ���� �ߴ�!"));
    }
    public void NatureSpAttBuff() // 19���� // 70% ����
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle)
        {
            if (monster.playerMonster) // �÷��̾� ���Ͷ�� 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.7f;
            }
            else // �� ���Ͷ��
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spDef * 0.7f;
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "Ư�� ���ݷ��� ���� �ߴ�!"));
    }
}
