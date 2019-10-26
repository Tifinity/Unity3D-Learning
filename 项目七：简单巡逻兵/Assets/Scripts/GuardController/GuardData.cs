using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardData : MonoBehaviour {
    public GameObject model;
    public float walkSpeed = 1.2f;
    public float runSpeed = 2.5f;
    public int sign;                      //标志巡逻兵在哪一块区域
    public bool isFollow = false;         //是否跟随玩家
    public int playerSign = -1;           //当前玩家所在区域标志
    public Vector3 start_position;        //当前巡逻兵初始位置   

    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;

    void Awake() {
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    public void OnGround() {
        anim.SetBool("OnGround", true);
    }
    public void OnGroundEnter() {
        
    }
}
