using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFlyAction : SSAction {
    public Vector3 _force;
    public Vector3 _wind;

    private ArrowFlyAction() { }
    public static ArrowFlyAction GetSSAction(Vector3 wind, Vector3 force) {
        ArrowFlyAction action = CreateInstance<ArrowFlyAction>();
        action._force = force;
        action._wind = wind;
        return action;
    }
    public override void Start() {
        gameobject.GetComponent<Rigidbody>().AddForce(_force, ForceMode.Impulse);
        gameobject.GetComponent<Rigidbody>().AddForce(_wind);
    }

    public override void Update() {}

    public override void FixedUpdate(){
        if (transform.position.y < -30) {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }
}
