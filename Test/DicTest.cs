using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicTest : MonoBehaviour
{
    Dictionary<int, string> dicTest = new Dictionary<int, string>();
    int level = 5;
    // Start is called before the first frame update
    void Start()
    {
        dicTest.Add(1, "mumbai");
        dicTest.Add(2, "newCity");
        dicTest.Add(4, "helloWorld");
        dicTest.Add(7, "helloWor");

        for (int i = level; i > level-5; i--)
        {
            if (dicTest.ContainsKey(i))
            {
                Debug.Log(dicTest[i]);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            level++;
    }
}
