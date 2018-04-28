using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { Started, Completed }

//动作与动作管理者之间的接口，可以被动作（SSAction及其子类）包含
//可以被动作管理者实现，实现事件调度(通知动作已完成，或动作要开始了，同时可传递某些参数)
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null);
}
