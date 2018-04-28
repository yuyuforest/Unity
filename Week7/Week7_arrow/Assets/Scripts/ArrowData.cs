using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowData : MonoBehaviour {

    //检测是否已飞出摄像机视野外
    public void Update()
    {
        /*
        Camera ca = Camera.main;
        Vector3 pos = ca.WorldToViewportPoint(gameObject.transform.position);
        bool isVisible = (ca.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
        if (!isVisible)
        {
            gameObject.SetActive(false);
            //Debug.Log("bye");
        }
        */
    }
}
