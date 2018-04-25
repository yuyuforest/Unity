using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDiskAction : SSAction, IActionParameter{
    public Vector3 velocity;                        //飞碟初速度
    public Vector3 acceleration;                           
    public Vector3 startingPoint = Vector3.zero;    //设置飞碟发射起点
    public Vector3 rotation = Vector3.zero;

    protected PhysicalDiskAction()
    {

    }

    public override void Start()
    {
        gameobject.transform.position = startingPoint;
        gameobject.transform.Rotate(rotation);
        var rigidBody = gameobject.GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.useGravity = false;
        rigidBody.velocity = velocity;
        var force = gameobject.GetComponent<ConstantForce>();
        force.relativeForce = acceleration;
        Debug.Log(startingPoint + " " + velocity + " " + acceleration + " ");
    }

    public override void Update () {
		
	}

    public override void FixedUpdate()
    {
        if (destroy) return;
        else if (!gameobject.activeSelf)
        {
            destroy = true; //一定要及时置destroy，否则会累积多个有相同游戏对象的action，发生冲突
        }
        else
        {
            Debug.Log(gameobject.name + " " + gameobject.GetComponent<Rigidbody>().velocity);
        }
    }

    public void setParameter(Vector3 start, Vector3 velocity, float acceleration)
    {
        this.velocity = velocity;
        this.startingPoint = start;
        this.acceleration = new Vector3(0, - acceleration, 0);
    }

    public static PhysicalDiskAction GetPhysicalDiskAction()
    {
        PhysicalDiskAction action = ScriptableObject.CreateInstance<PhysicalDiskAction>();
        return action;
    }
}
