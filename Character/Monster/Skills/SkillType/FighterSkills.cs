using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSkills : Skills
{
    public override void Start()
    {
        base.Start();
        nameToKorean.Add("FighterAttack1", "격투 공격1");
        nameToKorean.Add("FighterAttBuff", "격투 공격 버프");
        nameToKorean.Add("FighterAttack2", "격투 공격2");
        nameToKorean.Add("FighterDefDebuff", "격투 방어 약화");
        nameToKorean.Add("FighterAttack3", "격투 공격3");
        nameToKorean.Add("FighterAttack4", "격투 공격4");
        nameToKorean.Add("FighterAttDefBuff", "격투 공격 약화");
    }
    public void FighterAttack1() // 5레벨
    {
        //skillName = "FireStrike";
        if (skillInfo)
        {
            Debug.Log("스킬 이름 " + skillName);
            Debug.Log("스킬 효과 : 노말 속성 대미지 50");
        }
        else if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 60, false);
                Debug.Log(skillName + "로 공격!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 60, false);
                Debug.Log(skillName + "로 공격!");
            }
        }
    }
    public void FighterAttBuff() // 8레벨
    {
        //skillName = "NomalDefenceDebuff";
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
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "공격력이 증가 했다!"));
    }
    public void FighterAttack2() // 11레벨
    {
        //skillName = "FireStrike";
        if (skillInfo)
        {
            Debug.Log("스킬 이름 " + skillName);
            Debug.Log("스킬 효과 : 노말 속성 대미지 50");
        }
        else if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 80, false);
                Debug.Log(skillName + "로 공격!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 80, false);
                Debug.Log(" FireStrike 적 공격!");
            }
        }
    }
    public void FighterDefDebuff() // 14레벨 // 임시
    {
        //skillName = "NomalDefenceDebuff";
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
        StartCoroutine(uiManager.AttackState(true, null, enemyMonster.monsterName, "방어력이 감소 했다!"));
    }
    public void FighterAttack3() // 17레벨 지구력 모두 소모
    {
        //skillName = "FireStrike";
        if (skillInfo)
        {
            Debug.Log("스킬 이름 " + skillName);
            Debug.Log("스킬 효과 : 노말 속성 대미지 50");
        }
        else if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            firstAttack = true;
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 150, false);
                monster.endurance = 0;
                Debug.Log(skillName + "로 공격!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 150, false);
                monster.endurance = 0;
                Debug.Log(" FireStrike 적 공격!");
            }
        }
    }
    public void FighterAttack4() // 18레벨
    {
        //skillName = "FireStrike";
        if (skillInfo)
        {
            Debug.Log("스킬 이름 " + skillName);
            Debug.Log("스킬 효과 : 노말 속성 대미지 50");
        }
        else if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            monster.endurance += (monster.agility / 2);
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 100, false);
                Debug.Log(skillName + "로 공격!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, 100, false);
                Debug.Log(skillName + "로 공격!");
            }

        }
    }
    public void FighterAttDefBuff() // 19레벨
    {
        //skillName = "NomalDefenceDebuff";
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
                buffCount[(int)BuffList.att]++;
                buff[(int)BuffList.att] = enemyMonster.att * 0.3f;
                buff[(int)BuffList.def] = enemyMonster.def * 0.3f;
                Debug.Log("격투 속성 공, 방업!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.att]++;
                buff[(int)BuffList.att] = enemyMonster.att * 0.3f;
                buff[(int)BuffList.def] = enemyMonster.def * 0.3f;
                Debug.Log("격투 속성 공, 방업!");
            }
        }
        StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "공격력과 방어력이 증가 했다!"));
    } 
}
