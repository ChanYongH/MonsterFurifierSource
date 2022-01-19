using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;

public class NPC : MonoBehaviour
{
    public string npcTitle; // 엔피씨의 타이틀(직업 및 역할)
    public string npcName; // 엔피씨의 이름
    [SerializeField] protected List<string> dialog =  new List<string>(); // 인스펙터 창에서 대화 량을 정해 줄 수 있음

    // 대화 창
    protected GameObject dialogCanvas;
    protected TextMeshProUGUI dialogText; // NPC와의 대화를 할 때 나오는 메세지를 여기서 처리
    protected Button dialogButton;
    protected Transform npcPanel;
    protected TextMeshProUGUI[] npcPanelText = new TextMeshProUGUI[2]; // 0 : 타이틀 1: 이름
    public int count = 0;
    //public bool talk = false;
    
    // 근처에 오면 발동
    public GameObject rayObj;
    public TextMeshProUGUI rayText;

    // 이 부분 생각해보기 
    // 리플렉션? 다형성으로 구현? 다형성이 나을듯
    Type npc;
    MethodInfo action;
    public string actionEvent;

    protected PlayerWorld playerInWorld;

    // Start is called before the first frame update
    public virtual void Start()
    {
        playerInWorld = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<PlayerWorld>();
        //dialogCanvas = GameObject.FindGameObjectWithTag("Dialog").transform.GetChild(0).gameObject;
        dialogCanvas = transform.GetChild(1).GetChild(0).gameObject;
        dialogText = dialogCanvas.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        dialogButton = dialogCanvas.transform.GetChild(0).GetComponentInChildren<Button>();
        dialogButton.onClick.AddListener(Talk);

        npcPanel = transform.GetChild(3);
        npcPanelText[0] = npcPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        npcPanelText[1] = npcPanel.GetChild(2).GetComponent<TextMeshProUGUI>();

        npcPanelText[0].text = npcTitle;
        npcPanelText[1].text = npcName;

        rayObj = transform.GetChild(0).GetChild(0).gameObject;
        rayText = rayObj.GetComponentInChildren<TextMeshProUGUI>();
        npc = GetComponent<NPC>().GetType();
        
    }

    public virtual void Talk()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        dialogCanvas.SetActive(true); // 대화 창이 켜짐
        if (dialog.Count > count)
        {
            dialogText.text = dialog[count]; // 클릭을 하면 다음 대화 창으로 넘어감 count가 dialog의 리스트 수를 초과하면 다음 이벤트로
            count++;
        }
        else
        {
            action = npc.GetMethod(actionEvent); // NPC에 맞는 함수명을 적어주면 그 함수를 실행 함
            action.Invoke(GetComponent<NPC>(), null);
        }

    }
    public virtual void OnTriggerEnter(Collider other) // 만약 NPC 근처로 오면
    {
        if (other.GetComponent<PlayerWorld>() != null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            rayText.text = "F를 눌러 " + npcName + "과 대화 하세요";
        }
    }
    public virtual void OnTriggerExit(Collider other) // NPC 근처에서 빠져나가게 되면
    {
        if (other.GetComponent<PlayerWorld>() != null)
            transform.GetChild(0).gameObject.SetActive(false);
    }

}
