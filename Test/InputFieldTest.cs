using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldTest : MonoBehaviour
{
    public TMP_InputField input;
    public Button button;
    public string objName;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(ChangeName);
        //input.onValueChanged.AddListener(InputField);
    }

    void ChangeName()
    {
        objName = input.text;
    }
    //void InputField(string temp)
    //{
    //    objName = temp;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
