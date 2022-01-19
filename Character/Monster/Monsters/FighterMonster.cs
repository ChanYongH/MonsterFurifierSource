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
            //���ݷ� + ����, ����� ó�� + �����%
            monsterSpAtt = ((eMonster.spAtt + ((eMonster.skill.buff[(int)BuffList.spAtt]) - (skill.debuff[(int)BuffList.spAtt]))) * (_damage * 0.01f));
            spDamage = monsterSpAtt - monsterSpDef;
            Debug.Log(name + "(���� ��)�� Ư�� ����� ���ظ� �޾Ҵ�" + spDamage);
            Debug.Log(name + "(���� ��)�� Ư�� ���ݷ�" + monsterSpAtt);
            Debug.Log(name + "(���� ��)�� Ư�� ���� : " + monsterSpDef);
            if (spDamage < 1)
                spDamage = 1;
            StartCoroutine(uiManager.AttackState(false, "����"));
            Hp -= spDamage;
        }
    }
}
