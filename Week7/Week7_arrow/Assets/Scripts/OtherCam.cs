using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCam : MonoBehaviour {
    public bool isShowCount = false;
    public float leftTime;

    void Update()
    {
        if (isShowCount)
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                this.gameObject.SetActive(false);
                isShowCount = false;
            }
        }
    }

    public void startToShow()
    {//副摄像机显示时间为1s
        this.gameObject.SetActive(true);
        isShowCount = true;
        leftTime = 1;
    }
}
