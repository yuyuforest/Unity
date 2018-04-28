using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalArrowAction : SSAction{
    public Vector3 velocity = new Vector3(0, 3.0f, 17.0f);                        //箭初速度
    public Vector3 wind = new Vector3(-2, 2, 0);

    protected PhysicalArrowAction()
    {

    }

    public override void Start()
    {
        var rigidBody = gameobject.GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.AddForce(gameobject.transform.forward * 30.0f, ForceMode.VelocityChange);
    }

    public override void Update () {
		
	}

    public override void FixedUpdate()
    {
        if (destroy) return;
        else if (!gameobject.activeSelf)
        {
            destroy = true;
        }
        else
        {
        }
    }

    public static PhysicalArrowAction GetPhysicalArrowAction()
    {
        PhysicalArrowAction action = ScriptableObject.CreateInstance<PhysicalArrowAction>();
        return action;
    }
}
