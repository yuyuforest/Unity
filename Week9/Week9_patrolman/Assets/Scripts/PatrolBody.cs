using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBody : MonoBehaviour {

    PatrolController patrolController;
    
	void Start () {
        patrolController = gameObject.transform.parent.GetComponent<PatrolController>();

    }
	
	void Update () {
		
	}

    //检测近距离遇上其他物体的情况
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("wall"))
        {
            patrolController.borderIsFront = true;
        }
        else if (other.tag.Equals("Player"))
        {
            gameObject.transform.parent.GetComponent<PatrolController>().playerIsFront = true;
        }
    }

    //离开其他物体
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("wall"))
        {
            gameObject.transform.parent.GetComponent<PatrolController>().borderIsFront = false;
        }
        else if (other.tag.Equals("Player"))
        {
            gameObject.transform.parent.GetComponent<PatrolController>().playerIsFront = false;
        }
    }
}
