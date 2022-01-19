using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleQueue : MonoBehaviour
{
    public Transform battleCanvas;
    // 프리팹
    public GameObject playerMonster;
    public GameObject enemyMonster;
    // 오브젝트 풀링
    public Queue<GameObject> playerQueue = new Queue<GameObject>(); 
    public Queue<GameObject> enemyQueue = new Queue<GameObject>();
    // 풀링 횟수(표시하는 량이 많아짐)
    public int maxCount;

    public int pMonEndu; // 플레이어 지구력
    public int eMonEndu; // 적 지구력
    void Start()
    {
        SetQueue(); 
        StartCoroutine(TimeCo(true));
    }
    void SetQueue()
    {
        for(int i = 0; i < maxCount; i++)
        {
            GameObject pMon = Instantiate(playerMonster, battleCanvas.GetChild(0));
            GameObject eMon = Instantiate(enemyMonster, battleCanvas.GetChild(1));
            pMon.SetActive(false);
            eMon.SetActive(false);
            playerQueue.Enqueue(pMon);
            enemyQueue.Enqueue(eMon);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 한 턴이 끝나면 계속 발동되야 함(테스트 용)
            StartCoroutine(TimeCo(false));

    }

    IEnumerator TimeCo(bool first)
    {
        if (!first) // 처음 발동하는게 아니면(오브젝트를 전부 꺼주는 역할을 함)
        {
            for (int i = 0; i < battleCanvas.GetChild(0).childCount; i++)
            {
                battleCanvas.GetChild(0).GetChild(i).gameObject.SetActive(false);
                battleCanvas.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
        }
        while (true)
        {
            yield return new WaitForSeconds(0.22f);
            if (pMonEndu >= eMonEndu && pMonEndu > 0) // 지구력 비교
            {
                if (playerQueue.Count <= 0) // 큐가 더이상 없으면
                    break;
                playerQueue.Peek().SetActive(true);
                playerQueue.Dequeue();
                pMonEndu -= 5; // 지구력 - 날렵함
            }
            else if (pMonEndu < eMonEndu && eMonEndu > 0)
            {
                if (enemyQueue.Count <= 0)
                    break;
                enemyQueue.Peek().SetActive(true);
                enemyQueue.Dequeue();
                eMonEndu -= 10;
            }
            else
                break;
        }
    }
}
