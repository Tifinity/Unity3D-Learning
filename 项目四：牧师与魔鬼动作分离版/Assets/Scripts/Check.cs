using UnityEngine;

namespace CheckApplication {
    public class Check : MonoBehaviour {
        public FirstControllor sceneController;

        protected void Start() {
            sceneController = (FirstControllor)SSDirector.GetInstance().CurrentScenceController;
            sceneController.gameStatusManager = this;
        }

        public int CheckGame() {
            //0-游戏继续，1-失败，2-成功
            int startPriests = (sceneController.startLand.GetRoleNum())[0];
            int startDevils = (sceneController.startLand.GetRoleNum())[1];
            int endPriests = (sceneController.endLand.GetRoleNum())[0];
            int endDevils = (sceneController.endLand.GetRoleNum())[1];
            if (endPriests + endDevils == 6) return 2;
            int[] boatNum = sceneController.boat.GetRoleNumber();
            //加上船上的人
            if (sceneController.boat.GetBoatSign() == 1) {
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
}