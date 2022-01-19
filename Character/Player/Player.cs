using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public int equipMonster = 0;
    public GameObject playerMonsters;
    public List<GameObject> monsters;

    public override void Dead()
    { }
    public virtual void OnHit()
    { }
    public override void OnCrisis()
    { }
    // Start is called before the first frame update
}
