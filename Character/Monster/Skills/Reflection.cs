using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Runtime;

public class Reflection : MonoBehaviour
{
    public string skillName;
    Type skills;
    MethodInfo skillMethod;
    public bool state;
    //RuntimeTypeHandle skills;
    // Start is called before the first frame update
    void Start()
    {
        skills = GetComponent("Reflection").GetType();
    }

    public void SkillDelete(bool _state)
    {
        state = _state;
    }

    public void CallReflection()
    {
        skillMethod = skills.GetMethod(skillName);
        skillMethod.Invoke(GetComponent("Reflection"), null);
    }

    public void FireAttack()
    {
        if (state)
            Debug.Log("�� ����!");
        else
            Debug.Log("�Ұ����� �̷����ϴ�.");
    }

    public void WaterAttack()
    {
        if (state)
            Debug.Log("�� ����!");
        else
            Debug.Log("�������� �̷����ϴ�.");
    }
    public void NomalAttack()
    {
        Debug.Log("�⺻ ����!");
    }

    public void NatureAttack()
    {
        Debug.Log("Ǯ ����!");
    }
    public void ChangeSkill(int select)
    {
        Debug.Log(select);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CallReflection();
        }
    }
}
