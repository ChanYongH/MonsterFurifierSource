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
        isTrigger = true; // �����ڸ��� �浹�� ���� �ʰ� �ϱ� ���� �־� ��(isTrigger�� true������ �浹ó���� ��)
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Active>().isTrigger)
            transform.Translate(Vector3.down * 70); // ������ �̵�
    }

    private void OnDisable()
    {
        if (isTrigger) // ��Ȱ��ȭ�Ǹ� ��ġ�� ����ġ ��Ű�� isTrigger�� �ٽ� false, �ٽ� ��ť�� �����ش�.
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
