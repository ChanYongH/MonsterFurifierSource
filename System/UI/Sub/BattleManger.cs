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

    public void BattleStart() // 신이 넘어갈 때 실행
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
        //enemyMonster.gameObject.SetActive(false); // 몬스터 없애기
        //playerInBattle.gameObject.SetActive(false);
        battleZone.SetActive(false);
    }

    // 6번 째
    public void Battle() // 스킬을 선택 하고 실행
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

        if (turnJedge) // 플레이어 몬스터의 지구력이 적 몬스터의 지구력보다 크면
        {
            playerMonster.endurance -= (playerMonster.agility - (int)playerMonsterSkill.buff[(int)BuffList.agility]);
            //여기 수정했음 string - > monster
            StartCoroutine(uiManager.EnemyUseSkillCo(playerMonster, playerMonsterSkill.nameToKorean[uiManager.selectSkill]));
            //StartCoroutine(uiManager.EnemyUseSkillCo(playerMonster.monsterName, playerMonsterSkill.nameToKorean[uiManager.selectSkill]));
            //playerMonsterSkill.UseSkill();
        }
        else // 플레이어 몬스터의 지구력이 적 몬스터의 지구력보다 작으면
        {
            enemyMonster.endurance -= (enemyMonster.agility - (int)enemyMonsterSkill.buff[(int)BuffList.agility]);
            //여기 수정했음
            StartCoroutine(uiManager.EnemyUseSkillCo(enemyMonster, enemyMonsterSkill.nameToKorean[uiManager.enemySetSkill]));
            //StartCoroutine(uiManager.EnemyUseSkillCo(enemyMonster.monsterName, enemyMonsterSkill.nameToKorean[uiManager.enemySetSkill]));
            //enemyMonsterSkill.UseSkill();
        }
        StartCoroutine(NextTurn(turnJedge));
    }
    public IEnumerator NextTurn(bool judge)
    {
        bool playerturnJedge = playerMonster.endurance >= enemyMonster.endurance;
        bool enemyturnJedge = playerMonster.endurance < enemyMonster.endurance; // 나중에 구현 해봐야 함


        //yield return new WaitForSecondsRealtime(1);
        Debug.Log("플레이어" + playerMonster.name);
        if (playerMonster.isDead)
        {
            yield return new WaitForSecondsRealtime(uiManager.flowTime);
            uiManager.BattleResult(0, 0, false);
            yield break;
        }
        if (enemyMonster.isDead) // 플레이어가 선공일 때 죽음
        {
            yield return new WaitForSecondsRealtime(uiManager.flowTime);
            uiManager.BattleResult(enemyMonster.exp, enemyMonster.money, true); // 결과창(UI)를 보여주고 // 만약 레벨업을 했으면 ?
            yield break;
        }
        if(judge)
            yield return new WaitForSecondsRealtime(uiManager.flowTime * 2 + enemyMonsterSkill.skillUsingTime+1); // 3초뒤에 상대편이 공격
        else
            yield return new WaitForSecondsRealtime(uiManager.flowTime * 2 + playerMonsterSkill.skillUsingTime+1);

        if (judge)
        {
            if (playerturnJedge) // 플레이어의 지구력이 더 크면 한 번 더 공격
            {
                uiManager.BattleStart();
                yield break;
            }
            enemyMonster.endurance -= (enemyMonster.agility - (int)enemyMonsterSkill.buff[(int)BuffList.agility]);
            StartCoroutine(uiManager.EnemyUseSkillCo(enemyMonster, enemyMonsterSkill.nameToKorean[uiManager.enemySetSkill]));
        }
        else
        {
            if (enemyturnJedge) // 적의 지구력이 더 크면 한 번 더 공격
            {
                enemyMonster.endurance -= (enemyMonster.agility - (int)enemyMonsterSkill.buff[(int)BuffList.agility]);
                StartCoroutine(uiManager.EnemyUseSkillCo(enemyMonster, enemyMonsterSkill.nameToKorean[uiManager.enemySetSkill]));
                yield return new WaitForSecondsRealtime(uiManager.flowTime * 2); // 3초뒤에 다시 전투 시작 화면으로
                uiManager.BattleStart();
                yield break;
            }
            playerMonster.endurance -= (playerMonster.agility - (int)playerMonsterSkill.buff[(int)BuffList.agility]);
            //StartCoroutine(uiManager.EnemyUseSkillCo(playerMonster.monsterName, playerMonsterSkill.nameToKorean[uiManager.selectSkill]));
            StartCoroutine(uiManager.EnemyUseSkillCo(playerMonster, playerMonsterSkill.nameToKorean[uiManager.selectSkill]));
            //playerMonsterSkill.UseSkill();
        }
        yield return new WaitForSecondsRealtime(uiManager.flowTime+enemyMonsterSkill.skillUsingTime);
        if (playerMonster.endurance <= 0) // 턴을 모두 소비하고 난 후임
        {
            StartCoroutine(uiManager.MonsterChangeCo());
            yield break;
        }
        if (enemyMonster.isDead) // 플레이어가 후공일 때 죽음
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
        if (judge) // 플레이어가 공격 한 후(몬스터 공격 차례
            yield return new WaitForSecondsRealtime(uiManager.flowTime + enemyMonsterSkill.skillUsingTime); // 3초뒤에 상대편이 공격
        else
            yield return new WaitForSecondsRealtime(uiManager.flowTime + playerMonsterSkill.skillUsingTime); //playerMonsterSkill.skillUsingTime


        uiManager.BattleStart();
    }

}
