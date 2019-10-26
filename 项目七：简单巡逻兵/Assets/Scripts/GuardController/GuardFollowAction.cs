using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFollowAction : SSAction {
    private GameObject player;        
    private GuardData data;
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec; // 平面移动向量
    private float speed;

    private GuardFollowAction() {}

    public override void Start() {
        data = gameobject.GetComponent<GuardData>();
        anim = gameobject.GetComponent<Animator>();
        rigid = gameobject.GetComponent<Rigidbody>();
        speed = data.runSpeed;
        anim.SetFloat("forward", 2.0f);
    }

    public static GuardFollowAction GetSSAction(GameObject player) {
        GuardFollowAction action = CreateInstance<GuardFollowAction>();
        action.player = player;
        return action;
    }

    public override void Update() {
        //保留供物理引擎调用
        planarVec = gameobject.transform.forward * speed;
    }

    public override void FixedUpdate() {
        transform.LookAt(player.transform.position);
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z);
        
        //如果玩家脱离该区域则继续巡逻
        if (data.playerSign != data.sign) {
            this.destroy = true;
            this.callback.SSActionEvent(this, SSActionEventType.Competeted, 1, this.gameobject);
        }
    }
}
