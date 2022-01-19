using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddScroll : MonoBehaviour
{
    ScrollRect scrollRect;
    public float width;
    public float height;
    public RectTransform contentsRect; // 인스펙터에서 끌어 옴
    public float itemCount = 0;
    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        itemCount = 3;
    }
    public void SetContentSize()
    {
        if ((float)contentsRect.transform.childCount / 6 > 3) // 3줄을 초과하면
        {
            if ((float)contentsRect.transform.childCount / 6 > itemCount) // 줄을 넘어가면 
            {
                itemCount++; // 한 줄이 추가 된다
                height += 200; //
            }
        }
        scrollRect.content.sizeDelta = new Vector2(width, height);
        contentsRect.sizeDelta = new Vector3(100, height - 900f, 0);
    }
}