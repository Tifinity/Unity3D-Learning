using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneController : MonoBehaviour, IUserAction, ISceneController { 
    public Camera child_camera;
    public ScoreRecorder recorder;
    public ArrowFactory arrow_factory;
    public ArrowFlyActionManager action_manager;

    public GameObject bow;
    private GameObject arrow;                   
    private GameObject target;
    private int round = 0;

    private bool game_over = false;
    private bool game_start = false; 
    private string wind_name = "";  
    private Vector3 wind = new Vector3(0, 0, 0);
    private Vector3 force;

    void Start () {
        SSDirector director = SSDirector.GetInstance();
        arrow_factory = Singleton<ArrowFactory>.Instance;
        recorder = Singleton<ScoreRecorder>.Instance;
        director.CurrentScenceController = this;
        action_manager = gameObject.AddComponent<ArrowFlyActionManager>() as ArrowFlyActionManager;
        LoadResources();
        CreateWind();
    }

    void Update () {
        if(game_start) {
            Vector3 mpos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
            if (Input.GetButtonDown("Fire1")) {
                Shoot(mpos * 15 );
            }
            if (arrow == null) {
                arrow = arrow_factory.GetArrow();
                arrow.transform.position = bow.transform.position;
                arrow.gameObject.SetActive(true);
                arrow.GetComponent<Rigidbody>().isKinematic = true;
            }
            bow.transform.LookAt(mpos * 30);
            arrow.transform.LookAt(mpos * 30);
            arrow_factory.FreeArrow();
        }
    }
    public void LoadResources() {
        bow = Instantiate(Resources.Load("Prefabs/bow", typeof(GameObject))) as GameObject;
        target = Instantiate(Resources.Load("Prefabs/target", typeof(GameObject))) as GameObject;
    }
    
    public void Shoot(Vector3 force) {
        if (arrow != null) {
            arrow.GetComponent<Rigidbody>().isKinematic = false;
            action_manager.ArrowFly(arrow, wind, force);
            child_camera.GetComponent<ChildCamera>().StartShow();
            arrow = null;
            CreateWind();
            round++;
        }
    }

    public int GetScore() {
        return recorder.score;
    }

    public bool GetGameover() {
        return game_over;
    }

    public string GetWind() {
        return wind_name;
    }

    public void CreateWind() {
        float wind_directX = ((Random.Range(-10, 10) > 0) ? 1 : -1) * round;
        float wind_directY = ((Random.Range(-10, 10) > 0) ? 1 : -1) * round;
        Debug.Log(wind_directX);
        wind = new Vector3(wind_directX, wind_directY, 0);

        string Horizontal = "", Vertical = "", level = "";
        if (wind_directX > 0) {
            Horizontal = "西";
        } else if (wind_directX <= 0) {
            Horizontal = "东";
        }
        if (wind_directY > 0) {
            Vertical = "南";
        } else if (wind_directY <= 0) {
            Vertical = "北";
        }
        level = round.ToString();
        wind_name = Horizontal + Vertical + "风" + " " + level;
    }

    public void BeginGame() {
        Cursor.visible = false;
        game_start = true;
    }
}
