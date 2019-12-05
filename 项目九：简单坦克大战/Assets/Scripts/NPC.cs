using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {
	public delegate void recycle(GameObject tank);
	public delegate void win(int result);
	public static event recycle recycleEvent;
	private int countNPC;

	private Vector3 target;
	private FirstController sceneController;

    /*血条*/
    public UGUI_H HP;
    void Awake () {
		sceneController = Director.getDirector().currentSceneController;
	}

	void Start() {
		StartCoroutine(shoot());
	}
	
	void FixedUpdate () {
		if(sceneController.getResult()==0) {
			target = sceneController.getPlayerPosition();
			if(HP.GetHP() <= 0.0f && recycleEvent != null) {
				recycleEvent(this.gameObject);
				sceneController.decreaseCountNPC();
			}
			else {
				NavMeshAgent agent = GetComponent<NavMeshAgent>();
				agent.SetDestination(target);
			}
		}
		else {
			NavMeshAgent agent = GetComponent<NavMeshAgent> ();
			agent.velocity = Vector3.zero;
			agent.ResetPath();
		}
	}

	IEnumerator shoot() {
		while(sceneController.getResult()==0) {
			for(double i = 3; i > 0; i -= Time.deltaTime) {
				yield return null;
			}
			if (Vector3.Distance(transform.position, target) < 20) {
				Factory myFactory = Singleton<Factory>.Instance;
				GameObject bullet = myFactory.getBullet(tankType.NPC);
				bullet.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z) + transform.forward*1.5f;
				bullet.transform.forward = transform.forward;
				Rigidbody rb = bullet.GetComponent<Rigidbody>();
				rb.AddForce(bullet.transform.forward * 5, ForceMode.Impulse);
            }
		}
	}
}
