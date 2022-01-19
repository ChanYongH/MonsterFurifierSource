using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float maxHp;
    public float hp;
    public float speed;
    public int money; // 플레이어 : 보유한 돈 / 몬스터 : 처치시 주는 골드
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
            Debug.Log(name+"의 현재 HP : " + Hp);
        }
    }


    public abstract void Dead();
    public abstract void OnCrisis();
}
