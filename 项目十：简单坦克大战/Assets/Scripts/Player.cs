using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public delegate void destroy(int result);
	public static event destroy destroyEvent;
    /*血条*/
    public UGUI_H HP;

    void Start () {
		
	}
	
	void FixedUpdate() {
		if (HP.GetHP() <= 0) {
			this.gameObject.SetActive(false);
			if(destroyEvent != null) {
				destroyEvent(-1);
			}
		}		
	}
}
