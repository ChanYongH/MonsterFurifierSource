using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NatureMonster : Monster
{
    public override void Start()
    {
        base.Start();
        #region NatureSkills(Dic)
        getSkill.Keys.ToList().ForEach(key =>
        {
            getSkill[14] = "NatureDoubleDefBuff";
            getSkill[17] = "NatureAttack3";
        });
        getSkill.Add(5, "NatureAttack1");
        getSkill.Add(8, "NatureHpRecovery");
        getSkill.Add(11, "NatureAttack2");
        getSkill.Add(19, "NatureSpAttBuff");
        getSkill.Add(22, "NatureAttack4");
        #endregion
        if (!playerMonster)
        {
           for (int i = level; i > level - 8; i--)
           {
               if (getSkill.ContainsKey(i) && equipSkill.Count < 3)
                   equipSkill.Add(getSkill[i]);
           }
        }
        //ani = GetComponent<Animator>();

    }
    public override void MonsterOnHit(Monster eMonster, float _damage, bool special)
    {
        base.MonsterOnHit(eMonster, _damage, special);
        if (special)
        {
            float spDamage;
            float monsterSpDef;
            float monsterSpAtt;
            monsterSpDef = (((spDef + (skill.buff[(int)BuffList.spDef]) - (eMonster.skill.debuff[(int)BuffList.spDef])) * 0.1f));
            //공격력 + 버프, 디버프 처리 + 대미지%
            monsterSpAtt = ((eMonster.spAtt + ((eMonster.skill.buff[(int)BuffList.spAtt]) - (skill.debuff[(int)BuffList.spAtt]))) * (_damage * 0.01f));
            spDamage = monsterSpAtt - monsterSpDef;
            if (eMonster.GetComponent<FireMonster>() != null) // 1.5배 대미지
            {
                spDamage *= 1.5f;
                StartCoroutine(skill.hitEffect[1].ObjectSwitch(2));
                StartCoroutine(uiManager.AttackState(false, "굉장하다!"));
            }
            else if (eMonster.GetComponent<WaterMonster>() != null) // 0.5배 대미지
            {
                StartCoroutine(skill.hitEffect[2].ObjectSwitch(2));
                spDamage *= 0.5f;
                StartCoroutine(uiManager.AttackState(false, "좋지 않은 것 같다..!"));
            }
            else
            {
                StartCoroutine(skill.hitEffect[3].ObjectSwitch(2));
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
