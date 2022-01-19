using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professional : NPC
{
    MonsterInventory inven;
    [SerializeField] GameObject[] monsterPrefabs = new GameObject[3];
    int monsterNum = 0;
    public override void Start()
    {
        base.Start();
        inven = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetChild(1).GetComponent<MonsterInventory>();
    }

    public void GiveMonster()
    {
        count = 0;
        dialogCanvas.SetActive(false);
        inven.GetMonster(monsterPrefabs[monsterNum].GetComponentInChildren<Monster>());
        monsterNum++;
        playerInWorld.Money += 10;
        playerInWorld.cameraRotateSpeed = 4;
        playerInWorld.speed = 1;
    }
}
