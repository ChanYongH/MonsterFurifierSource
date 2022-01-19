using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpwaner : MonoBehaviour
{
    public GameObject monster;
    public List<float> randNum = new List<float>();
    public List<float> randNum2 = new List<float>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            randNum.Add(Random.Range(-45f, -30f));
            randNum2.Add(Random.Range(-1f, 3f));
            Vector3 rand = new Vector3(randNum[i], -49f, randNum2[i]);
            GameObject temp = Instantiate(monster);
            temp.transform.GetChild(0).localPosition = rand;
            temp.name += i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
