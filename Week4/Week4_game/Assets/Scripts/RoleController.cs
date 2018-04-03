using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleController : MonoBehaviour {

	private bool living;

	// Use this for initialization
	void Start () {
		living = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!living) {
			var c = this.GetComponent<Renderer> ().material.color;
			if(c.a > 0) this.GetComponent<Renderer> ().material.color = new Color(c.r, c.g, c.b, c.a - Time.deltaTime * 50);
		}
	}

	public void beKilled () {
		living = false;
	}
}
