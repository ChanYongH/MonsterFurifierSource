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
        if (!playerMonster) // �� ���͸�
        {
            for(int i = level; i > level-8; i--)
            {
                // ���� - 8�� �ִ� ��ųʸ��� key���� �����ͼ� return�Ѵ�.
                // return �� string�� ���� ���ش�.
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
            //���ݷ� + ����, ����� ó�� + �����%
            monsterSpAtt = ((eMonster.spAtt + ((eMonster.skill.buff[(int)BuffList.spAtt]) - (skill.debuff[(int)BuffList.spAtt]))) * (_damage * 0.01f));
            spDamage = monsterSpAtt - monsterSpDef;
            if (eMonster.GetComponent<WaterMonster>() != null) // 1.5�� �����
            {
                spDamage *= 1.5f;
                StartCoroutine(skill.hitEffect[2].ObjectSwitch(2));
                StartCoroutine(uiManager.AttackState(false, "�����ϴ�!"));
            }
            else if (eMonster.GetComponent<NatureMonster>() != null) // 0.5�� �����
            {
                spDamage *= 0.5f;
                StartCoroutine(skill.hitEffect[3].ObjectSwitch(2));
                StartCoroutine(uiManager.AttackState(false, "���� ���� �� ����..!"));
            }
            else
            {
                StartCoroutine(skill.hitEffect[1].ObjectSwitch(2));
                StartCoroutine(uiManager.AttackState(false, "����"));
            }
            Debug.Log(name + "(���� ��)�� Ư�� ����� ���ظ� �޾Ҵ�" + spDamage);
            Debug.Log(name + "(���� ��)�� Ư�� ���ݷ�" + monsterSpAtt);
            Debug.Log(name + "(���� ��)�� Ư�� ���� : " + monsterSpDef);
            if (spDamage < 1)
                spDamage = 1;
            Hp -= spDamage;
            ani.SetTrigger("OnHit");
        }
    }

}

