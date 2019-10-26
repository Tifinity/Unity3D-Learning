using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour {
    void OnCollisionEnter(Collision other) {
        //当玩家与侦察兵相撞
        if (other.gameObject.tag == "Guard") {
            Singleton<GameEventManager>.Instance.PlayerGameover();
        }
    }
}
