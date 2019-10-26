using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPatrolAction : SSAction {
    private enum Dirction { EAST, NORTH, WEST, SOUTH };
    private float pos_x, pos_z;                 
    private float move_length;                 
    private bool move_sign = true;              
    private Dirction dirction = Dirction.EAST;  

    private GuardData data;
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec; // 平面移动向量
    private GuardPatrolAction() { }

    public override void Start() {
        data = gameobject.GetComponent<GuardData>();
        anim = gameobject.GetComponent<Animator>();
        rigid = gameobject.GetComponent<Rigidbody>();
        //播放走路动画
        anim.SetFloat("forward", 1.0f);
    }
    public static GuardPatrolAction GetSSAction(Vector3 location) {
        GuardPatrolAction action = CreateInstance<GuardPatrolAction>();
        action.pos_x = location.x;
        action.pos_z = location.z;
        //设定移动矩形的边长
        action.move_length = Random.Range(5, 6);
        return action;
    }

    public override void Update() {
        //保留供物理引擎调用
        planarVec = gameobject.transform.forward * data.walkSpeed;
    }

    public override void FixedUpdate() {
        //巡逻
        Gopatrol();
        //玩家进入该区域，巡逻结束，开始追逐
        if (data.playerSign == data.sign) {
            this.destroy = true;
            this.callback.SSActionEvent(this, SSActionEventType.Competeted, 0, this.gameobject);
        }
    }

    void Gopatrol() {
        if (move_sign) {
            //不需要转向则设定一个目的地，按照矩形移动
            switch (dirction) {
                case Dirction.EAST:
                    pos_x -= move_length;
                    break;
                case Dirction.NORTH:
                    pos_z += move_length;
                    break;
                case Dirction.WEST:
                    pos_x += move_length;
                    break;
                case Dirction.SOUTH:
                    pos_z -= move_length;
                    break;
            }
            move_sign = false;
        }
        this.transform.LookAt(new Vector3(pos_x, 0, pos_z));
        float distance = Vector3.Distance(transform.position, new Vector3(pos_x, 0, pos_z));

        if (distance > 0.9) {
            rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z);
        } else {
            dirction = dirction + 1;
            if(dirction > Dirction.SOUTH) {
                dirction = Dirction.EAST;
            }
            move_sign = true;
        }
    }
}
