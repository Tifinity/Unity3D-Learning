using UnityEngine.SceneManagement;
using UnityEngine;
using ControllerApplication;
using InterfaceApplication;

public class FirstControllor : MonoBehaviour, ISceneController, IUserAction {
    public LandModel startLand;           
    public LandModel endLand;
    public Water water;
    public BoatModel boat;
    private RoleModel[] roles;
    private UserGUI GUI;

    void Start() {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        GUI = gameObject.AddComponent<UserGUI>() as UserGUI;
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
        boat.BoatMove();
        GUI.sign = Check();
    }

    public void MoveRole(RoleModel role) {
        if (GUI.sign != 0) return;
        if (role.IsOnBoat()) {
            if (boat.GetBoatSign() == 1) {
                boat.DeleteRoleByName(role.GetName());
                role.Move(startLand.GetEmptyPosition());
                role.GetRole().transform.parent = null;
                role.SetBoat(false);
                role.SetLand(1);
                startLand.AddRole(role);
                //Debug.Log("boat to startt");
            }
            else {
                boat.DeleteRoleByName(role.GetName());
                role.Move(endLand.GetEmptyPosition());
                role.GetRole().transform.parent = null;
                role.SetBoat(false);
                role.SetLand(-1);
                endLand.AddRole(role);
                //Debug.Log("boat to end");
            }       
        }
        else {
            if (role.GetLand() == 1) {
                if (boat.GetEmptyNumber() == -1 || startLand.GetLandSign() != boat.GetBoatSign()) return;
                startLand.DeleteRoleByName(role.GetName());
                role.Move(boat.GetEmptyPosition());
                role.GetRole().transform.parent = boat.GetBoat().transform;
                role.SetBoat(true);
                boat.AddRole(role);
                //Debug.Log("start to boat");
            }
            else {
                if (boat.GetEmptyNumber() == -1 || endLand.GetLandSign() != boat.GetBoatSign()) return;
                endLand.DeleteRoleByName(role.GetName());
                role.Move(boat.GetEmptyPosition());
                role.GetRole().transform.parent = boat.GetBoat().transform;
                role.SetBoat(true);
                boat.AddRole(role);
                //Debug.Log("end to boat");
            }
        }
        GUI.sign = Check();
    }

    public void Restart() {
        //重新加载场景
        SceneManager.LoadScene(0);
    }

    public int Check() {
        //0-游戏继续，1-失败，2-成功
        int startPriests = (startLand.GetRoleNum())[0];
        int startDevils = (startLand.GetRoleNum())[1];
        int endPriests = (endLand.GetRoleNum())[0];
        int endDevils = (endLand.GetRoleNum())[1];
        if (endPriests + endDevils == 6) return 2;
        int[] boatNum = boat.GetRoleNumber();
        //加上船上的人
        if (boat.GetBoatSign() == 1) {
            startPriests += boatNum[0];
            startDevils += boatNum[1];
        }
        else {
            endPriests += boatNum[0];
            endDevils += boatNum[1];
        }
        if ((endPriests > 0 && endPriests < endDevils) || (startPriests > 0 && startPriests < startDevils)) {
            return 1;
        }
        return 0;
    }
}