using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    RectTransform pos;
    public UIManager battleQueue;
    public bool isTrigger = false;
    void OnEnable()
    {
        pos = GetComponent<RectTransform>();
        battleQueue = FindObjectOfType<UIManager>();
        //Time.fixedDeltaTime = 0.02f * Time.timeScale;
        StartCoroutine(TimeCo());
    }
    
    IEnumerator TimeCo()
    {
        yield return new WaitForSecondsRealtime(0.17f);
        isTrigger = true; // 나오자마자 충돌을 하지 않게 하기 위해 넣어 줌(isTrigger가 true여야지 충돌처리가 됨)
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Active>().isTrigger)
            transform.Translate(Vector3.down * 70); // 밑으로 이동
    }

    private void OnDisable()
    {
        if (isTrigger) // 비활성화되면 위치를 원위치 시키고 isTrigger는 다시 false, 다시 인큐를 시켜준다.
        {
            isTrigger = false;
            pos.anchoredPosition = new Vector3(-793, 502, 0);
            if (name == "PlayerMonster(Clone)")
                battleQueue.playerQueue.Enqueue(gameObject);
            else
                battleQueue.enemyQueue.Enqueue(gameObject);
        }

    }
}
