using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterfaceApplication;

/*动作基类*/
public class SSAction : ScriptableObject {
    public bool enable = true;                      //是否进行
    public bool destroy = false;                    //是否删除

    public GameObject gameobject;                   //动作对象
    public Transform transform;                     //动作对象的transform
    public ISSActionCallback callback;              //回调函数

    /*防止用户自己new对象*/
    protected SSAction() { }                      

    public virtual void Start() {
        throw new System.NotImplementedException();
    }

    public virtual void Update() {
        throw new System.NotImplementedException();
    }
}

/*子类 - 移动到指定位置*/
public class SSMoveToAction : SSAction {
    public Vector3 target;  //目的地
    public float speed;     //移动速率

    private SSMoveToAction() { }
    public static SSMoveToAction GetSSAction(Vector3 _target, float _speed) {
        //让unity自己创建一个SSMoveToAction实例确保内存正确回收
        SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();
        action.target = _target;
        action.speed = _speed;
        return action;
    }

    //C#必须显示声明重写父类虚函数
    public override void Start() {
        //该动作建立时无需任何操作
    }

    public override void Update() {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        //动作完成，通知动作管理者或动作组合
        if (this.transform.position == target) {
            this.destroy = true;
            this.callback.SSActionEvent(this);      
        }
    }
}

/*动作组合*/
public class SequenceAction : SSAction, ISSActionCallback {
    public List<SSAction> sequence;    //动作的列表
    public int repeat = -1;            //-1就是无限循环做组合中的动作
    public int start = 0;              //当前做的动作的索引

    public static SequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence) {
        //让unity自己创建一个SequenceAction实例
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.sequence = sequence;
        action.repeat = repeat;
        action.start = start;
        return action;
    }

    public override void Start() {
        //对每个动作执行
        foreach (SSAction action in sequence) {
            action.gameobject = this.gameobject; 
            action.transform = this.transform;
            action.callback = this;                 //每一个动作的回调函数为该动作组合
            action.Start();
        }
    }

    public override void Update() {
        if (sequence.Count == 0) return;
        if (start < sequence.Count) {
            sequence[start].Update();               //start在回调函数中递加
        }
    }

    //实现接口ISSActionCallback
    public void SSActionEvent(
        SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null
    ) {
        source.destroy = false;                     //可能会无限循环所以先不删除
        this.start++;                               //下一个动作
        if (this.start >= sequence.Count) {
            this.start = 0;
            if (repeat > 0) repeat--;               //repeat<0就不会再减小
            if (repeat == 0) {                      //动作组合结束
                this.destroy = true;                //删除
                this.callback.SSActionEvent(this);  //通知管理者
            }
        }
    }

    void OnDestroy() {
        //如果自己被删除则应该释放自己管理的动作   
    }
}

/*动作管理基类*/
public class SSActionManager : MonoBehaviour, ISSActionCallback {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();    //动作字典
    private List<SSAction> waitingAdd = new List<SSAction>();                       //等待执行的动作列表
    private List<int> waitingDelete = new List<int>();                              //等待删除动作的key的列表                

    protected void Update() {
        //获取动作实例将等待执行的动作加入字典并清空待执行列表
        foreach (SSAction ac in waitingAdd) {
            actions[ac.GetInstanceID()] = ac;                                       
        }
        waitingAdd.Clear();

        //对于字典中每一个pair，看是执行还是删除
        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction ac = kv.Value;
            if (ac.destroy) {
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable) {
                ac.Update();
            }
        }

        //删除所有已完成的动作并清空待删除列表
        foreach (int key in waitingDelete) {
            SSAction ac = actions[key];
            actions.Remove(key);
            Object.Destroy(ac);
        }
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager) {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    public void SSActionEvent(
        SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null) {
    }
}

/*本次游戏场景中动作管理类*/
public class PADSceneActionManager : SSActionManager {
    public FirstControllor sceneController;

    private SequenceAction boatMove;
    private SequenceAction roleMove;

    protected void Start() {
        sceneController = (FirstControllor)SSDirector.GetInstance().CurrentScenceController;
        sceneController.actionManager = this;
    }

    protected new void Update() {
        base.Update();
    }

    public void moveBoat(GameObject boat, Vector3 endPos, float speed) {
        SSAction action1 = SSMoveToAction.GetSSAction(endPos, speed);
        boatMove = SequenceAction.GetSSAcition(0, 0, new List<SSAction> { action1 });
        this.RunAction(boat, boatMove, this);
    }

    public void moveRole(GameObject role, Vector3 middlePos, Vector3 endPos, float speed) {
        //两段移动
        SSAction action1 = SSMoveToAction.GetSSAction(middlePos, speed);
        SSAction action2 = SSMoveToAction.GetSSAction(endPos, speed);
        //repeat=1，start=0，两个动作
        roleMove = SequenceAction.GetSSAcition(1, 0, new List<SSAction> { action1, action2 });
        this.RunAction(role, roleMove, this);
    }
}