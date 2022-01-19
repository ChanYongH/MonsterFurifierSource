using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    public GameObject player;
    public PlayerWorld playerInWorld;
    public PlayerBattle playerInBattle;
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
        player = GameObject.FindGameObjectWithTag("Player");
        playerInWorld = player.GetComponentInChildren<PlayerWorld>();
        playerInBattle = player.GetComponentInChildren<PlayerBattle>();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 신이 넘어갈 때 실행
    {
        GameObject enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster");
        if (scene.name == "BattleScene")
        {
            Transform playerMonsterPos = GameObject.FindGameObjectWithTag("PlayerMonsterPos").transform;
            Transform enemyMonsterPos = GameObject.FindGameObjectWithTag("EnemyMonsterPos").transform;
            player.transform.position = playerMonsterPos.position;
            enemyMonster.transform.position = enemyMonsterPos.position;
            playerInWorld.gameObject.SetActive(false);
            playerInBattle.gameObject.SetActive(true);
            playerInBattle.monsters[playerInBattle.equipMonster].SetActive(true);
        }
        else if (scene.name == "PlayerTest")
        {
            playerInWorld.gameObject.SetActive(true);
            enemyMonster.SetActive(false); // 몬스터 없애기
            playerInBattle.gameObject.SetActive(false);
        }
    }
}
