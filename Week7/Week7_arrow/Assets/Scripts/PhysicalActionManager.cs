using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalActionManager : SSActionManager, ISSActionCallback, IActionManager
{
    public ArrowFactory ArrowFactory;

    //new是方法覆盖，该方法不会多态，且需要通过base.Start()调用原方法
    protected new void Start()
    {
        ArrowFactory = ArrowFactory.Instance;
    }

    //关于new，同上
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        ArrowFactory.RecycleArrow(source.gameobject);
    }

    public void sendArrow(GameObject Arrow, IActionParameter da)
    {
        this.RunAction(Arrow, (PhysicalArrowAction)da, this);
    }
}
