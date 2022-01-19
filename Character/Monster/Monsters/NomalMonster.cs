using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalMonster : Monster
{
    public override void Start()
    {
        base.Start();
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
            StartCoroutine(uiManager.AttackState(false, "좋다"));
            Debug.Log(name + "(맞은 애)가 특수 대미지 피해를 받았다" + spDamage);
            Debug.Log(name + "(맞은 애)의 특수 공격력" + monsterSpAtt);
            Debug.Log(name + "(맞은 애)의 특수 방어력 : " + monsterSpDef);
            if (spDamage < 1)
                spDamage = 1;
            Hp -= spDamage;
        }
    }
}
