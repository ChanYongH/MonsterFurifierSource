using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float maxHp;
    public float hp;
    public float speed;
    public int money; // �÷��̾� : ������ �� / ���� : óġ�� �ִ� ���
    public float MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;

        }
    }
    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (Hp <= 0)
                Dead();
            Debug.Log(name+"�� ���� HP : " + Hp);
        }
    }


    public abstract void Dead();
    public abstract void OnCrisis();
}
