using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskData : MonoBehaviour {
    public GameObject sphere, cube1, cube2;
    public Color sphereColor, cubeColor;
    public int score = 1;
    //public bool isFlying = false;
    public void setBonus()  //设置特殊飞碟
    {
        sphereColor = Color.blue;
        cubeColor = Color.red;
        sphere.GetComponent<MeshRenderer>().material.color = sphereColor;
        cube1.GetComponent<MeshRenderer>().material.color = cubeColor;
        cube2.GetComponent<MeshRenderer>().material.color = cubeColor;
        score = 3;
    }
    public void setDefault()    //重置为默认飞碟
    {
        sphereColor = Color.white;
        cubeColor = Color.black;
        sphere.GetComponent<MeshRenderer>().material.color = sphereColor;
        cube1.GetComponent<MeshRenderer>().material.color = cubeColor;
        cube2.GetComponent<MeshRenderer>().material.color = cubeColor;
        score = 1;
    }
    public bool isBonus()  
    {
        return score > 1;
    }

    //检测是否已飞出摄像机视野外
    public void Update()
    {
        Camera ca = Camera.main;
        Vector3 pos = ca.WorldToViewportPoint(gameObject.transform.position);
        bool isVisible = (ca.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
        if (!isVisible)
        {
            gameObject.SetActive(false);
            //Debug.Log("bye");
        }
    }
}
