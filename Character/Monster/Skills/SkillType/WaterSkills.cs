using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSkills : Skills
{
    public override void Start()
    {
        base.Start();
        #region SkillNameDictionary
        nameToKorean.Add("WaterAttack1", "물 튀기기");
        nameToKorean.Add("WaterAgilitybuff", "물살가르기");
        nameToKorean.Add("WaterAttack2", "물 뿜기");
        nameToKorean.Add("WaterSpAttbuff", "촉촉해지기");
        nameToKorean.Add("WaterAttack3", "아쿠아 샷");
        nameToKorean.Add("WaterEnduranceRecovery", "수분 보충");
        nameToKorean.Add("WaterAttack4", "아쿠아 캐논");
        #endregion
    }
    public void WaterAttack1() // 5레벨
    {
        StartCoroutine(SpecialAttackCo(0, 40));
    }
    public void WaterAttack2() // 11레벨
    {
        StartCoroutine(SpecialAttackCo(0, 50));
    }
    public void WaterAttack3() // 17레벨 // 선제타격 기능 넣기
    {
        StartCoroutine(SpecialAttackCo(0, 60));
    }
    public void WaterAttack4() // 22레벨 // 날렵함의 반절만 사용한다.
    {
        StartCoroutine(SpecialAttackCo(0, 75));
        monster.endurance += (monster.agility / 2);
    }
    public void WaterAgilitybuff() // 8레벨
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.agility]++;
                debuff[(int)BuffList.agility] = enemyMonster.agility * 0.5f;
                Debug.Log("특수 방어력 감소!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.agility]++;
                buff[(int)BuffList.agility] = enemyMonster.agility * 0.5f;
                Debug.Log("특수 방어력 감소!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "날렵함이 감소해서 더 날렵 해졌다!"));
    }
    public void WaterSpAttbuff() // 14레벨 // 임시
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.3f;
                Debug.Log("특수 공격력 증가!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.3f;
                Debug.Log("특수 공격력 증가!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "특수 공격력이 증가 했다!"));
    }
    public void WaterEnduranceRecovery() // 19레벨
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                monster.endurance = monster.maxEndurance;
                Debug.Log("지구력 회복!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                monster.endurance = monster.maxEndurance;
                Debug.Log("지구력 회복!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "지구력을 모두 회복했다!"));
    }
}
