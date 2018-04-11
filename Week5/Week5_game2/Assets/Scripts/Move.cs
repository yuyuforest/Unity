using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
//using UnityEditorInternal;

public class Move : MonoBehaviour {

	enum State{still, up, horizontal, down};

	private State state;

	private bool isDirect;
	private Vector3 beginning, ending;

	// Use this for initialization
	void Start () {
		if (this.name == "boat")
			this.isDirect = true;
		else if (this.name == "priest" || this.name == "devil")
			this.isDirect = false;
		state = State.still;
		beginning = this.transform.position;
		ending = beginning;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.up) {
			Vector3 beginningup = new Vector3 (beginning.x, 5, 0);
			this.transform.position = Vector3.MoveTowards (this.transform.position, beginningup, 0.1f);
			if (this.transform.position == beginningup)
				state = State.horizontal;
		}
		else if (state == State.horizontal) {
			Vector3 endingup = new Vector3 (ending.x, transform.position.y, 0);
			this.transform.position = Vector3.MoveTowards (this.transform.position, endingup, 0.1f);
			if (this.transform.position == endingup) {
				if (isDirect)
					state = State.still;
				else
					state = State.down;
			}
		}
		else if (state == State.down) {
			this.transform.position = Vector3.MoveTowards (this.transform.position, ending, 0.1f);
			if (this.transform.position == ending)
				state = State.still;
		}
	}

	public void setDirect(bool d){
		isDirect = d;
	}

	public void goTo(Vector3 end) {
		beginning = this.transform.position;
		ending = end;
		if (this.transform.position.y != end.y)
			isDirect = false;
		else
			isDirect = true;
		if (isDirect)
			state = State.horizontal;
		else
			state = State.up;
	}

	public bool isMoving(){
		return state != State.still;
	}
}
