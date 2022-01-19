using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rBody;
    [SerializeField]
    float bulletSpeed;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(ActiveCo());
    }

    IEnumerator ActiveCo()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed);
    }
}
