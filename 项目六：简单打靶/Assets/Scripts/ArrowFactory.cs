using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory : MonoBehaviour {
    public GameObject arrow = null;                             
    private List<GameObject> used = new List<GameObject>();   
    private Queue<GameObject> free = new Queue<GameObject>();  
    public FirstSceneController sceneControler;

    public void Start() {
        sceneControler = (FirstSceneController)SSDirector.GetInstance().CurrentScenceController;
    }
    public GameObject GetArrow() {
        if (free.Count == 0) {
            arrow = Instantiate(Resources.Load<GameObject>("Prefabs/arrow"));
        } else {
            arrow = free.Dequeue();
            arrow.gameObject.SetActive(true);
        }
        used.Add(arrow);
        return arrow;
    }

    public void FreeArrow() {
        for (int i = 0; i < used.Count; i++) {
            if (used[i].gameObject.transform.position.y < -30) {
                used[i].gameObject.SetActive(false);
                free.Enqueue(used[i]);
                used.Remove(used[i]);
                break;
            }
        }
    }
}
