using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//对SSActionManager根据实际用途的具体化，是一个针对特定场景的动作管理器
//因此需要实现ISSActionCallback接口，明确对动作对象开始或结束后做什么处理
public class CCActionManager : SSActionManager, ISSActionCallback {

    public CCMoveToAction moveStraight;
    public CCSequenceAction notStraight;
    public CCMoveToAction rise;
    public CCMoveToAction horizontal;
    public CCMoveToAction fall;

    public bool isMoving = false;

    //new是方法覆盖，该方法不会多态，且需要通过base.Start()调用原方法
    protected new void Start()
    {
        
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
        //检测是否正在执行某个动作
        if (events == SSActionEventType.Completed)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    public void moveBoatOrRole(GameObject gj, Vector3 target, float speed)
    {
        moveStraight = CCMoveToAction.GetSSAction(target, speed);
        this.RunAction(gj, moveStraight, this);
    }

    public void moveRoleBetweenBankAndBoat(GameObject gj, Vector3 target, float speed)
    {
        rise = CCMoveToAction.GetSSAction(new Vector3(gj.transform.position.x, 5, target.z), speed);
        horizontal = CCMoveToAction.GetSSAction(new Vector3(target.x, 5, target.z), speed);
        fall = CCMoveToAction.GetSSAction(target, speed);
        notStraight = CCSequenceAction.GetSSAcition(1, 0, new List<SSAction> {rise, horizontal, fall }, this);
        this.RunAction(gj, notStraight, this);
    }
}
