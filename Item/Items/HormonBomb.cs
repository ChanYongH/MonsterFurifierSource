using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HormonBomb : Items
{
    public override void UseItem(Monster pMonster, Monster eMonster = null, UIManager uIManager = null, PlayerWorld playerInWorld = null)
    {
        itemDuration = 5;
        //호르몬 폭탄에 있는 UseItem()으로 옮길 예정임
        uIManager.captureProgress = true; // 실패 출력을 위해 사용
        eMonster.hormone.SetActive(true); // 몬스터에 있는 호르몬을 활성화 시켜준다.
        playerInWorld.equipMonster = 3; // 포획 총알로 바꿔준다.
        PlayerWorld.battleOut.Invoke();
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // 작아 질 수록 빨리 연산 됨
        eMonster.transform.position = eMonster.monsterWorldPos.transform.position;
        //StartCoroutine(CaptureCo(item[index].itemDuration)); // 아이템 지속시간(n초 뒤에 다시 배틀로 돌아옴) // 꺼져있는 상태로는 실행이 안됨
    }
    
    // 일반화를 써서 상황에 따라 다른 클래스를 불러와서 써보려 했지만 장렬히 전사
    //public override void UseItemGeneric<T, U, V>(T classObj, U classObj2, V classObj3) where T : class where U : class where V : class
    //{
    //    Type type = classObj.GetType();
    //    type.

    //    type.captureProgress = true; // 실패 출력을 위해 사용
    //    //호르몬 폭탄에 있는 UseItem()으로 옮길 예정임
    //    eMonster.hormone.SetActive(true); // 몬스터에 있는 호르몬을 활성화 시켜준다.
    //    playerInWorld.equipMonster = 3; // 포획 총알로 바꿔준다.
    //    PlayerWorld.battleOut.Invoke();
    //    Time.timeScale = 1;
    //    Time.fixedDeltaTime = 0.02f * Time.timeScale; // 작아 질 수록 빨리 연산 됨
    //    eMonster.transform.position = eMonster.monsterWorldPos.transform.position;
    //    StartCoroutine(CaptureCo(2)); // 아이템 지속시간(n초 뒤에 다시 배틀로 돌아옴)
    //}
}
