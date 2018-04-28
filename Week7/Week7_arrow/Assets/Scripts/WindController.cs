using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour {
    private Vector3 wind;
    private string windDescription = "风: 强度0";
    public Vector3 Wind
    {
        get
        {
            return wind;
        }
    }

	void Start () {
		
	}

    public void resetWind()
    {

        wind = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
        setWindDescription();
    }

    public string getWindDescription()
    {
        return windDescription;
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name + " enter wind");
        ///*
        Transform arrow = other.transform.parent;
        if (arrow.tag != "Arrow") return;

        arrow.GetComponent<ConstantForce>().force = wind;
        //*/
    }

    private void OnTriggerExit(Collider other)
    {
        ///*
        Transform arrow = other.transform.parent;
        if (arrow.tag != "Arrow") return;
        arrow.GetComponent<ConstantForce>().force = Vector3.zero;
        //*/
    }

    private void setWindDescription()
    {
        string first = wind.x > 0 ? "西" : "东";
        string mid = "偏";
        string second = wind.z > 0 ? "南" : "北";
        float angle = 0;
        string direction = "";
        string strength = "强度";
        
        if (wind.x == 0)
        {
            angle = 90;
        }
        else
        {
            angle = Mathf.Atan(Mathf.Abs(wind.z) / Mathf.Abs(wind.x)) * 180 / Mathf.PI;
        }
        if (angle == 90) direction += second;
        else if (angle == 0 && wind.x != 0) direction += first;
        else if (wind.x != 0 && wind.z != 0) direction = first + mid + second;
        if (angle != 0 && angle != 90) direction = direction + (int)angle + "°";
        if (direction != "") direction += " ";

        strength += (int)wind.magnitude;

        windDescription = "风：" + direction + strength;
    }
}
