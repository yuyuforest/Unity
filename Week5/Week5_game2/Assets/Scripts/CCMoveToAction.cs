using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承了SSAction动作基类，定义一个更具体的动作类型
public class CCMoveToAction : SSAction {

    public Vector3 target;        //目的地
    public float speed;           //速度

    //防止开发者自己new一个动作
    protected CCMoveToAction()
    {

    }

    public override void Start()
    {
        //通知动作管理者，动作即将开始
        this.callback.SSActionEvent(this, SSActionEventType.Started);
    }

    //该动作具体的运动实现
    public override void Update()
    {
        //运动代码
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);

        //到达目的地
        if (this.transform.position == target)
        {
            this.destroy = true;                    //动作要被销毁
            this.callback.SSActionEvent(this);      //通知动作管理者该动作结束
        }
        else this.callback.SSActionEvent(this, SSActionEventType.Started);
    }


    //开发者只能使用该函数来新建一个动作
    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();  //让unity自己创建动作类，确保内存正确回收
        action.target = target;
        action.speed = speed;
        return action;
    }
}
