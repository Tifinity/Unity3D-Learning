using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction action;
    GUIStyle score_style = new GUIStyle();
    GUIStyle text_style = new GUIStyle();
    private bool game_start = false;

    void Start () {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
        text_style.normal.textColor = new Color(0, 0, 0, 1);
        text_style.fontSize = 16;
        score_style.normal.textColor = new Color(1, 0, 1, 1);
        score_style.fontSize = 16;
    }

    void Update() {}
    private void OnGUI() {
        if(game_start) {
            GUI.Label(new Rect(10, 5, 200, 50), "分数:", text_style);
            GUI.Label(new Rect(55, 5, 200, 50), action.GetScore().ToString(), score_style);
            GUI.Label(new Rect(Screen.width - 170, 30, 200, 50), "风向: ", text_style);
            GUI.Label(new Rect(Screen.width - 110, 30, 200, 50), action.GetWind(), text_style);
        }
        else {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.width / 2 - 320, 100, 100), "简单打靶", text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 150, 100, 50), "游戏开始")) {
                game_start = true;
                action.BeginGame();
            }
        }
    }
}
