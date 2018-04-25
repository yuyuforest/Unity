using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
    private static DiskFactory instance;
    public static DiskFactory Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new DiskFactory();
            }
            return instance;
        }
    }

    private int size = 10;
    private List<GameObject> idleDiskQueue;
    private List<GameObject> flyingDiskQueue;
    public static Vector3 recycleSite = new Vector3(-20, -20, -20);

    public int recycleCounter = 0;

    protected DiskFactory()
    {
        if (instance == null) instance = this;
    }
    
    void Start () {
        idleDiskQueue = new List<GameObject>();
        flyingDiskQueue = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject gj;
            gj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Disk"), recycleSite, Quaternion.identity);
            
            idleDiskQueue.Add(gj);
            gj.name = i.ToString();
        }
	}
	
	void Update () {
		
	}

    //获取一个飞碟
    public GameObject GetDisk()
    {
        GameObject disk = idleDiskQueue[0];
        idleDiskQueue.Remove(disk);
        flyingDiskQueue.Add(disk);
        disk.SetActive(true);
        Debug.Log("send " + disk.name);
        return disk;
    }

    //回收飞碟
    public void RecycleDisk(GameObject disk)
    {
        DiskData dd = disk.GetComponent<DiskData>();
        if (dd.isBonus()) dd.setDefault();
        if (disk == null) Debug.Log("null object");
        if (flyingDiskQueue == null) Debug.Log("null queue");
        flyingDiskQueue.Remove(disk);
        idleDiskQueue.Add(disk);
        disk.transform.position = recycleSite;
        recycleCounter++;
    }
}
