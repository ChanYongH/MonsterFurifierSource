using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCookie : Items
{
    public override void UseItem(Monster pMonster, Monster eMonster = null, UIManager uIManager = null, PlayerWorld playerInWorld = null)
    {
        useItemText = "체력을 30만큼 회복 했다!";
        pMonster.Hp += 30;
    }
}
