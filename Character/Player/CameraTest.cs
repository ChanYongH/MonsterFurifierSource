using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraTest : MonoBehaviour
{
    public GameObject Player;
    public GameObject MainCamera;

    private float camera_dist = 0f; //���׷κ��� ī�޶������ �Ÿ�
    public float camera_width = -10f; //���ΰŸ�
    public float camera_height = 4f; //���ΰŸ�

    Vector3 dir;
    void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
        //MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //ī�޶󸮱׿��� ī�޶������ ����
        camera_dist = Mathf.Sqrt(camera_width * camera_width + camera_height * camera_height);

        //ī�޶󸮱׿��� ī�޶���ġ������ ���⺤��
        dir = new Vector3(0, 0, camera_width).normalized;

    }


    void Update()
    {
        //transform.position = Player.transform.position;
        //����ĳ��Ʈ�� ���Ͱ�
        Vector3 ray_target = transform.forward * camera_width;

        RaycastHit hitinfo;
        Physics.Raycast(transform.position, ray_target, out hitinfo, camera_width);

        if (hitinfo.point != Vector3.zero)//�����ɽ�Ʈ ������
        {
            //point�� �ű��.
            transform.position = hitinfo.point;
        }
        else
        {
            //������ǥ�� 0���� �����. (ī�޶󸮱׷� �ű��.)
            transform.localPosition = Vector3.zero;
            //ī�޶���ġ������ ���⺤�� * ī�޶� �ִ�Ÿ� �� �ű��.
            transform.Translate(dir * camera_width);

        }
    }

}

