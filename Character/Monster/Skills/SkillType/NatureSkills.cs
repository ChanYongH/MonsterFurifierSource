using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureSkills : Skills
{
    public override void Start()
    {
        base.Start();
        nameToKorean.Add("NatureAttack1", "풀 뿜기");
        nameToKorean.Add("NatureHpRecovery", "자연 치유");
        nameToKorean.Add("NatureAttack2", "리프 어택");
        nameToKorean.Add("NatureDoubleDefBuff", "자연의 보호");
        nameToKorean.Add("NatureAttack3", "대지 흡수");
        nameToKorean.Add("NatureSpAttBuff", "대자연의 힘");
        nameToKorean.Add("NatureAttack4", "PPAP");
    }
    public void NatureAttack1() // 5레벨
    {
        StartCoroutine(SpecialAttackCo(0, 40));
    }
    public void NatureAttack2() // 11레벨
    {
        StartCoroutine(SpecialAttackCo(1, 50));
    }
    public void NatureAttack3() // 17레벨 // 흡수 생각해보기
    {
        StartCoroutine(SpecialAttackCo(2, 70));
    }
    public void NatureAttack4() // 22레벨
    {
        StartCoroutine(SpecialAttackCo(3, 80));
    }
    public void NatureHpRecovery() // 8레벨 40% 회복
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                monster.Hp += (monster.MaxHp * 0.4f);
                Debug.Log("풀 속성 Hp회복!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                monster.Hp += (monster.MaxHp * 0.4f);
                Debug.Log("풀 속성 Hp회복!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "체력을 회복 했다!"));
    }
    public void NatureDoubleDefBuff()  // 14레벨
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.def]++;
                buff[(int)BuffList.def] = enemyMonster.def * 0.3f;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
                Debug.Log("풀 속성 방어력, 특수 방어력 모두 증가!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.def]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.3f;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
                Debug.Log("풀 속성 방어력, 특수 방어력 모두 증가!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, enemyMonster.monsterName, "방어력과 특수 방어력이 감소 했다!"));
    }
    public void NatureSpAttBuff() // 19레벨 // 70% 증가
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle)
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.7f;
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spDef * 0.7f;
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "특수 공격력이 증가 했다!"));
    }
}
