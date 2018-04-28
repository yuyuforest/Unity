using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class co : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject.name + " collide " + collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + " trigger " + other.gameObject.name);
    }
}
