using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FighterMonster : Monster
{
    public override void Start()
    {
        base.Start();
        #region FighterSkills(Dic)
        getSkill.Keys.ToList().ForEach(key =>
        {
            getSkill[14] = "FighterDefDebuff";
            getSkill[17] = "FighterAttack3";
        });
        getSkill.Add(5, "FighterAttack1");
        getSkill.Add(8, "FighterAttBuff");
        getSkill.Add(11, "FighterAttack2");
        getSkill.Add(18, "FighterAttack4");
        getSkill.Add(19, "FighterAttDefBuff");
        #endregion
        if (!playerMonster)
        {
            for (int i = level; i > level - 8; i--)
            {
                if (getSkill.ContainsKey(i) && equipSkill.Count < 3)
                    equipSkill.Add(getSkill[i]);
            }
        }
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
            Debug.Log(name + "(맞은 애)가 특수 대미지 피해를 받았다" + spDamage);
            Debug.Log(name + "(맞은 애)의 특수 공격력" + monsterSpAtt);
            Debug.Log(name + "(맞은 애)의 특수 방어력 : " + monsterSpDef);
            if (spDamage < 1)
                spDamage = 1;
            StartCoroutine(uiManager.AttackState(false, "좋다"));
            Hp -= spDamage;
        }
    }
}
