using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControllerApplication;

namespace InterfaceApplication {
    //场景接口
    public interface ISceneController {
        void LoadResources();
    }

    //用户接口，包含所有用户交互的事件
    public interface IUserAction {
        //移动船
        void MoveBoat();
        //移动角色
        void MoveRole(RoleModel role);
        //重新开始
        void Restart();
        //检查游戏状态
    }

    public enum SSActionEventType : int { Started, Competeted }

    public interface ISSActionCallback {
        void SSActionEvent(
            SSAction source, SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0, string strParam = null, Object objectParam = null
        );
    }
}
