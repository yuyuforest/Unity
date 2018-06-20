using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct State
{
    public int beginningPriests;
    public int beginningDevils;
    public bool boatBeginning;
    public State(int p = 3, int d = 3, bool b = true)
    {
        beginningPriests = p;
        beginningDevils = d;
        boatBeginning = b;
    }
    public int endingPriests
    {
        get
        {
            return 3 - beginningPriests;
        }
    }
    public int endingDevils
    {
        get
        {
            return 3 - beginningDevils;
        }
    }
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is State)) return false;
        State s2 = (State)obj;
        return beginningPriests == s2.beginningPriests && beginningDevils == s2.beginningDevils 
            && boatBeginning == s2.boatBeginning;
    }
}

public class AIGameController : MonoBehaviour {

    public State current = new State();
    public State last = new State();
    public State[] states = new State[16];
    int[,] diagraph = new int[16, 16];

	void Start () {
        states[0] = new State(3, 3, true);  //3P3DB
        states[1] = new State(3, 2, false); //3P2D
        diagraph[1, 0] = 1;
        states[2] = new State(3, 2, true);  //3P2DB
        states[3] = new State(3, 1, false); //3P1D
        diagraph[0, 3] = 1;
        diagraph[3, 2] = 1;
        states[4] = new State(3, 1, true);  //3P1DB
        states[5] = new State(3, 0, false); //3P0D
        diagraph[2, 5] = 1;
        diagraph[5, 4] = 1;
        states[6] = new State(2, 2, false); //2P2D
        diagraph[0, 6] = 1;
        diagraph[6, 2]= 1;
        states[7] = new State(2, 2, true);  //2P2DB
        states[8] = new State(1, 1, false); //1P1D
        diagraph[4, 8] = 1;
        diagraph[8, 7] = 1;
        states[9] = new State(1, 1, true);  //1P1DB
        states[10] = new State(0, 3, true); //0P3DB
        states[11] = new State(0, 2, false);    //0P2D
        diagraph[7, 11] = 1;
        diagraph[11, 10] = 1;
        states[12] = new State(0, 2, true);     //0P2DB
        states[13] = new State(0, 1, false);    //0P1D
        diagraph[13, 9] = 1;
        diagraph[10, 13] = 1;
        diagraph[13, 12] = 1;
        states[14] = new State(0, 0, false);    //0P0D  //win
        diagraph[9, 14] = 1;
        diagraph[12, 14] = 1;
        states[15] = new State(-1, -1, false);  //lose

        for(int i = 0; i < 16; i++)
        {
            for(int j = 0; j < 16; j++)
            {
                if (diagraph[i, j] == 0) diagraph[i, j] = -1;
            }
        }

        current = new State(3, 3, true);
    }
	
	void Update () {
		
	}

    public State Next()
    {
        int now = -1;
        int prev = -1;
        for(int i = 0; i < states.Length; i++)
        {
            if (states[i].Equals(current)) now = i;
            if (states[i].Equals(last)) prev = i;
        }
        Debug.Log("now " + now + " prev " + prev);
        List<int> list = new List<int>();
        for(int i = 0; i < states.Length; i++)
        {
            if (diagraph[now, i] == 1 && i != prev)
            {
                list.Add(i);
                Debug.Log("list " + i);
            }
        }
        int next = Random.Range(0, list.Count);
        return states[list[next]];
    }


    public void change(int priests, int devils)
    {
        last = current;
        current.beginningPriests += priests;
        current.beginningDevils += devils;
        current.boatBeginning = !current.boatBeginning;
    }
}
