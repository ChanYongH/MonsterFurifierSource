using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HormonBomb : Items
{
    public override void UseItem(Monster pMonster, Monster eMonster = null, UIManager uIManager = null, PlayerWorld playerInWorld = null)
    {
        itemDuration = 5;
        //ȣ���� ��ź�� �ִ� UseItem()���� �ű� ������
        uIManager.captureProgress = true; // ���� ����� ���� ���
        eMonster.hormone.SetActive(true); // ���Ϳ� �ִ� ȣ������ Ȱ��ȭ �����ش�.
        playerInWorld.equipMonster = 3; // ��ȹ �Ѿ˷� �ٲ��ش�.
        PlayerWorld.battleOut.Invoke();
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // �۾� �� ���� ���� ���� ��
        eMonster.transform.position = eMonster.monsterWorldPos.transform.position;
        //StartCoroutine(CaptureCo(item[index].itemDuration)); // ������ ���ӽð�(n�� �ڿ� �ٽ� ��Ʋ�� ���ƿ�) // �����ִ� ���·δ� ������ �ȵ�
    }
    
    // �Ϲ�ȭ�� �Ἥ ��Ȳ�� ���� �ٸ� Ŭ������ �ҷ��ͼ� �Ẹ�� ������ ����� ����
    //public override void UseItemGeneric<T, U, V>(T classObj, U classObj2, V classObj3) where T : class where U : class where V : class
    //{
    //    Type type = classObj.GetType();
    //    type.

    //    type.captureProgress = true; // ���� ����� ���� ���
    //    //ȣ���� ��ź�� �ִ� UseItem()���� �ű� ������
    //    eMonster.hormone.SetActive(true); // ���Ϳ� �ִ� ȣ������ Ȱ��ȭ �����ش�.
    //    playerInWorld.equipMonster = 3; // ��ȹ �Ѿ˷� �ٲ��ش�.
    //    PlayerWorld.battleOut.Invoke();
    //    Time.timeScale = 1;
    //    Time.fixedDeltaTime = 0.02f * Time.timeScale; // �۾� �� ���� ���� ���� ��
    //    eMonster.transform.position = eMonster.monsterWorldPos.transform.position;
    //    StartCoroutine(CaptureCo(2)); // ������ ���ӽð�(n�� �ڿ� �ٽ� ��Ʋ�� ���ƿ�)
    //}
}
