using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureBullet : MonoBehaviour
{
    [SerializeField]
    float bulletSpeed;
    BulletPooling bulletPool;

    // Start is called before the first frame update
    void OnEnable()
    {
        bulletPool = gameObject.GetComponentInParent<BulletPooling>();
        StartCoroutine(ActiveCo());
    }

    IEnumerator ActiveCo()
    {
        yield return new WaitForSecondsRealtime(2); // Æ÷È¹ ÃÑ¾Ë¸¸ Àû¿ë
        gameObject.SetActive(false);
        //bulletPool.pooling.Enqueue(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed);
    }
}
