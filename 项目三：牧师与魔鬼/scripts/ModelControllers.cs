using UnityEngine;

namespace ControllerApplication {
    public class Water {
        GameObject water;
        public Water() {
            water = Object.Instantiate(
                Resources.Load<GameObject>("Prefabs/Water"),
                Vector3.zero, Quaternion.identity) as GameObject;
            water.name = "water";
        }
    }
    public class LandModel {
        GameObject land;
        //保存每个角色放在陆地上的位置
        Vector3[] positions;  
        //表示是开始岸还是结束岸
        int land_sign;   
        //该陆地上的角色
        RoleModel[] roles = new RoleModel[6];
        public LandModel(string land_mark) {
            positions = new Vector3[] {
                new Vector3(6.35F,2.014F,0), new Vector3(7.35f,2.014F,0), new Vector3(8.35f,2.014F,0),
                new Vector3(9.35f,2.014F,0), new Vector3(10.35f,2.014F,0), new Vector3(11.35f,2.014F,0)
            };
            if (land_mark == "start") land_sign = 1;
            else land_sign = -1;
            land = Object.Instantiate(
                    Resources.Load<GameObject>("Prefabs/Land"),
                    new Vector3(9 * land_sign, 1, 0), Quaternion.identity) as GameObject;
            land.name = land_mark + "Land";
        }
        public int GetLandSign() { return land_sign; }
        public int GetEmptyNumber() {
            for (int i = 0; i < roles.Length; i++) {
                if (roles[i] == null) return i;
            }
            return -1;
        }
        public Vector3 GetEmptyPosition() {
            Vector3 pos = positions[GetEmptyNumber()];
            pos.x = land_sign * pos.x;
            return pos;
        }
        public int[] GetRoleNum() {
            int[] count = { 0, 0 };
            for (int i = 0; i < roles.Length; i++) {
                if (roles[i] != null) {
                    if (roles[i].GetSign() == 0) count[0]++;
                    else count[1]++;
                }
            }
            return count;
        }
        public void AddRole(RoleModel role) {
            roles[GetEmptyNumber()] = role;
        }
        public RoleModel DeleteRoleByName(string name) {
            for (int i = 0; i < roles.Length; i++) {
                if (roles[i] != null && roles[i].GetName() == name) {
                    RoleModel role = roles[i];
                    roles[i] = null;
                    return role;
                }
            }
            return null;
        }
    }
    public class BoatModel {
        GameObject boat;
        Vector3[] startPos;
        Vector3[] endPos;
        Move move;
        Click click;
        int boat_sign = 1;    
        RoleModel[] roles = new RoleModel[2]; 
        public BoatModel() {
            boat = Object.Instantiate(
                Resources.Load<GameObject>("Prefabs/Boat"), 
                new Vector3(5, 0.5f, 0), Quaternion.identity) as GameObject;
            boat.name = "boat";
            move = boat.AddComponent(typeof(Move)) as Move;
            click = boat.AddComponent(typeof(Click)) as Click;
            click.SetBoat(this);
            startPos = new Vector3[] { new Vector3(5.5f, 1, 0), new Vector3(4.5f, 1, 0) };
            endPos = new Vector3[] { new Vector3(-4.5f, 1, 0), new Vector3(-5.5f, 1, 0) };
        }
        public bool IsEmpty() {
            for (int i = 0; i < roles.Length; i++) {
                if (roles[i] != null) return false;
            }
            return true;
        }
        public void BoatMove() {
            if (boat_sign == -1) move.MovePosition(new Vector3(5, 0.5f, 0)); 
            else move.MovePosition(new Vector3(-5, 0.5f, 0));
            boat_sign *= -1;
        }
        public int GetBoatSign() { return boat_sign; }
        public RoleModel DeleteRoleByName(string role_name) {
            for (int i = 0; i < roles.Length; i++) {
                if (roles[i] != null && roles[i].GetName() == role_name) {
                    RoleModel role = roles[i];
                    roles[i] = null;
                    return role;
                }
            }
            return null;
        }
        public int GetEmptyNumber() {
            for (int i = 0; i < roles.Length; i++) {
                if (roles[i] == null) return i;
            }
            return -1;
        }
        public Vector3 GetEmptyPosition() {
            Vector3 pos;
            if (boat_sign == -1) pos = endPos[GetEmptyNumber()];
            else pos = startPos[GetEmptyNumber()];
            Debug.Log(GetEmptyNumber());
            return pos;
        }
        public void AddRole(RoleModel role) {
            roles[GetEmptyNumber()] = role;
        }
        public GameObject GetBoat() { return boat; }
        public int[] GetRoleNumber() {
            int[] count = { 0, 0 };
            for (int i = 0; i < roles.Length; i++) {
                if (roles[i] == null) continue;
                if (roles[i].GetSign() == 0) count[0]++;
                else count[1]++;
            }
            return count;
        }
    }

    public class RoleModel {
        GameObject role;
        Click click;
        Move move;
        int role_sign;
        int land_sign;//在哪块陆地上
        bool on_boat;
        public RoleModel(string role_name, Vector3 pos) {
            if (role_name == "priest") {
                role = Object.Instantiate(
                    Resources.Load<GameObject>("Prefabs/Priest"),
                    pos, Quaternion.Euler(0, -90, 0)) as GameObject;
                role_sign = 0;
            }
            else {
                role = Object.Instantiate(
                    Resources.Load<GameObject>("Prefabs/Devil"),
                    pos, Quaternion.Euler(0, -90, 0)) as GameObject;
                role_sign = 1;
            }
            land_sign = 1;
            move = role.AddComponent(typeof(Move)) as Move;
            click = role.AddComponent(typeof(Click)) as Click;
            click.SetRole(this);
        }
        public int GetSign() { return role_sign; }
        public GameObject GetRole() {
            return role;
        }
        public string GetName() { return role.name; }
        public int GetLand() { return land_sign; }
        public void SetLand(int land) { land_sign = land; }
        public bool IsOnBoat() { return on_boat; }
        public void SetBoat(bool a) { on_boat = a; }
        public void SetName(string name) { role.name = name; }
        public void SetPosition(Vector3 pos) {
            role.transform.position = pos;
        }
        public void Move(Vector3 v) {
            move.MovePosition(v);
        }
    }
}
