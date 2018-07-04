using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//组合动作，用于执行一组动作
//可以作为动作类型，也可以视作一个动作管理器（实现了ISSActionCallback接口）
public class CCSequenceAction : SSAction, ISSActionCallback {

    public List<SSAction> sequence;
    public int repeat = -1; //表示重复的次数，如果为-1，则无限循环执行动作
    public int start = 0;   //作为索引来获取sequence中需要执行的动作，这个类的update方法会调用sequence[start].Update()

    //开发者通过该函数来新建一个CCSequenceAction
    public static CCSequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence, ISSActionCallback callback = null)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        action.callback = callback;
        return action;
    }

    //执行start指示的队列中的动作
    public override void Update()
    {
        if (sequence.Count == 0 || repeat == 0)
        {
            return;
        }
        if (start < sequence.Count)
        {
            sequence[start].Update();
            this.callback.SSActionEvent(this, SSActionEventType.Started);
        }
    }

    //动作类的对象source调用callback.SSActionEvent()，CCSequenceAction要根据source的状态来决定进行什么处理
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        if (events == SSActionEventType.Started) return;
        source.destroy = false; //可能还要进行循环，暂时不销毁（但该动作可能在通知管理器前就将destroy置true了，比如CCMoveToAction,所以要重置false）
        this.start++;           //events默认为动作结束，所以start往后推一位，选定下一个动作
        if (this.start >= sequence.Count)
        {
            if (repeat > 0) repeat--;
            this.start = 0;     //start归零，repeat减1，表示又执行了一次循环
            if (repeat == 0)    //重复次数已用完
            {
                this.destroy = true;                //该组合动作需要销毁
            }
        }
    }

    public override void Start()
    {
        this.callback.SSActionEvent(this, SSActionEventType.Started);
        foreach (SSAction action in sequence)
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    void OnDestroy()
    {
        
    }
}
