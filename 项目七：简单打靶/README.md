# 简单打靶

[TOC]

## 游戏规则与要求

- 1. 游戏内容要求：
     1. 靶对象为 5 环，按环计分；
     2. 箭对象，射中后要插在靶上
        - **增强要求**：射中后，箭对象产生颤抖效果，到下一次射击 或 1秒以后
     3. 游戏仅一轮，无限 trials；
        - **增强要求**：添加一个风向和强度标志，提高难度



## 项目地址与演示视频

项目地址 -> [传送门🚪](https://github.com/Tifinity/Unity3DStudy-master/tree/master/%E9%A1%B9%E7%9B%AE%E4%B8%83%EF%BC%9A%E7%AE%80%E5%8D%95%E6%89%93%E9%9D%B6)

视频连接 -> [传送门🚪]( https://www.bilibili.com/video/av71567483/ )



## 具体实现

- 与上一个项目相同，将动作管理相关的类改成适配物理引擎。箭工厂与飞碟工厂相同，

- 主要的内容在FirstSenceController中，

  首先时Update函数，使用LookAt函数将弓箭转向鼠标的方向，如果没有箭则从工厂中取出一支箭放在弓上，检测鼠标左键按下则射箭。

  ~~~c#
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
  ~~~

  Shoot函数：通知动作管理器根据初始的力和风力射箭，并且设置新的更强的风向。

  ~~~c#
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
  ~~~

  

- 动作管理器与打飞碟相同，箭飞行的动作很简单，给一个冲击力和风力即可。

  ~~~C#
  public class ArrowFlyAction : SSAction {
      public Vector3 _force;
      public Vector3 _wind;
  
      private ArrowFlyAction() { }
      public static ArrowFlyAction GetSSAction(Vector3 wind, Vector3 force) {
          ArrowFlyAction action = CreateInstance<ArrowFlyAction>();
          action._force = force;
          action._wind = wind;
          return action;
      }
      public override void Start() {
          gameobject.GetComponent<Rigidbody>().AddForce(_force, ForceMode.Impulse);
          gameobject.GetComponent<Rigidbody>().AddForce(_wind);
      }
  
      public override void Update() {}
  
      public override void FixedUpdate(){
          if (transform.position.y < -30) {
              this.destroy = true;
              this.callback.SSActionEvent(this);
          }
      }
  }
  ~~~

  

- CollisionDetection脚本检测碰撞，将其挂在靶子的每一环上，使用OnTriggerEnter函数检测碰撞，碰撞之后将箭头隐藏并且将箭设置为运动学停止其运动。

  ~~~c#
public class CollisionDetection : MonoBehaviour {
      public FirstSceneController scene_controller;
      public ScoreRecorder recorder;
  
      void Start() {
          scene_controller = SSDirector.GetInstance().CurrentScenceController as FirstSceneController;
          recorder = Singleton<ScoreRecorder>.Instance;
      }
  
      void OnTriggerEnter(Collider arrow_head) {
          Transform arrow = arrow_head.gameObject.transform.parent;
          if (arrow == null) return;
          arrow.GetComponent<Rigidbody>().isKinematic = true;
          arrow_head.gameObject.SetActive(false);
          recorder.Record(this.gameObject);
      }
  }
  ~~~
  
  
  


## 总结

本次项目主要是关于碰撞的操作，需要注意的一点是当物体运动速度比较快或者碰撞体比较薄时，可能会出现穿过去而没有出发碰撞的情况，此时需要将速度快的物体即箭碰撞检测调为持续动态，将不动的物体碰撞检测设为持续。

靶子：

![1571328345(1)](image\1571328345(1).jpg)

箭：

![1571328382(1)](image\1571328382(1).jpg)



## 参考资料

[师兄博客]( https://blog.csdn.net/c486c/article/details/80058316 )

