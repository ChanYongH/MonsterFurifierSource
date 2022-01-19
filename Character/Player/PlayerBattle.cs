using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBattle : Player
{
    public static Action MonsterChange;
    private void Start()
    {
        if (playerMonsters.transform.childCount != 0)
        {
            for (int i = 0; i < playerMonsters.transform.childCount; i++) // 플레이어의 몬스터가 씬으로 넘어갈 때 사용
                monsters.Add(playerMonsters.transform.GetChild(i).gameObject);
            for (int i = 0; i < monsters.Count; i++)
                monsters[i].SetActive(false);
        }
        MonsterChange += ChangeMonster;
    }

    void ChangeMonster()
    {
        // 몬스터를 순차적을 바꿔 줌
        if (equipMonster == 0) 
            equipMonster = 1;
        else if (equipMonster == 1)
            equipMonster = 2;
        else if (equipMonster == 2)
            equipMonster = 0;
    }
    public void SetEquipMonster()
    {
        monsters.Clear();
        for (int i = 0; i < playerMonsters.transform.childCount; i++) // 플레이어의 몬스터가 씬으로 넘어갈 때 사용
            monsters.Add(playerMonsters.transform.GetChild(i).gameObject);
        //for (int i = 0; i < monsters.Count; i++)
            //monsters[i].SetActive(false);
    }
    public override void Dead()
    { }
    public override void OnHit()
    { }
    public override void OnCrisis()
    { }
}
