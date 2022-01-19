using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    PlayerWorld playerInWorld;
    UIManager uiManager;
    TextMeshProUGUI playerName;
    public List<GameObject> playerHpObj = new List<GameObject>();
    public TextMeshProUGUI playerMoney;
    public Transform playerInfo;
    public Transform[] equipMonsters = new Transform[3];
    public Image[] monsterAttribute = new Image[3];
    public Transform[] monsterBullet = new Transform[3];
    public List<GameObject> bulletUI = new List<GameObject>();
    public GameObject[] selectMonster = new GameObject[3];
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        playerInWorld = FindObjectOfType<PlayerWorld>();
        playerName = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        playerInfo = transform.GetChild(1);
        playerMoney = playerInfo.GetChild(3).GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < playerInfo.GetChild(0).childCount; i++)
            playerHpObj.Add(playerInfo.GetChild(0).GetChild(i).gameObject);
        for(int i = 0; i < playerInWorld.maxHp;i++)
            playerHpObj[i].SetActive(true);
        for (int i = 0; i < equipMonsters.Length; i++)
        {
            equipMonsters[i] = transform.GetChild(i+2).transform;
            // ���� �Ѿ�
            monsterBullet[i] = equipMonsters[i].GetChild(2);
            // ���� �Ӽ��� ���� ������ ������ �̹����� ������ �޶����� ���� �� ����
            monsterAttribute[i] = equipMonsters[i].GetChild(0).GetComponent<Image>();
            // �����ϸ� �߻��ϴ� ��ƼŬ
            selectMonster[i] = equipMonsters[i].GetChild(3).gameObject;
        }
        playerMoney.text = playerInWorld.money+"G";
        if (UIManager.setPlayerName != null)
            playerName.text = UIManager.setPlayerName;
        else
            playerName.text = "ǻ�����̾�";
    }
}
