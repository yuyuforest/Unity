using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour {

    public Transform player = null;
    public Transform patrol = null;

    //是否遇上当前巡逻区域的边界（墙），这个标记用来通知PatrolAction调整方向
    public bool borderIsFront = false;

    //是否与玩家近距离接触
    public bool playerIsFront = false;
    
    //是否正在追捕
    public bool isHunting
    {
        get
        {
            return player != null;
        }
    }

    //巡逻区域的坐标范围
    private float xmin, xmax, zmin, zmax;
    
	void Start () {
        patrol = gameObject.transform;
        xmin = (int)(patrol.position.x / 5) * 5 + 0.3f;
        xmax = (int)(patrol.position.x / 5 + 1) * 5 - 0.3f;
        zmin = (int)(patrol.position.z / 5) * 5 + 0.3f;
        zmax = (int)(patrol.position.z / 5 + 1) * 5 - 0.3f;
    }
	
	void Update () {
        if(!isInZone(patrol))   //若巡逻兵走出了范围，则标记borderIsFront为true，将会通知PatrolAction来调整方向
        {
            borderIsFront = true;
        }
        if (playerIsFront)      //若巡逻兵近距离接触玩家，则启用捕获玩家的相关事件
        {
            PatrolEventManager.Hunt();
        }
		if(isHunting && !isInZone(player))  //检测追击过程中玩家离开当前区域的情况
        {
            player = null;
            gameObject.GetComponent<Animator>().SetBool("ToAttack", false);
            PatrolEventManager.Flee();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        //玩家在当前区域，则追击
        if (isInZone(other.transform) && other.tag.Equals("Player"))
        {
            player = other.transform;
            gameObject.GetComponent<Animator>().SetBool("ToAttack", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //玩家在当前区域但已离开追捕范围，则追捕失败，玩家得分
        if (isInZone(other.transform) && other.tag.Equals("Player"))
        {
            player = null;
            gameObject.GetComponent<Animator>().SetBool("ToAttack", false);
            PatrolEventManager.Flee();
        }
    }

    //用于检查某些角色是否在当前巡逻区域
    private bool isInZone(Transform role)
    {
        return zmin <= role.position.z && role.position.z <= zmax && xmin <= role.position.x && role.position.x <= xmax;
    }
}
