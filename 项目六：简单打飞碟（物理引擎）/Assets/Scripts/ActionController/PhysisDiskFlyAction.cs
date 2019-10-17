using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysisDiskFlyAction : SSAction {
    private Vector3 start_vector;                              
    public float power;
    private PhysisDiskFlyAction() { }
    public static PhysisDiskFlyAction GetSSAction(int lor, float power) {
        PhysisDiskFlyAction action = CreateInstance<PhysisDiskFlyAction>();
        if (lor == -1) {
            action.start_vector = Vector3.left * power;
        }
        else {
            action.start_vector = Vector3.right * power;
        }
        action.power = power;
        return action;
    }

    public override void Update() { }

    public override void FixedUpdate() {
        if (transform.position.y <= -10f) {
            gameobject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }

    public override void Start() {
        gameobject.GetComponent<Rigidbody>().AddForce(start_vector*3, ForceMode.Impulse);
    }
}