using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//动作基类
public class SSAction : ScriptableObject
{

    public bool enable = true;          //是否可以进行该动作
    public bool destroy = false;        //是否要销毁动作，SSActionManager及其子类会检查这个参数，从而决定是否update
    public GameObject gameobject;       //该动作的对象
    public Transform transform;         //动作对象的transform属性
    public ISSActionCallback callback;  //相当于该动作的管理者（ActionManager或者SequenceAction）
    //SSAction类对象可通过调用callback.SSActionEvent(...)来告诉管理者该动作的状态，让管理者进行相关处理
    //比如CCMoveToAction，在Update里，到达目的地后调用callback.SSActionEvent()，告诉管理者动作结束

    //防止开发者自己new一个动作
    protected SSAction()
    {

    }

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }

    public virtual void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }
}