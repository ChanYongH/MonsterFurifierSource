using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddScroll : MonoBehaviour
{
    ScrollRect scrollRect;
    public float width;
    public float height;
    public RectTransform contentsRect; // �ν����Ϳ��� ���� ��
    public float itemCount = 0;
    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        itemCount = 3;
    }
    public void SetContentSize()
    {
        if ((float)contentsRect.transform.childCount / 6 > 3) // 3���� �ʰ��ϸ�
        {
            if ((float)contentsRect.transform.childCount / 6 > itemCount) // ���� �Ѿ�� 
            {
                itemCount++; // �� ���� �߰� �ȴ�
                height += 200; //
            }
        }
        scrollRect.content.sizeDelta = new Vector2(width, height);
        contentsRect.sizeDelta = new Vector3(100, height - 900f, 0);
    }
}