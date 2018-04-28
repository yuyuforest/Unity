using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory : MonoBehaviour {
    private static ArrowFactory instance;
    public static ArrowFactory Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ArrowFactory();
            }
            return instance;
        }
    }

    private int size = 10;
    private List<GameObject> idleArrowQueue;
    private List<GameObject> flyingArrowQueue;
    public static Vector3 recycleSite = new Vector3(-20, -20, -20);

    public int recycleCounter = 0;

    protected ArrowFactory()
    {
        if (instance == null) instance = this;
    }
    
    void Start () {
        idleArrowQueue = new List<GameObject>();
        flyingArrowQueue = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject gj;
            gj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Arrow"), recycleSite, Quaternion.identity);
            gj.GetComponent<Rigidbody>().isKinematic = true;
            idleArrowQueue.Add(gj);
            gj.name = i.ToString();
        }
	}
	
	void Update () {
		
	}

    //获取一个飞碟
    public GameObject GetArrow()
    {
        GameObject Arrow = idleArrowQueue[0];
        idleArrowQueue.Remove(Arrow);
        flyingArrowQueue.Add(Arrow);
        Arrow.SetActive(true);
        Arrow.transform.position = new Vector3(0, 0, -18);
        Debug.Log("send " + Arrow.name);
        return Arrow;
    }

    //回收飞碟
    public void RecycleArrow(GameObject Arrow)
    {
        //ArrowData dd = Arrow.GetComponent<ArrowData>();
        flyingArrowQueue.Remove(Arrow);
        idleArrowQueue.Add(Arrow);
        Arrow.transform.position = recycleSite;
        recycleCounter++;
        //Debug.Log("recycle " + Arrow.name);
        //Debug.Log("recycle " + recycleCounter + " Arrows");
    }
}
