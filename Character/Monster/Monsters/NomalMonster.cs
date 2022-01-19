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
            //���ݷ� + ����, ����� ó�� + �����%
            monsterSpAtt = ((eMonster.spAtt + ((eMonster.skill.buff[(int)BuffList.spAtt]) - (skill.debuff[(int)BuffList.spAtt]))) * (_damage * 0.01f));
            spDamage = monsterSpAtt - monsterSpDef;
            StartCoroutine(uiManager.AttackState(false, "����"));
            Debug.Log(name + "(���� ��)�� Ư�� ����� ���ظ� �޾Ҵ�" + spDamage);
            Debug.Log(name + "(���� ��)�� Ư�� ���ݷ�" + monsterSpAtt);
            Debug.Log(name + "(���� ��)�� Ư�� ���� : " + monsterSpDef);
            if (spDamage < 1)
                spDamage = 1;
            Hp -= spDamage;
        }
    }
}
