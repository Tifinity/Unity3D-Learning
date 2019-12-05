using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUserGUI : MonoBehaviour {
	IUserAction action;
	GUIStyle labelStyle;

	void Start () {
		action = Director.getDirector().currentSceneController as IUserAction;
	}
	
	void Update () {
		if(action.getResult() == 0) {
			if(Input.GetKey(KeyCode.W)) {
				action.moveForward();
			}
			else if(Input.GetKey(KeyCode.S)) {
				action.moveBack();
			}
			else {
				action.noMove();
			}

			if(Input.GetKeyDown(KeyCode.Space)) {
				action.shoot();
			}
			float deltaX = 0.5f;
			if(Input.GetKey(KeyCode.A)) {
				action.turn((-1)*deltaX);
			}
			else if(Input.GetKey(KeyCode.D)) {
				action.turn(deltaX);
			}
			else {
				action.noTurn();
				
			}
		}
	}

	void OnGUI() {
		labelStyle = new GUIStyle("label");
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontSize = Screen.height/15;
		GUI.color = Color.black;
		if(action.getResult() == 1) {
			Debug.Log("YOU WIN!");
			GUI.Label(new Rect(Screen.width/2 - Screen.width/8,Screen.height/2 - Screen.height/8,Screen.width/4,Screen.height/4), "YOU WIN!",labelStyle);
		}
		else if(action.getResult() == -1) {
			Debug.Log("Game Over!");
			GUI.Label(new Rect(Screen.width/2 - Screen.width/8,Screen.height/2 - Screen.height/8,Screen.width/4,Screen.height/4), "Game Over!",labelStyle);
		}
	}
}
