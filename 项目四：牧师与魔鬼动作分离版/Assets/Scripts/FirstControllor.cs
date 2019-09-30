using UnityEngine.SceneManagement;
using UnityEngine;
using ControllerApplication;
using InterfaceApplication;
using CheckApplication;

public class FirstControllor : MonoBehaviour, ISceneController, IUserAction {
    public LandModel startLand;           
    public LandModel endLand;
    public Water water;
    public BoatModel boat;
    private RoleModel[] roles;
    private UserGUI GUI;
    public Check gameStatusManager;
    public PADSceneActionManager actionManager; //添加动作管理类

    void Start() {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        GUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        actionManager = gameObject.AddComponent<PADSceneActionManager>() as PADSceneActionManager;
        gameStatusManager = gameObject.AddComponent<Check>() as Check;
        LoadResources();
    }

    public void LoadResources() {
        water = new Water();
        startLand = new LandModel("start");
        endLand = new LandModel("end");
        boat = new BoatModel();
        roles = new RoleModel[6];
        for (int i = 0; i < 3; i++) {
            RoleModel role = new RoleModel("priest", startLand.GetEmptyPosition());
            role.SetName("priest" + i);
            startLand.AddRole(role);
            roles[i] = role;
        }
        for (int i = 0; i < 3; i++) {
            RoleModel role = new RoleModel("devil", startLand.GetEmptyPosition());
            role.SetName("devil" + i);
            startLand.AddRole(role);
            roles[i + 3] = role;
        }
    }

    public void MoveBoat(){
        if (boat.IsEmpty() || GUI.sign != 0) return;
        Vector3 endPos;
        boat.ChangeBoatSign();
        if (boat.GetBoatSign() == -1) endPos = new Vector3(-5, 0.5f, 0);
        else endPos = new Vector3(5, 0.5f, 0);
        actionManager.moveBoat(boat.GetGameObject(), endPos, boat.boatSpeed);
        GUI.sign = gameStatusManager.CheckGame();
    }

    public void MoveRole(RoleModel role) {
        if (GUI.sign != 0) return;
        Vector3 middlePos, endPos;
        if (role.IsOnBoat()) {
            if (boat.GetBoatSign() == 1) {
                boat.DeleteRoleByName(role.GetName());
                endPos = startLand.GetEmptyPosition();
                role.SetLand(1);
                startLand.AddRole(role); 
            }
            else {
                boat.DeleteRoleByName(role.GetName());
                endPos = endLand.GetEmptyPosition();   
                role.SetLand(-1);
                endLand.AddRole(role);
            }
            middlePos = new Vector3(role.GetGameObject().transform.position.x, endPos.y, endPos.z);
            actionManager.moveRole(role.GetGameObject(), middlePos, endPos, role.roleSpeed);
            role.GetGameObject().transform.parent = null;
            role.SetBoat(false);
        }
        else {
            if (role.GetLand() == 1) {
                if (boat.GetEmptyNumber() == -1 || startLand.GetLandSign() != boat.GetBoatSign()) return;
                startLand.DeleteRoleByName(role.GetName());
            }
            else {
                if (boat.GetEmptyNumber() == -1 || endLand.GetLandSign() != boat.GetBoatSign()) return;
                endLand.DeleteRoleByName(role.GetName());
            }
            endPos = boat.GetEmptyPosition();
            middlePos = new Vector3(endPos.x, role.GetGameObject().transform.position.y, endPos.z);
            actionManager.moveRole(role.GetGameObject(), middlePos, endPos, role.roleSpeed);
            role.GetGameObject().transform.parent = boat.GetGameObject().transform;
            role.SetBoat(true);
            boat.AddRole(role);
        }
        GUI.sign = gameStatusManager.CheckGame();
    }

    public void Restart() {
        //重新加载场景
        SceneManager.LoadScene(0);
        LoadResources();
    }

    
}