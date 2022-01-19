using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDrink : Items
{
    public override void UseItem(Monster pMonster, Monster eMonster = null, UIManager uIManager = null, PlayerWorld playerInWorld = null)
    {
        useItemText = "지구력을 모두 회복했다!";
        pMonster.endurance = pMonster.maxEndurance;
    }
}
