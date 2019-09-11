# 离散仿真引擎基础

### 简答题

##### 1. 解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系。

- 游戏对象：出现在场景中，一般有人物、道具和风景等等，充当组件的容器，实现真正的功能。游戏对象自己不做任何事。他们需要专有属性，才可以成为一个角色，一个环境，或一个特殊效果。

- 资源：表示可以在项目中使用的任何素材。可能来自在Unity之外创建的文件，例如3D模型、音频文件，图像，脚本或Unity支持的任何其他类型的文件。还有一些可以在Unity中创建的资源类型，如动画控制器、音频混音器或渲染纹理。

##### 2. 下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）

​																	![1568017251(1)](T:\TH\大三上\3D游戏设计\2离散仿真引擎基础\1568017251(1).jpg)

游戏对象树的结构主要包括：摄像机，场景，光源，风，开始位置等

![1568017302(1)](T:\TH\大三上\3D游戏设计\2离散仿真引擎基础\1568017302(1).jpg)

资源目录包括：动画，素材，文本，模型，场景，预设等

##### 3. 编写一个代码，使用 debug 语句来验证 [MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html) 基本行为或事件触发的条件

```c#
public class Test : MonoBehaviour {
    void Awake() {
        Debug.Log ("onAwake");
    }
    
    void Start () {
        Debug.Log ("onStart");
    }

    void Update () {
        Debug.Log ("onUpdate");
    }

    void FixedUpdate() {
        Debug.Log ("onFixedUpdate");
    }
    
    void LateUpdate() {
        Debug.Log ("onLateUpdate");
    }
    
    void OnGUI() {
        Debug.Log ("onGUI");
    }
    
    void OnDisable() {
        Debug.Log ("onDisable");
    }
    
    void OnEnable() {
        Debug.Log ("onEnable");
    }
}
```

##### 4. 查找脚本手册，了解 [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)，Transform，Component 对象

- 分别翻译官方对三个对象的描述（Description）

  GameObject ：游戏对象是Unity中表示游戏角色，游戏道具和游戏场景的基本对象。他们自身无法完成许多功能，但是他们构成了那些给予他们实体功能的组件的容器。

  Transform ：转换组件决定了游戏场景中每个游戏对象的位置，旋转和缩放比例。每个游戏对象都有转换组件。

  Component ：组件是游戏中对象和行为的细节。它是每个游戏对象的功能部分。

- 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件
  
  table对象的属性是GameObject（游戏对象）
  
  第一个选择框是activeSelf属性，第二个是对象名称，第三个是static属性，下一行是标签Tag和Layer，第三行是预设。
  
  table对象的Transform中：
  
  - Position是空间位置
  - Rotation是旋转角度
  
  table的部件：Transform，Mesh Filter（网格过滤器），Box Collider（盒状碰撞器），Mesh Renderer（网格渲染器）。
  
- Scale是比例大小
  
  table的部件有chair1，chair2，chair3，chair4
  
  ![ch02-homework](T:\TH\大三上\3D游戏设计\2离散仿真引擎基础\ch02-homework.png)
  
  - 用 UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）
  
  
  
- 整理相关学习资料，编写简单代码验证以下技术的实现：

  - 查找对象
  
    ```c#
    var cube1 = GameObject.Find("Cube");
    if (null != cube1) Debug.Log ("find cube");
    var cube2 = GameObject.Find ("/Cube");
    if (null != cube2) Debug.Log ("find cube");
    ```
  
  - 添加子对象
  
    ```c#
    GameObject child = GameObject.CreatePrimitive (PrimitiveType.Cube);
    child.transform.position = new Vector3 (0, 0, 0);
    child.transform.parent = father.transform;
    ```
  
  - 遍历对象树
  
    ```c#
    GameObject[] game = GameObject.FindObjectsOfType<GameObject>();
    foreach (GameObject tmp in game) {
    	Debug.Log (tmp.name);
    }
    ```
  
  - 清除所有子对象
  
    ```c#
     foreach (GameObject tmp in game) {
         GameObject.Destroy (tmp);
     }
    ```
  

##### 5. 资源预设（Prefabs）与 对象克隆 (clone)

  - 预设（Prefabs）有什么好处？

    预设类似于一个模板，通过预设可以创建相同属性的对象，这些对象和预设关联。一旦预设发生改变，所有通过预设实例化的对象都会产生相应的变化，适合批量处理。

  - 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？

    1. 实例对象会随着预设对象的改变而改变；对象克隆不会随克隆本体改变。
    2. 对象克隆需要场景中有被克隆对象；预设则不需要。

  - 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象。

    ```c#
    void Start () {
    	GameObject table_1 = (GameObject)Instantiate(table);
    }
    ```
    
##### 6. 尝试解释组合模式（Composite Pattern / 一种设计模式）。使用 BroadcastMessage() 方法向子对象发送消息。

- 组合模式允许用户将对象组合成树形结构表现“整体-部分”的层次结构，使得客户以一致的方式处理单个对象以及对象的组合，组合模式实现的关键地方是单个对象与复合对象必须实现相同的接口，这就是组合模式能够将组合对象和简单对象进行一致处理的原因。 



### 编程实践

**井字棋传送门** -> [🚪](https://github.com/Tifinity/Unity3DStudy-master)