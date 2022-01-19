using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class FireMonster : Monster
{
    public override void Start()
    {
        base.Start();
        #region FireSkills(Dic)
        getSkill.Keys.ToList().ForEach(key =>
        {
            getSkill[14] = "FireSpAttBuff";
            getSkill[17] = "FireAttack3";
        });
        getSkill.Add(5, "FireAttack1");
        getSkill.Add(8, "FireSpDefDebuff");
        getSkill.Add(11, "FireAttack2");
        getSkill.Add(19, "FireSpDefBuff");
        getSkill.Add(22, "FireAttack4");
        #endregion
        if (!playerMonster) // 적 몬스터면
        {
            for(int i = level; i > level-8; i--)
            {
                // 레벨 - 8의 있는 딕셔너리의 key값을 가져와서 return한다.
                // return 한 string을 장착 해준다.
                if (getSkill.ContainsKey(i) && equipSkill.Count < 3) 
                    equipSkill.Add(getSkill[i]);
            }
        }
    }
    public override void MonsterOnHit(Monster eMonster, float _damage, bool special)
    {
        base.MonsterOnHit(eMonster, _damage, special);
        if(special)
        {
            float spDamage;
            float monsterSpDef;
            float monsterSpAtt;
            monsterSpDef = (((spDef + (skill.buff[(int)BuffList.spDef]) - (eMonster.skill.debuff[(int)BuffList.spDef])) * 0.1f));
            //공격력 + 버프, 디버프 처리 + 대미지%
            monsterSpAtt = ((eMonster.spAtt + ((eMonster.skill.buff[(int)BuffList.spAtt]) - (skill.debuff[(int)BuffList.spAtt]))) * (_damage * 0.01f));
            spDamage = monsterSpAtt - monsterSpDef;
            if (eMonster.GetComponent<WaterMonster>() != null) // 1.5배 대미지
            {
                spDamage *= 1.5f;
                StartCoroutine(skill.hitEffect[2].ObjectSwitch(2));
                StartCoroutine(uiManager.AttackState(false, "굉장하다!"));
            }
            else if (eMonster.GetComponent<NatureMonster>() != null) // 0.5배 대미지
            {
                spDamage *= 0.5f;
                StartCoroutine(skill.hitEffect[3].ObjectSwitch(2));
                StartCoroutine(uiManager.AttackState(false, "좋지 않은 것 같다..!"));
            }
            else
            {
                StartCoroutine(skill.hitEffect[1].ObjectSwitch(2));
                StartCoroutine(uiManager.AttackState(false, "좋다"));
            }
            Debug.Log(name + "(맞은 애)가 특수 대미지 피해를 받았다" + spDamage);
            Debug.Log(name + "(맞은 애)의 특수 공격력" + monsterSpAtt);
            Debug.Log(name + "(맞은 애)의 특수 방어력 : " + monsterSpDef);
            if (spDamage < 1)
                spDamage = 1;
            Hp -= spDamage;
            ani.SetTrigger("OnHit");
        }
    }

}

