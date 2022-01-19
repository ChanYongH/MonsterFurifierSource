using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerWorld : Player
{
    public GameObject mainCamera;
    [SerializeField] private GameObject originPos;
    [SerializeField] private GameObject aimModeCanvas;
    [SerializeField] private GameObject aimZone;
    [SerializeField] private Transform shotZone;
    public List<BulletPooling> bullets;
    public Camera battleCamera;
    public Camera inventoryCamera;
    Animator ani;
    bool aimState = false; // 에임모드를 빨리 했을 때 이상하게 작동해서 넣었음
    bool aimShotAble = false; // 오른쪽 마우스를 누르면서 왼쪽 마우스를 클릭하면 발사되게 하기위해서 사용 

    [SerializeField] public Transform charaterBody;
    [SerializeField] public Transform cameraArm;
    [SerializeField] public float cameraRotateSpeed;

    public static Action Gobattle; // 전투 시작
    public static Action battleOut; // 전투 종료

    public GameObject inven;
    public bool onInven;
    public PlayerBattle playerInBattle;

    bool onAim = false;

    //포획
    public Monster enemyMonster;
    public bool captureProgress = false;

    PlayerUIManager playerUI;

    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            playerUI.playerMoney.text = money + "G";
        }

    }

    private void Start()
    {
        hp = maxHp;
        ani = charaterBody.GetComponent<Animator>();
        playerUI = FindObjectOfType<PlayerUIManager>();
        aimModeCanvas = GameObject.FindGameObjectWithTag("AimMode").transform.GetChild(0).gameObject;
        bullets = new List<BulletPooling>();
        GameObject monsterSetting = GameObject.FindGameObjectWithTag("SetMonster");
        inven = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).gameObject;
        playerInBattle = transform.parent.GetChild(1).GetComponent<PlayerBattle>();

        Gobattle += PlayerGoBattle;
        battleOut += BattleOut;

        for (int i = 0; i< monsterSetting.transform.childCount; i++)
            bullets.Add(monsterSetting.transform.GetChild(i).GetComponent<BulletPooling>());
        bullets.Add(transform.GetChild(0).GetChild(2).GetComponentInChildren<BulletPooling>());
        money = 1000;

    }
    void Update()
    {
        PlayerMove();
        AimMode();
        LookAround();
        SettingMonster();
        Interaction();
        InventoryShow();
        //if()
    }
    public override void Dead()
    { }
    public override void OnHit()
    { }
    public override void OnCrisis()
    { }
    public void PlayerGoBattle()
    {
        mainCamera.GetComponent<Camera>().targetDisplay = 1;
        aimModeCanvas.SetActive(false);
        battleCamera.targetDisplay = 0;
        AimState(false);
        playerUI.gameObject.SetActive(false);
        //for(int i = 0; i< bullets[3].transform.childCount; i++)
        //{
        //    bullets[3].pooling.Enqueue(bullets[3].transform.GetChild(i).gameObject);
        //}
    }

    void AimState(bool state)
    {
        aimShotAble = state;
        aimState = state;
        onAim = state;
    }
    public void BattleOut()
    {
        playerUI.gameObject.SetActive(true);
        for (int i = 0; i < bullets[3].transform.childCount; i++)
        {
            if(bullets[3].pooling.Count < 10)
            bullets[3].pooling.Enqueue(bullets[3].transform.GetChild(i).gameObject);
        }
        mainCamera.GetComponent<Camera>().targetDisplay = 0;
        battleCamera.targetDisplay = 1;
        GameObject originPos = GameObject.FindGameObjectWithTag("PlayerWorldPos");
        transform.parent.position = originPos.transform.position;
        transform.localPosition = new Vector3(0,0,0);
        if (GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle") != null)
            enemyMonster = GameObject.FindGameObjectWithTag("EnemyMonsterOnBattle").GetComponent<Monster>();
        mainCamera.transform.localPosition = new Vector3(0, 0, 0);
        mainCamera.transform.localEulerAngles = new Vector3(28, 3, 0);
        cameraArm.transform.localPosition = new Vector3(-0.1f, 0.35f, 0.1f);
        cameraArm.transform.localEulerAngles = new Vector3(0, 0, 0);
        
        //왜있지?
        //for (int i = 0; i < playerInBattle.monsters.Count; i++)
        //{
        //    playerInBattle.transform.GetChild(0).GetChild(i).tag = "PlayerMonster";
        //    playerInBattle.transform.GetChild(0).GetChild(i).GetComponent<Monster>().playerMonster = true;
        //}
        //if (playerInBattle.monsters.Count > 0) // 인벤토리를 열면 잠깐 장착한 몬스터의 위치를 변경해주고, 크기를 변경 해준다.
        //{
        //    for (int i = 0; i < playerInBattle.monsters.Count; i++)
        //    {
        //        playerInBattle.monsters[i].transform.localPosition = 
        //        new Vector3(transform.parent.localPosition.x + 1542.90002f, transform.parent.localPosition.y
        //         + 6813f, transform.parent.localPosition.z + 101.199997f);
        //        playerInBattle.monsters[i].transform.localScale = new Vector3(15f, 15f, 15f);
        //    }
        //}
    }
    //public void PlayerInWorld()
    //{
    //    mainCamera.transform.localPosition = new Vector3(-0.3f, charaterBody.transform.localPosition.y + 3f, -3.5f);
    //    mainCamera.transform.localEulerAngles = new Vector3(28, 3, 0);
    //    cameraArm.transform.localPosition = new Vector3(-0.1f, 0.35f, 0.1f);
    //    cameraArm.transform.localEulerAngles = new Vector3(0, 0, 0);
    //    aimModeCanvas = GameObject.FindGameObjectWithTag("AimMode").transform.GetChild(0).gameObject;
    //}
    //public void OnSceneLoadedPlayer(Scene scene, LoadSceneMode mode)
    //{
    //}

    public void InventoryShow()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!onInven)
            {
                inven.SetActive(true);
                inven.transform.GetChild(0).GetComponentInChildren<ItemInventory>().ShowItemList();
                playerInBattle.gameObject.SetActive(true);
                for (int i = 0; i < playerInBattle.transform.GetChild(0).childCount; i++)
                    playerInBattle.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                inventoryCamera.targetDisplay = 0;
                Camera.main.targetDisplay = 2;
                MonsterInventory monsterInven = inven.transform.GetChild(1).GetComponent<MonsterInventory>();
                if (monsterInven.equipMonsterChange != null)
                {
                    monsterInven.equipMonsterChange.SetActive(false);
                    for(int i = 0; i < monsterInven.monsterChangeButton.Length; i++)
                        monsterInven.monsterChangeButton[i].SetActive(false);
                }
                
            }
            else
            {
                inven.SetActive(false);
                inventoryCamera.targetDisplay = 2;
                Camera.main.targetDisplay = 0;
            }
            StartCoroutine(OnOffCo(onInven, 0.1f));
        }
    }
    IEnumerator OnOffCo(bool state, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        onInven = !state;
    }
    public void Interaction() // NPC와의 대화에 사용
    {
        RaycastHit hit;
        float maxDistance = 1f;
        //Debug.DrawRay(new Vector3(charaterBody.position.x, 1.5f, charaterBody.position.z), new Vector3(charaterBody.forward.x, 0.5f, charaterBody.forward.z) * maxDistance, Color.blue, 0.5f);
        if (Physics.Raycast(new Vector3(charaterBody.position.x, 1.2f, charaterBody.position.z), 
            new Vector3(charaterBody.forward.x,0.5f, charaterBody.forward.z), out hit, maxDistance))
        {
            if (hit.transform.GetComponent<NPC>() != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    hit.transform.GetComponent<NPC>().Talk();
                    cameraRotateSpeed = 0;
                    speed = 0;
                    Debug.Log("상점 주인한테 말걸었다!");
                }
            }
        }
    }
    void PlayerMove()
    {
        //Debug.DrawRay(cameraArm.position, cameraArm.forward, Color.red);
        //Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z), Color.red);
        //수평으로 맞춰주기 위해서 새로 작성
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
        bool isMove = moveInput.magnitude != 0;
        bool moveKey = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A);
        bool backKey = Input.GetKey(KeyCode.S);
        ani.SetBool("isWalk", moveKey);
        ani.SetBool("isBackWalk", backKey);
        if (speed != 0) // speed가 0이되면 이동 못함(대화할 때 사용)
        {
                if (isMove)
            {
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                charaterBody.forward = moveDir;
                transform.position += moveDir * Time.deltaTime * 5f;
            }
        }
        
    }
    void AimMode()
    {
        Vector3 originRot = originPos.transform.eulerAngles;
        if (Input.GetMouseButtonDown(1))
        {
            AimState(true);
            aimModeCanvas.SetActive(true);
            mainCamera.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, aimZone.transform.position, Time.deltaTime);
            //StartCoroutine(AimModeCo());
            //cameraRotateSpeed += 1;
        }
        if (onAim) // 에임 
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, aimZone.transform.position, Time.deltaTime * 10);
        if (Input.GetMouseButtonUp(1))
        {
            AimState(false);
            aimModeCanvas.SetActive(false);
            mainCamera.transform.localPosition = new Vector3(-0.3f, charaterBody.transform.localPosition.y + 4f, -3.5f);
            mainCamera.transform.localEulerAngles = new Vector3(28f, 3f, 0);
        }
        //Vector3 shotMode = new Vector3(shotZone.forward.x, shotZone.position.y, shotZone.forward.z);
        if (Input.GetMouseButtonDown(0) && aimShotAble) // 총 발사
        {
            Debug.Log((equipMonster+1) + "번 몬스터 발사 남은 총알 갯수 : " + bullets[equipMonster].pooling.Count);
            bullets[equipMonster].ShotBullet(shotZone); // shotzone도 돌아가야 함
            if (equipMonster < 3)
            {
                for (int i = 0; i < playerUI.monsterBullet[equipMonster].transform.childCount; i++) // 
                {
                    playerUI.monsterBullet[equipMonster].transform.GetChild(i).gameObject.SetActive(false);
                }

                for (int i = 0; i < bullets[equipMonster].pooling.Count; i++)
                {
                    playerUI.monsterBullet[equipMonster].transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            if (captureProgress) // 포획 아이템을 사용 하고 있으면
            {
                for (int i = 0; i < bullets[3].maxCount; i++) // 남은 총알 갯수 초기화
                    enemyMonster.hormone.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                for (int i = 0; i < bullets[3].pooling.Count; i++) // 남은 총알 갯수 갱신
                    enemyMonster.hormone.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    void LookAround()
    {
        // 이전값과 현재 값의 차이를 델타라고 함.
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y * cameraRotateSpeed; // 상하 움직임 제한하기위해서 사용

        //Quaternion으로 변환하지 않고 바로 eulerAngles로 변환해서 사용 가능
        //밸류만큼 회전 하라는 뜻
        if (cameraRotateSpeed != 0) // 카메라 회전이 0이면 마우스로 카메라를 이동시키지 못함
        {
            if (x < 180)
            {
                x = Mathf.Clamp(x, -1f, 70f); //90
            }
            else
            {
                x = Mathf.Clamp(x, 300f, 361f);
            }
        }
        else
            x = 0;
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x * cameraRotateSpeed, camAngle.z);
        //여기서 카메라 회전을 아래, 위로 반전 시켜줄 수 있는 설정을 넣어줄 수 있음
        //mainCamera.transform.LookAt(charaterBody);
        if(!aimState)
            mainCamera.transform.localPosition = new Vector3(-0.3f, charaterBody.transform.localPosition.y + 4f,-3.5f);
        //camAngle.x - mouseDelta.y * cameraRotateSpeed
    }
    void SettingMonster()
    {
        Dictionary<int, KeyCode> keyDic;
        keyDic = new Dictionary<int, KeyCode>();
        keyDic.Add(0, KeyCode.Alpha1);
        keyDic.Add(1, KeyCode.Alpha2);
        keyDic.Add(2, KeyCode.Alpha3);
        if (!captureProgress)
        {
            for (int i = 0; i < playerInBattle.monsters.Count; i++) // 현재 장착한 몬스터의 갯수
                if (Input.GetKeyDown(keyDic[i])) 
                {
                    equipMonster = i;
                    for (int j = 0; j < playerUI.selectMonster.Length; j++)
                    {
                        playerUI.selectMonster[j].SetActive(false); // 선택 시, 파티클을 발생시키는 객체를 모두 꺼준다.
                        playerUI.equipMonsters[j].GetChild(2).gameObject.SetActive(false); // 몬스터 총알 수를 보여주는 객체를 모두 꺼준다.
                    }
                    playerUI.selectMonster[equipMonster].SetActive(true); // 선택 된 몬스터만 파티클이 발생하게
                    playerUI.equipMonsters[equipMonster].GetChild(2).gameObject.SetActive(true); // 선택 된 몬스터만 총알이 보이게
                }
        }
    }
}
