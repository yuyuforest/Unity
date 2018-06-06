using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Query : MonoBehaviour
{

    private Button Title;   //公告题目
    public Text Content;    //公告内容
    private int frame = 20; //帧数

    // Use this for initialization  
    void Start()
    {
        Button Title = this.gameObject.GetComponent<Button>();
        Title.onClick.AddListener(TaskOnClick);
    }

    //打开公告内容信息
    IEnumerator Open()
    {
        float rx = -90;                             //文本旋转角度
        int height = Content.text.Length / 13 + 30; //规定文本框的高度
        height = (height / frame + 1) * frame;      //保证文本框高度可以整除帧数
        float y = 0;
        for (int i = 0; i < frame; i++)
        {
            rx += 90f / frame;
            y += height / frame;
            Content.transform.rotation = Quaternion.Euler(rx, 0, 0);    //随帧旋转
            Content.rectTransform.sizeDelta = new Vector2(Content.rectTransform.sizeDelta.x, y);    //随帧展开文本框
            if (i == 0)
            {
                Content.gameObject.SetActive(true);
            }
            yield return null;
        }
    }

    //折叠公告内容信息
    IEnumerator Close()
    {
        float rx = 0;
        int height = Content.text.Length / 13 + 30;
        height = (height / frame + 1) * frame;
        float y = height;
        for (int i = 0; i < frame; i++)
        {
            rx -= 90f / frame;
            y -= height / frame;
            Content.transform.rotation = Quaternion.Euler(rx, 0, 0);
            Content.rectTransform.sizeDelta = new Vector2(Content.rectTransform.sizeDelta.x, y);
            if (i == frame - 1)
            {
                Content.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
    
    void TaskOnClick()
    {
        if (Content.gameObject.activeSelf)
        {
            StartCoroutine(Close());
        }
        else
        {
            StartCoroutine(Open());
        }

    }
}