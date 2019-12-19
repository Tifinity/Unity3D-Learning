---
title: "Unity3D项目十一：简单AR"
date: 2019-12-19T13:02:33+08:00
draft: false
---

# 简单AR学习

## 作业要求

1、 图片识别与建模

2、 虚拟按键小游戏

## 项目地址

[Github]()

## 实现效果

[Bilibili]()

## 准备工作

### 安装Java和AndroidSDK

要导出到安卓手机的话需要进行环境的配置，过程比较麻烦，如果只是在电脑上运行的话不需要进行这一步。

安装Java：[官网](https://www.java.com/zh_CN/)

安装android sdk:[一个博客](https://blog.csdn.net/Akatsuki__Itachi/article/details/90813230)

### 使用Vuforia

[Vuforia官网](https://developer.vuforia.com)

[Vuforia官方文档](https://library.vuforia.com/articles/Training/getting-started-with-vuforia-in-unity.html)

### 在Unity3D中安装Android平台支持

文件->Build Setting，选择Android，点击切换平台。

如果没有会有下载按钮，下载安装即可。

### 在Unity3D中安装Vuforia支持

文件->Build Settings->Android->切换平台->PlaySettings->XR设置

在这里勾选”支持虚拟现实“，SDKs中添加Vuforia，如果没有下方会有下载按钮，下载安装即可。

![image-20191218200028135](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191218200028135.png)

### 申请密钥

右上角注册->Develop->LicenseManager->Get Development Key，免费开发者每个月可以扫描1000次。

![image-20191218144125699](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191218144125699.png)

随便输入一个LicenseName，下面打勾，然后Confirm即可。

![image-20191218152540394](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191218152540394.png)

项目就创建好了。

![image-20191218152618585](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191218152618585.png)

然后添加数据库用于保存识别图的数据。

Develop->Target Magager->Add Database

![image-20191218170754664](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191218170754664.png)

进入创建好的数据库，Add Target添加识别图。

![image-20191218170838215](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191218170838215.png)

拍一张照上传，选择Single Image，Width的默认单位是米，我输入的是0.1。识别图就是之后你的摄像头拍到这个东西，就会执行你定义好的操作。

![image-20191218171138871](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191218171138871.png)

上传之后会生成识别图的特征点数据包，Vuforia还会为你的识别图打分，星级越高说明识别效果越好。

以下关于图像星级的说明引用自CSDN的[Wonderful_sky](https://blog.csdn.net/Wonderful_sky/article/details/80744334)师兄，感谢。

> 图像的星级
> 我们会注意到将识别图上传到数据库时，target项右侧会有星级显示，星级越高代表识别图的质量越高。你可能会问，何为识别图质量？识别图质量有什么用？答案是，你踩过坑之后就知道了。经试验，影响识别图最主要的因素就是待识别图的对比度，这里应该要极力避免使用有很多连续相同或相似大色块的图片，因为分析识别图特征点的原理是 根据色块边缘 来决定的，色彩变化越丰富，色块边缘、棱角就越多（如果棱角分布均匀且每个色块都很小那就再好不过了），进而特征点就越多，而特征点越多就意味着识别图质量越高。另外，以下三点则是我总结的识别图质量最主要影响的三个方面：
>
> - 星级越高越容易识别
> - 星级越高识别速度越快
> - 星级越高更不容易出现抖动

下载数据库，Download Database->Unity Editor，得到一个Unity包，之后用到。

回到Unity中，Resources->VuforiaConfiguration，复制之前的License Key粘贴到App License Key中。

![image-20191218201812956](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191218201812956.png)

现在鼠标右键->创建游戏对象，有了Vuforia Engine，创建一个ARCamera和一个ImageTarget。将之前下载的数据库包导入，上图中的Databases属性会变成你的数据库，并且新版本会自动加载，不用再做设置。接下来选中ImageTarget，设置你要是用的识别图，如果自己拍的效果不够好，也可以在网上下载。

![image-20191219104550286](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191219104550286.png)

另外，虚拟按钮集成到了ImageTarget->Advanced里，点击添加虚拟按钮后就会在ImageTarget下生成子对象。

![image-20191219111829733](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191219111829733.png)

需要注意的是，虚拟按钮必须放在识别图内，因为虚拟按钮的原理是根据识别图被遮挡的特征点是否在此按钮范围内来判断按钮是否被按下。

![image-20191219134206119](https://github.com/Tifinity/MyImage/raw/master/Unity3D/hw11/image-20191219134206119.png)

下方两个大的色块是虚拟按钮，而两个小的色块是Plane，用来在运行时提示这里有按钮。

### 代码实现

脚本挂在ImageTarget上，先找到子对象中所有按钮，然后根据按钮名字来判断执行什么功能。

~~~C#
public class ButtonTest : MonoBehaviour, IVirtualButtonEventHandler { 
    private GameObject sphere;
    void Start() {
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>(); 
        for (int i = 0; i < vbs.Length; ++i) {
            vbs[i].RegisterEventHandler(this);
        }
        sphere = transform.Find("Sphere").gameObject;
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb){
        switch (vb.VirtualButtonName){
            case "b1":
                sphere.transform.position = new Vector3(1f, 0f, 0f);
                break;
            case "b2":
                sphere.transform.position = new Vector3(-1f, 0f, 0f);
                break;
        }
        Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb){
        switch (vb.VirtualButtonName){
            case "b1":
                break;
            case "b2":
                break;
        }
        Debug.Log("OnButtonReleased: " + vb.VirtualButtonName);
    }
}
~~~