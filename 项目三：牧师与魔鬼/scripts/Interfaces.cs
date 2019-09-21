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
        int Check();
    }
}
