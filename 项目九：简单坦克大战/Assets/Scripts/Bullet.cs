using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float explosionRadius = 3f;

    public tankType type;

	public void setTankType(tankType type) {
		this.type = type;
	}
    void OnCollisionEnter(Collision other) {
        Factory myFactory = Singleton<Factory>.Instance;
        GameObject o = other.gameObject;
        if (o.tag == "tankPlayer" && this.type == tankType.NPC) {
            o.GetComponent<Player>().HP.SetHP(0, 1f);
        }  
        else if(o.tag == "tankNPC" && this.type == tankType.PLAYER ) {
            o.GetComponent<NPC>().HP.SetHP(0, 1f);
        }
        myFactory.recycleBullet(this.gameObject);
    }
}
