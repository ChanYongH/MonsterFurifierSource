using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCookie : Items
{
    public override void UseItem(Monster pMonster, Monster eMonster = null, UIManager uIManager = null, PlayerWorld playerInWorld = null)
    {
        useItemText = "ü���� 30��ŭ ȸ�� �ߴ�!";
        pMonster.Hp += 30;
    }
}
