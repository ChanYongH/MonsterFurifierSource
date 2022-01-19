using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleQueue : MonoBehaviour
{
    public Transform battleCanvas;
    // ������
    public GameObject playerMonster;
    public GameObject enemyMonster;
    // ������Ʈ Ǯ��
    public Queue<GameObject> playerQueue = new Queue<GameObject>(); 
    public Queue<GameObject> enemyQueue = new Queue<GameObject>();
    // Ǯ�� Ƚ��(ǥ���ϴ� ���� ������)
    public int maxCount;

    public int pMonEndu; // �÷��̾� ������
    public int eMonEndu; // �� ������
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
        if (Input.GetKeyDown(KeyCode.Space)) // �� ���� ������ ��� �ߵ��Ǿ� ��(�׽�Ʈ ��)
            StartCoroutine(TimeCo(false));

    }

    IEnumerator TimeCo(bool first)
    {
        if (!first) // ó�� �ߵ��ϴ°� �ƴϸ�(������Ʈ�� ���� ���ִ� ������ ��)
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
            if (pMonEndu >= eMonEndu && pMonEndu > 0) // ������ ��
            {
                if (playerQueue.Count <= 0) // ť�� ���̻� ������
                    break;
                playerQueue.Peek().SetActive(true);
                playerQueue.Dequeue();
                pMonEndu -= 5; // ������ - ������
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
