using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManger : MonoBehaviour
{
    GameObject player;
    PlayerWorld playerInWorld;
    PlayerBattle playerInBattle;
    public Monster playerMonster;
    Skills playerMonsterSkill;
    public Monster enemyMonster;
    Skills enemyMonsterSkill;
    UIManager uiManager;
    public static bool battle;
    public GameObject battleZone;
    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInWorld = player.transform.GetChild(0).GetComponent<PlayerWorld>();
        playerInBattle = player.transform.GetChild(1).GetComponent<PlayerBattle>();
        PlayerWorld.Gobattle += BattleStart;
    }
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        PlayerWorld.battleOut += BattleEnd;
        PlayerBattle.MonsterChange += MonsterChangeOverrideBattle;
    }

    void MonsterChangeOverrideBattle()
    {
        for (int i = 0; i < playerInBattle.monsters.Count; i++)
            playerInBattle.monsters[i].SetActive(false);
        playerInBattle.monsters[playerInBattle.equipMonster].SetActive(true);
        playerMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
        Transform playerMonsterPos = GameObject.FindGameObjectWithTag("PlayerMonsterPos").transform;
        playerMonsterSkill = playerMonster.transform.GetChild(0).GetComponent<Skills>();
        player.transform.position = playerMonsterPos.position;
    }

    public void BattleStart() // ���� �Ѿ �� ����
    {
        battleZone.SetActive(true);
        playerInWorld.gameObject.SetActive(false);
        playerInBattle.gameObject.SetActive(true);
        for (int i = 0; i < playerInBattle.monsters.Count; i++)
            playerInBattle.monsters[i].SetActive(false);
        playerInBattle.monsters[playerInBattle.equipMonster].SetActive(true);
        playerInBattle.monsters[playerInBattle.equipMonster].transform.localPosition = new Vector3(0, 0, 0);
        playerInBattle.monsters[playerInBattle.equipMonster].transform.localScale = new Vector3(1, 1, 1);
        playerMonster = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<Monster>();
        enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();

        Transform playerMonsterPos = GameObject.FindGameObjectWithTag("PlayerMonsterPos").transform;
        Transform enemyMonsterPos = GameObject.FindGameObjectWithTag("EnemyMonsterPos").transform;
        playerMonsterSkill = playerMonster.transform.GetChild(0).GetComponent<Skills>();
        enemyMonsterSkill = enemyMonster.transform.GetChild(0).GetComponent<Skills>();
        player.transform.position = playerMonsterPos.position;
        enemyMonster.transform.position = enemyMonsterPos.position;
        player.transform.LookAt(enemyMonsterPos);
        enemyMonster.transform.LookAt(playerMonsterPos);
    }
    public void BattleEnd()
    {
        playerInWorld.gameObject.SetActive(true);
        for (int i = 0; i < playerInBattle.monsters.Count; i++)
            playerInBattle.monsters[i].SetActive(true);
        //enemyMonster.gameObject.SetActive(false); // ���� ���ֱ�
        //playerInBattle.gameObject.SetActive(false);
        battleZone.SetActive(false);
    }

    // 6�� °
    public void Battle() // ��ų�� ���� �ϰ� ����
    {
        int playerEndurance = playerMonster.endurance;
        int enemyEndurance = enemyMonster.endurance;
        bool turnJedge = playerEndurance >= enemyEndurance;
        if (enemyEndurance <= 0)
            enemyMonster.endurance = enemyMonster.maxEndurance;
        if (playerMonsterSkill.firstAttack)
            turnJedge = true;
        else if (enemyMonsterSkill.firstAttack)
            turnJedge = false;

        if (turnJedge) // �÷��̾� ������ �������� �� ������ �����º��� ũ��
        {
            playerMonster.endurance -= (playerMonster.agility - (int)playerMonsterSkill.buff[(int)BuffList.agility]);
            //���� �������� string - > monster
            StartCoroutine(uiManager.EnemyUseSkillCo(playerMonster, playerMonsterSkill.nameToKorean[uiManager.selectSkill]));
            //StartCoroutine(uiManager.EnemyUseSkillCo(playerMonster.monsterName, playerMonsterSkill.nameToKorean[uiManager.selectSkill]));
            //playerMonsterSkill.UseSkill();
        }
        else // �÷��̾� ������ �������� �� ������ �����º��� ������
        {
            enemyMonster.endurance -= (enemyMonster.agility - (int)enemyMonsterSkill.buff[(int)BuffList.agility]);
            //���� ��������
            StartCoroutine(uiManager.EnemyUseSkillCo(enemyMonster, enemyMonsterSkill.nameToKorean[uiManager.enemySetSkill]));
            //StartCoroutine(uiManager.EnemyUseSkillCo(enemyMonster.monsterName, enemyMonsterSkill.nameToKorean[uiManager.enemySetSkill]));
            //enemyMonsterSkill.UseSkill();
        }
        StartCoroutine(NextTurn(turnJedge));
    }
    public IEnumerator NextTurn(bool judge)
    {
        bool playerturnJedge = playerMonster.endurance >= enemyMonster.endurance;
        bool enemyturnJedge = playerMonster.endurance < enemyMonster.endurance; // ���߿� ���� �غ��� ��


        //yield return new WaitForSecondsRealtime(1);
        Debug.Log("�÷��̾�" + playerMonster.name);
        if (playerMonster.isDead)
        {
            yield return new WaitForSecondsRealtime(uiManager.flowTime);
            uiManager.BattleResult(0, 0, false);
            yield break;
        }
        if (enemyMonster.isDead) // �÷��̾ ������ �� ����
        {
            yield return new WaitForSecondsRealtime(uiManager.flowTime);
            uiManager.BattleResult(enemyMonster.exp, enemyMonster.money, true); // ���â(UI)�� �����ְ� // ���� �������� ������ ?
            yield break;
        }
        if(judge)
            yield return new WaitForSecondsRealtime(uiManager.flowTime * 2 + enemyMonsterSkill.skillUsingTime+1); // 3�ʵڿ� ������� ����
        else
            yield return new WaitForSecondsRealtime(uiManager.flowTime * 2 + playerMonsterSkill.skillUsingTime+1);

        if (judge)
        {
            if (playerturnJedge) // �÷��̾��� �������� �� ũ�� �� �� �� ����
            {
                uiManager.BattleStart();
                yield break;
            }
            enemyMonster.endurance -= (enemyMonster.agility - (int)enemyMonsterSkill.buff[(int)BuffList.agility]);
            StartCoroutine(uiManager.EnemyUseSkillCo(enemyMonster, enemyMonsterSkill.nameToKorean[uiManager.enemySetSkill]));
        }
        else
        {
            if (enemyturnJedge) // ���� �������� �� ũ�� �� �� �� ����
            {
                enemyMonster.endurance -= (enemyMonster.agility - (int)enemyMonsterSkill.buff[(int)BuffList.agility]);
                StartCoroutine(uiManager.EnemyUseSkillCo(enemyMonster, enemyMonsterSkill.nameToKorean[uiManager.enemySetSkill]));
                yield return new WaitForSecondsRealtime(uiManager.flowTime * 2); // 3�ʵڿ� �ٽ� ���� ���� ȭ������
                uiManager.BattleStart();
                yield break;
            }
            playerMonster.endurance -= (playerMonster.agility - (int)playerMonsterSkill.buff[(int)BuffList.agility]);
            //StartCoroutine(uiManager.EnemyUseSkillCo(playerMonster.monsterName, playerMonsterSkill.nameToKorean[uiManager.selectSkill]));
            StartCoroutine(uiManager.EnemyUseSkillCo(playerMonster, playerMonsterSkill.nameToKorean[uiManager.selectSkill]));
            //playerMonsterSkill.UseSkill();
        }
        yield return new WaitForSecondsRealtime(uiManager.flowTime+enemyMonsterSkill.skillUsingTime);
        if (playerMonster.endurance <= 0) // ���� ��� �Һ��ϰ� �� ����
        {
            StartCoroutine(uiManager.MonsterChangeCo());
            yield break;
        }
        if (enemyMonster.isDead) // �÷��̾ �İ��� �� ����
        {
            yield return new WaitForSecondsRealtime(uiManager.flowTime);
            uiManager.BattleResult(enemyMonster.exp, enemyMonster.money, true);
            yield break;
        }
        if (playerMonster.isDead)
        {
            yield return new WaitForSecondsRealtime(uiManager.flowTime);
            uiManager.BattleResult(0, 0, false);
            yield break;
        }
        if (judge) // �÷��̾ ���� �� ��(���� ���� ����
            yield return new WaitForSecondsRealtime(uiManager.flowTime + enemyMonsterSkill.skillUsingTime); // 3�ʵڿ� ������� ����
        else
            yield return new WaitForSecondsRealtime(uiManager.flowTime + playerMonsterSkill.skillUsingTime); //playerMonsterSkill.skillUsingTime


        uiManager.BattleStart();
    }

}
