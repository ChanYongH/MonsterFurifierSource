using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDoctor : NPC
{
    bool inPlayer = false;
    //PlayerWorld playerInWorld; //이거 
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
    public void CureMonster()
    {
        count = 0;
        dialogCanvas.SetActive(false);
        playerInWorld.Money += 10;
        playerInWorld.cameraRotateSpeed = 4;
        playerInWorld.speed = 1;
        for (int i = 0; i < playerInWorld.bullets.Count - 1; i++)
        {
            playerInWorld.bullets[i].pooling.Clear();
            for (int j= 0; j < playerInWorld.bullets[i].maxCount; j++)
                playerInWorld.bullets[i].pooling.Enqueue(playerInWorld.bullets[i].transform.GetChild(j).gameObject);
        }

        for (int i = 0; i < playerInWorld.playerInBattle.monsters.Count; i++)
            playerInWorld.playerInBattle.monsters[i].GetComponent<Monster>().Hp =
                playerInWorld.playerInBattle.monsters[i].GetComponent<Monster>().MaxHp;
        Debug.Log("몬스터 치료소에 왔다!");
        
    }

    public override void OnTriggerEnter(Collider other) // 만약 NPC 근처로 오면
    {
        base.OnTriggerEnter(other);
        if(other.GetComponent<PlayerWorld>() != null)
            playerInWorld = other.GetComponent<PlayerWorld>();
    }
    public override void OnTriggerExit(Collider other) // NPC 근처에서 빠져나가게 되면
    {
        base.OnTriggerExit(other);
        
    }
}
