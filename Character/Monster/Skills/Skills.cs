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
    public bool skillInfo; // 이름 변경 해야 됨

    public UIManager uiManager; // private

    public Dictionary<string, string> nameToKorean; // Skill의 영어로된 string 값을 한글로 바꿔 줌

    protected float[] buffCount = new float[5]; // 버프 시간
    protected float[] debuffCount = new float[5]; // 디버프 시간

    public float[] buff = new float[5]; // 버프(일시적으로 올라가는 값)
    public float[] debuff = new float[5]; // 디버프(일시적으로 올라가는 값)
    public bool firstAttack = false; // 선제 공격 스킬


    //스킬
    public List<GameObject> attackSkillEffect = new List<GameObject>(); // 몬스터가 공격 할 때 나오는 파티클
    public List<GameObject> buffEffect = new List<GameObject>(); // 몬스터가 버프를 걸거나 디버프를 당했을 때 나오는 파티클
    public List<GameObject> hitEffect = new List<GameObject>(); // 맞았을 때 나오는 파티클
    public float skillUsingTime; // 애니메이션 지속시간
    public virtual void Start()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
            attackSkillEffect.Add(transform.GetChild(0).GetChild(i).gameObject);
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
            buffEffect.Add(transform.GetChild(1).GetChild(i).gameObject);
        //0 : 노말 / 1 : 불 / 2 : 물 / 3 : 자연
        for (int i = 0; i < transform.GetChild(2).childCount; i++)
            hitEffect.Add(transform.GetChild(2).GetChild(i).gameObject);


        nameToKorean = new Dictionary<string, string>();
        monster = transform.GetComponentInParent<Monster>();
        skills = GetComponent("Skills").GetType(); // 이 클래스의 타입을 가져온다.
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        nameToKorean.Add("NomalAttack1", "박치기");
        nameToKorean.Add("NomalAttDebuff", "째려보기");
        nameToKorean.Add("NomalAttack2", "몸통박치기");
        nameToKorean.Add("NomalDefenceUp", "단단해지기");
        nameToKorean.Add("NomalSpDefenceUp", "특수 코팅");
        nameToKorean.Add("NomalAttack3", "힘껏 치기");
        nameToKorean.Add("NomalAttack4", "뭉개버리기");
        nameToKorean.Add("NomalAgilitybuff", "신속해지기");
    }
    public void AddSkill(string _skillName) // monster의 level 프로퍼티로 사용
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
        if (!fullSkill) // 스킬을 3개 미만으로 가지고 있으면
        {
            monster.equipSkill.Add(skillName); // 몬스터의 List<string>에 넣어준다
        }
        else
        {
            skillInfo = true; // 스킬의 정보를 보여 줄 수 있도록했다.
            skillMethod.Invoke(GetComponent("Skills"), null); // skillMetod에서 가져온 메서드를 실행시킨다.
            uiManager.skillChange = true;
            //StartCoroutine(uiManager.ChangeSkillPanel());
        }
    }
    // 7번 째
    public void UseSkill()
    {
        if (monster.playerMonster)
        {
            skillName = uiManager.selectSkill;
            skillMethod = skills.GetMethod(uiManager.selectSkill);
            skillMethod.Invoke(GetComponent("Skills"), null);
        }
        else // 장착 한 것 중에서 랜덤으로 공격
        {
            //int randomNum = UnityEngine.Random.Range(0, 3);
            //skillName = monster.equipSkill[randomNum];
            //skillMethod = skills.GetMethod(monster.equipSkill[randomNum]);
            skillMethod = skills.GetMethod(uiManager.enemySetSkill);
            skillMethod.Invoke(GetComponent("Skills"), null);
        }
        BuffDisabled(); // 스킬을 쓸때마다 턴이 갱신
    }

    //버프해제(2턴간 지속임)
    public void BuffDisabled() // 배틀매니저에서 사용 
    {
        for (int i = 0; i < buffCount.Length; i++)
        {
            if (buffCount[i] >= 1) // 버프, 디버프스킬을 사용하면 
                buffCount[i]++;
            if (debuffCount[i] >= 1) // 턴 마다 카운트가 올라간다.
                debuffCount[i]++;
            if (buffCount[i] > 3) // 버프 해제
            {
                buffCount[i] = 0;
                buff[i] = 0;
                Debug.Log("버프해제!");
            }
            if (debuffCount[i] > 3)
            {
                debuffCount[i] = 0;
                debuff[i] = 0;
                Debug.Log("디버프해제!");
            }
        }
    }
    public IEnumerator SpecialAttackCo(int attKind, float damage) // 다른 속성에서도 사용
    {
        if (!skillInfo && BattleManger.battle)
        {
            monster.ani.SetTrigger("isAttack"); // 공격 모션 발동
            skillUsingTime = monster.ani.GetCurrentAnimatorStateInfo(0).length; // 공격 모션 지속시간
            StartCoroutine(attackSkillEffect[attKind].ObjectSwitch(2)); // 확장 메서드를 이용해서 2초뒤에 꺼지게 설정
        
            yield return new WaitForSecondsRealtime(skillUsingTime);

            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, damage, true); 
                Debug.Log(skillName + "로 공격!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, damage, true);
                Debug.Log(" FireStrike 적 공격!");
            }
        }
    }

    public IEnumerator NomalAttackCo(float damage)
    {
        if (!skillInfo && BattleManger.battle)
        {
            monster.ani.SetTrigger("isAttack"); // 공격 모션 발동
            skillUsingTime = monster.ani.GetCurrentAnimatorStateInfo(0).length; // 공격 모션 지속시간

            // 노말 스킬은 파티클이 없다.
            yield return new WaitForSecondsRealtime(skillUsingTime);

            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, damage, false);
                Debug.Log(skillName + "로 공격!");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                enemyMonster.MonsterOnHit(monster, damage, false);
                Debug.Log(" FireStrike 적 공격!");
            }
        }
    }
    public IEnumerator BuffEffect(bool isBuff)
    {
        if (!skillInfo)
        {
            monster.ani.SetTrigger("isAttack"); // 공격 모션 발동
            skillUsingTime = monster.ani.GetCurrentAnimatorStateInfo(0).length; // 공격 모션 지속시간
            if (monster.playerMonster) // 플레이어 몬스터라면 
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
    public void NomalAttack1() // 1레벨
    {
        StartCoroutine(NomalAttackCo(40));
    }
    public void NomalAttack2() // 6레벨
    {
        StartCoroutine(NomalAttackCo(50));
    }
    public void NomalAttack3() // 14레벨
    {
        StartCoroutine(NomalAttackCo(65));
    }
    public void NomalAttack4() // 17레벨
    {
        StartCoroutine(NomalAttackCo(80));
    }

    public void NomalAttDebuff() // 3레벨
    {
        StartCoroutine(BuffEffect(false));
        if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {

            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                debuffCount[(int)BuffList.att]++;
                debuff[(int)BuffList.att] = enemyMonster.att * 0.3f;
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                debuffCount[(int)BuffList.att]++;
                debuff[(int)BuffList.att] = enemyMonster.att * 0.3f;
            }
            StartCoroutine(uiManager.AttackState(true, null, enemyMonster.monsterName, "공격력이 감소 했다!"));
        }
    }
    public void NomalDefenceUp() // 9레벨
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                buffCount[(int)BuffList.def]++; // 버프를 걸면 카운트가 된다. 3턴째에 해제
                buff[(int)BuffList.def] = monster.def * 0.5f; // 일시적으로 상승되는 값 // 수정했음 이전 : enemyMonster.def * 0.3f
                Debug.Log(transform.parent.name+"방어력 상승~");
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.def]++;
                buff[(int)BuffList.def] = monster.def * 0.5f;
                Debug.Log(transform.parent.name + "방어력 상승~");
            }
            StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "방어력이 증가 했다!"));
        }
    }
    public void NomalSpDefenceUp() // 12레벨
    {
        StartCoroutine(BuffEffect(true));
        if(!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                buffCount[(int)BuffList.spDef]++; // 버프를 걸면 카운트가 된다. 3턴째에 해제
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f; // 일시적으로 상승되는 값 
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.spDef]++;
                buff[(int)BuffList.spDef] = enemyMonster.spDef * 0.3f;
            }
            StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "특수 방어력이 증가 했다!"));
        }
    }
    public void NomalAgilitybuff() // 20레벨
    {
        StartCoroutine(BuffEffect(true));
        if (!skillInfo && BattleManger.battle) // 스킬 발동 효과
        {
            if (monster.playerMonster) // 플레이어 몬스터라면 
            {
                enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
                buffCount[(int)BuffList.agility]++;
                buff[(int)BuffList.agility] = enemyMonster.agility * 0.5f;
                Debug.Log(transform.parent.name + "의 스피드 상승" + buff[(int)BuffList.agility]);
            }
            else // 적 몬스터라면
            {
                enemyMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
                buffCount[(int)BuffList.agility]++;
                buff[(int)BuffList.agility] = enemyMonster.agility * 0.5f;
                Debug.Log(transform.parent.name + "의 스피드 상승" + buff[(int)BuffList.agility]);
            }
            StartCoroutine(uiManager.AttackState(true, null, monster.monsterName, "날렵함이 감소해서 더 날렵 해졌다!"));
        }
    }
    //#endregion
}

