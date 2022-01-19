using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;

public class NPC : MonoBehaviour
{
    public string npcTitle; // ���Ǿ��� Ÿ��Ʋ(���� �� ����)
    public string npcName; // ���Ǿ��� �̸�
    [SerializeField] protected List<string> dialog =  new List<string>(); // �ν����� â���� ��ȭ ���� ���� �� �� ����

    // ��ȭ â
    protected GameObject dialogCanvas;
    protected TextMeshProUGUI dialogText; // NPC���� ��ȭ�� �� �� ������ �޼����� ���⼭ ó��
    protected Button dialogButton;
    protected Transform npcPanel;
    protected TextMeshProUGUI[] npcPanelText = new TextMeshProUGUI[2]; // 0 : Ÿ��Ʋ 1: �̸�
    public int count = 0;
    //public bool talk = false;
    
    // ��ó�� ���� �ߵ�
    public GameObject rayObj;
    public TextMeshProUGUI rayText;

    // �� �κ� �����غ��� 
    // ���÷���? ���������� ����? �������� ������
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
        dialogCanvas.SetActive(true); // ��ȭ â�� ����
        if (dialog.Count > count)
        {
            dialogText.text = dialog[count]; // Ŭ���� �ϸ� ���� ��ȭ â���� �Ѿ count�� dialog�� ����Ʈ ���� �ʰ��ϸ� ���� �̺�Ʈ��
            count++;
        }
        else
        {
            action = npc.GetMethod(actionEvent); // NPC�� �´� �Լ����� �����ָ� �� �Լ��� ���� ��
            action.Invoke(GetComponent<NPC>(), null);
        }

    }
    public virtual void OnTriggerEnter(Collider other) // ���� NPC ��ó�� ����
    {
        if (other.GetComponent<PlayerWorld>() != null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            rayText.text = "F�� ���� " + npcName + "�� ��ȭ �ϼ���";
        }
    }
    public virtual void OnTriggerExit(Collider other) // NPC ��ó���� ���������� �Ǹ�
    {
        if (other.GetComponent<PlayerWorld>() != null)
            transform.GetChild(0).gameObject.SetActive(false);
    }

}
