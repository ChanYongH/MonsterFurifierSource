using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkills : Skills
{
    public override void Start()
    {
        base.Start();
        nameToKorean.Add("FireAttack1", "불꽃 튀기기");
        nameToKorean.Add("FireSpDefDebuff", "기름 붓기");
        nameToKorean.Add("FireAttack2", "불 뿜기");
        nameToKorean.Add("FireSpAttBuff", "기름 코팅");
        nameToKorean.Add("FireAttack3", "파이어 붐");
        nameToKorean.Add("FireSpDefBuff", "화염 코팅");
        nameToKorean.Add("FireAttack4", "메테오 볼");
    }
    public void FireAttack1() // 5레벨
    {
        StartCoroutine(SpecialAttackCo(0, 40));
    }
    public void FireAttack2() // 11레벨
    {
        StartCoroutine(SpecialAttackCo(1, 60));
    }
    public void FireAttack3() // 17레벨
    {
        monster.endurance -= monster.agility;
        StartCoroutine(SpecialAttackCo(2, 90));
    }
    public void FireAttack4() // 22레벨
    {
        StartCoroutine(SpecialAttackCo(3, 100));
    }
    public void FireSpDefDebuff() // 8레벨
    {
        StartCoroutine(BuffEffect(false));
        if (skillInfo)
        {
            Debug.Log("스킬 이름 " + skillName);
            Debug.Log("스킬 효과 : 노말 속성 대미지 50");
        }
        else if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                debuffCount[(int)BuffList.spDef]++;
                debuff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                debuffCount[(int)BuffList.spDef]++;
                debuff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, enemyMonster.monsterName, "특수 방어력이 감소 했다!"));
    }
    public void FireSpAttBuff()  // 14레벨
    {
        StartCoroutine(BuffEffect(true));
        if (skillInfo)
        {
            Debug.Log("스킬 이름 " + skillName);
            Debug.Log("스킬 효과 : 노말 속성 대미지 50");
        }
        else if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.3f;
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spAtt]++;
                buff[(int)BuffList.spAtt] = enemyMonster.spAtt * 0.3f;
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "특수 공격력이 증가 했다!"));
    }
    public void FireSpDefBuff() // 19레벨
    {
        StartCoroutine(BuffEffect(true));
        if (skillInfo)
        {
            Debug.Log("스킬 이름 " + skillName);
            Debug.Log("스킬 효과 : 불 속성 대미지 50");
        }
        else if (!skillInfo && BattleManger.battle)
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spDef]++;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spDef]++;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "특수 방어력이 증가 했다!"));
    }
}
