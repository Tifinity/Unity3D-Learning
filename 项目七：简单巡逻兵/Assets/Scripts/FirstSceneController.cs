using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneController : MonoBehaviour, IUserAction, ISceneController {
    public GuardFactory guard_factory;                               //巡逻者工厂
    public ScoreRecorder recorder;                                   //记录员
    public GuardActionManager action_manager;                        //运动管理器
    public int playerSign = -1;                                      //当前玩家所处哪个格子
    public GameObject player;                                        //玩家
    public UserGUI gui;                                              //交互界面

    private List<GameObject> guards;                                 //场景中巡逻者列表
    private bool game_over = false;                                  //游戏结束

    
    void Awake() {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        guard_factory = Singleton<GuardFactory>.Instance;
        action_manager = gameObject.AddComponent<GuardActionManager>() as GuardActionManager;
        gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        LoadResources();
        recorder = Singleton<ScoreRecorder>.Instance;
    }

    void Update() {
        for (int i = 0; i < guards.Count; i++) {
            guards[i].gameObject.GetComponent<GuardData>().playerSign = playerSign;
        }
    }


    public void LoadResources() {
        Instantiate(Resources.Load<GameObject>("Prefabs/Plane"));
        player = Instantiate(
            Resources.Load("Prefabs/Player"), 
            new Vector3(13, 8, -13), Quaternion.identity) as GameObject;
        guards = guard_factory.GetPatrols();

        for (int i = 0; i < guards.Count; i++) {
            action_manager.GuardPatrol(guards[i], player);
        }
    }

    public int GetScore() {
        return recorder.GetScore();
    }

    public bool GetGameover() {
        return game_over;
    }

    public void Restart() {
        SceneManager.LoadScene("Scenes/mySence");
    }

    void OnEnable() {
        GameEventManager.ScoreChange += AddScore;
        GameEventManager.GameoverChange += Gameover;
    }
    void OnDisable() {
        GameEventManager.ScoreChange -= AddScore;
        GameEventManager.GameoverChange -= Gameover;
    }

    void AddScore() {
        recorder.AddScore();
    }

    void Gameover() {
        game_over = true;
    }
}
