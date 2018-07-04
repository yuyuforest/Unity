using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEventManager : MonoBehaviour {
    public static PatrolEventManager instance;
    public delegate void HuntAction();              //巡逻兵抓获成功
    public static event HuntAction OnHuntAction;
    public delegate void FleeAction();              //玩家逃跑成功
    public static event FleeAction OnFleeAction;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void flee()
    {
        if (OnFleeAction != null)
        {
            OnFleeAction();
        }
    }

    public void hunt()
    {
        if (OnHuntAction != null)
        {
            OnHuntAction();
        }
    }

    public static void Hunt()
    {
        if (instance != null) instance.hunt();
    }

    public static void Flee()
    {
        if (instance != null) instance.flee();
    }
}
