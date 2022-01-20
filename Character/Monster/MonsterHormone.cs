using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHormone : MonoBehaviour
{
    UIManager uiManager;
    public float deltaSpeed;
    public float repeatTime;
    public float[] randomMove = new float[3];
    Vector3 randomPos;
    public int count = 0;
    Monster monster;

    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //Start���� �ʱ�ȭ�� ������ ������ �̻��� ������ ���ٰ� ���ڸ��� �´�. �� �ʱ�ȭ�� ������
        //�θ��� �����ǰ� ������ ���� �����༭ �������� �����̰��Ѵ�.
        randomMove[0] = transform.parent.position.x + Random.Range(-3f, 3f); 
        randomMove[1] = Random.Range(1f, 5f);
        randomMove[2] = transform.parent.position.z; //+ Random.Range(-0.5f, 0.5f);
        randomPos = new Vector3(randomMove[0], randomMove[1], transform.position.z);//transform.position.z + randomMove[2]);
        StartCoroutine(RandomMoveCo());
        monster = GetComponentInParent<Monster>();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, randomPos, Time.deltaTime * deltaSpeed); // �ε巴�� ������ �� �ְ� ����
        if (Input.GetKeyDown(KeyCode.Tab))
            StartCoroutine(RandomMoveCo());
    }
    IEnumerator RandomMoveCo()
    {
        count = 0;
        while (true)
        {
            yield return new WaitForSeconds(repeatTime);
            count++; 
            randomMove[0] = transform.parent.position.x + Random.Range(-3f, 3f);
            randomMove[1] = Random.Range(1f, 5f);
            randomMove[2] = transform.parent.position.z; //+ Random.Range(-0.5f, 0.5f);
            randomPos = new Vector3(randomMove[0], randomMove[1], transform.position.z); //transform.position.z + 0);//randomMove[2]);
            if (count > 20) // �ӽ÷� 20�� ���� 
                break;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CaptureBullet>() != null)
        {
            uiManager.captureState = true; // ĸó ����!
            monster.exp = monster.level * 10 + Random.Range(5, 10);
            Debug.Log("�¾Ҵ�!");
        }
    }
}
