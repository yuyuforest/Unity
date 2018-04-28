using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

    public RoundController roundController;
    public ScoreRecorder recorder;

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name + " enter " + gameObject.name);
        Transform parent = other.transform.parent;
        if (parent == null || parent.tag != "Arrow") return;
        parent.tag = "Shot";
        Rigidbody rigid = parent.GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = true;
        recorder.AddScore(int.Parse(gameObject.name));
        Debug.Log(other.gameObject.name + " get " + gameObject.name);
    }
}
