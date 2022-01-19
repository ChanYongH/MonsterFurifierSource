using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletPooling : MonoBehaviour
{
    public Queue<GameObject> pooling;
    public int maxCount;
    [SerializeField] private GameObject bullet;
    private void Start()
    {
        pooling = new Queue<GameObject>();
        SetPooling();
    }
    public void SetPooling()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject bulletObj = Instantiate(bullet, transform);
            pooling.Enqueue(bulletObj);
            bullet.SetActive(false);
        }
    }
    public GameObject ShotBullet(Transform spot)
    {
        if(pooling.Count <= 0)
            return null;
        pooling.Peek().SetActive(true);
        pooling.Peek().transform.position = spot.position;
        pooling.Peek().transform.rotation = spot.rotation;

        return pooling.Dequeue();
    }
}
