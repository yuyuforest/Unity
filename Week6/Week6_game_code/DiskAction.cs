using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskAction : SSAction {

    public float time = 0;
    public float xSpeed;
    public float zSpeed;
    public float yUpSpeed;
    public float yDownSpeed;
    public float speed;
    public Vector3 startingPoint = Vector3.zero;    //设置飞碟发射起点
    public Vector3 rotation = Vector3.zero;

    protected DiskAction() { }

    public override void Start () {
        gameobject.transform.position = startingPoint;
        gameobject.transform.Rotate(rotation);
        time = 0;
	}

	public override void Update ()
    {
        if (destroy) return;
        else if (!gameobject.activeSelf)
        {
            destroy = true; //一定要及时置destroy，否则会累积多个有相同游戏对象的action，发生冲突
        }
        else
        {
            time += Time.deltaTime;
            Vector3 pos = gameobject.transform.position;
            float newx = pos.x + Time.deltaTime * xSpeed;
            float newy = yUpSpeed * time - yDownSpeed * time * time;
            float newz = pos.z + Time.deltaTime * zSpeed;
            Vector3 target = new Vector3(newx, newy, newz);
            gameobject.transform.position = target;
        }
	}

    public static DiskAction GetDiskAction()
    {
        DiskAction action = ScriptableObject.CreateInstance<DiskAction>();

        action.xSpeed = 10.0f;
        action.zSpeed = 15.0f;
        action.yDownSpeed = 4.0f;
        action.yUpSpeed = 6.0f;
        return action;
    }
}
