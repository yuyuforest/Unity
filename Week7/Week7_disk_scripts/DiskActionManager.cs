using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskActionManager : SSActionManager, ISSActionCallback, IActionManager {

    public DiskFactory diskFactory;

    //new是方法覆盖，该方法不会多态，且需要通过base.Start()调用原方法
    protected new void Start()
    {
        diskFactory = DiskFactory.Instance;
    }

    //关于new，同上
    protected new void Update()
    {
        base.Update();
    }

    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        diskFactory.RecycleDisk(source.gameobject);
    }

    public void sendDisk(GameObject disk, IActionParameter da)
    {
        this.RunAction(disk, (DiskAction)da, this);
    }
}
