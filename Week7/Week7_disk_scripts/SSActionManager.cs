using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();    //要执行的动作
    private List<SSAction> waitingAdd = new List<SSAction>();     //等待加入actions队列的动作
    private List<int> waitingDelete = new List<int>();  //存放从actions队列里移出的动作，等待删除

    protected void Start()
    {

    }

    protected void Update()
    {
        //将waitingAdd里的所有动作移入actions
        foreach (SSAction ac in waitingAdd)
            actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();

        //检查actions里的动作，如需要销毁则移入waitingDelete，否则调用其Update方法，执行该动作
        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                ac.callback.SSActionEvent(ac);      //通知ac的动作管理器ac已结束
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }

        //逐个回收动作对象，正确管理内存
        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }

    protected void FixedUpdate()
    {
        //将waitingAdd里的所有动作移入actions
        foreach (SSAction ac in waitingAdd)
            actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();

        //检查actions里的动作，如需要销毁则移入waitingDelete，否则调用其Update方法，执行该动作
        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                ac.callback.SSActionEvent(ac);      //通知ac的动作管理器ac已结束
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.FixedUpdate();
            }
        }

        //逐个回收动作对象，正确管理内存
        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }

    //参数：游戏对象gameobject，动作对象action，动作管理器manager
    //将action的游戏对象设为gameobject，管理器设为manager
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);     //将action加入等待执行的队列
        action.Start();             //开始action
    }
}
