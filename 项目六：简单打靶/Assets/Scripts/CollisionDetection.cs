using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {
    public FirstSceneController scene_controller;
    public ScoreRecorder recorder;

    void Start() {
        scene_controller = SSDirector.GetInstance().CurrentScenceController as FirstSceneController;
        recorder = Singleton<ScoreRecorder>.Instance;
    }

    void OnTriggerEnter(Collider arrow_head) {
        Transform arrow = arrow_head.gameObject.transform.parent;
        if (arrow == null) return;
        arrow.GetComponent<Rigidbody>().isKinematic = true;
        arrow_head.gameObject.SetActive(false);
        recorder.Record(this.gameObject);
    }
}
