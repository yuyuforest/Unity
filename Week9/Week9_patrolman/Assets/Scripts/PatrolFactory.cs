using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFactory : System.Object {

    private static PatrolFactory instance;
    private List<GameObject> patrolList;


    protected PatrolFactory()
    {
        patrolList = new List<GameObject>();
    }

    public static PatrolFactory getInstance()
    {
        if(instance == null) instance = new PatrolFactory();
        return instance;
    }

	void Start () {
	}
	
	void Update () {
		
	}

    public GameObject GetPatrol(Vector3 pos, Quaternion qua)
    {
        GameObject patrol = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Patrol"), pos, qua);
        patrolList.Add(patrol);
        return patrol;
    }
}
