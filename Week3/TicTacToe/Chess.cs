using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class Chess : MonoBehaviour {
	public int counter;
	public int player;
	private int[,] grid = new int[3, 3];

	void Start () {  
		restart();  
	}

	void restart()  
	{  
		player = 1;  
		for (int i = 0; i < 3; ++i)  
		{  
			for (int j = 0; j < 3; ++j)  
			{  
				grid[i, j] = 0;  
			}  
		}  
		counter = 0;  
	} 

	private int winCheck()  
	{  
		for(int i = 0; i < 3; ++i)  
		{  
			if (grid[i, 0] != 0 && grid[i,0]==grid[i, 1] && grid[i, 1] == grid[i, 2])  
			{  
				return grid[i, 0];  
			}  
		}  
		for (int i = 0; i < 3; ++i)  
		{  
			if (grid[0, i] != 0 && grid[0, i] == grid[1, i] && grid[1, i] == grid[2, i])  
			{  
				return grid[0, i];  
			}  
		}  
		if(grid[1,1]!=0&&  
			grid[0,0]==grid[1,1]&&  
			grid[1,1]==grid[2,2]||  
			grid[0,2]==grid[1,1]&&  
			grid[1,1]==grid[2,0]  
		)  
		{  
			return grid[1, 1];  
		}  
		if (counter == 9) return 3;  
		return 0;  
	}  

	private void OnGUI()  
	{  
		if(GUI.Button(new Rect(20, 300, 100, 50),"restart"))  
		{  
			restart();  
		}  

		int result = winCheck(); 

		GUIStyle ui = new GUIStyle  
		{  
			fontSize = 20  
		};  
		ui.normal.textColor = Color.red;  
		ui.fontStyle = FontStyle.Bold;  

		switch (result)  
		{  
			case 1:  
				GUI.Label(new Rect(500, 100, 100, 50), "O Win!", style: ui);//先手 
				break;  
			case 2:  
				GUI.Label(new Rect(500, 100, 100, 50), "X Win!", style: ui);//后手
				break;  
			case 3:  
				GUI.Label(new Rect(500, 100, 100, 50), "Dual!", style: ui);//平局  
				break;  
		}  

		for (int i = 0; i < 3; ++i)  
		{  
			for(int j = 0; j < 3; ++j)  
			{  
				if (grid[i, j] == 1)  
				{  
					GUI.Button(new Rect(i * 50, j * 50, 50, 50), "O");  
				}  
				if (grid[i, j] == 2)  
				{  
					GUI.Button(new Rect(i * 50, j * 50, 50, 50), "X");  
				}  
				if(GUI.Button(new Rect(i * 50, j * 50, 50, 50), ""))  
				{  
					if (result == 0)  
					{  
						if (player == 1) grid[i, j] = 1;  
						else grid[i, j] = 2;  
						counter++;  
						player = -player;  
					}  
				}  
			}  
		}  
	}  


	void Update () {  

	}  
}
