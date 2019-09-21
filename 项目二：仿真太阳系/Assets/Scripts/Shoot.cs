using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {
    public GameObject bullet;
    public float speed = 100;
    void Start() {
 
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            GameObject b = GameObject.Instantiate(bullet, transform.position, transform.rotation);
            Rigidbody rgd = b.GetComponent<Rigidbody>();
            rgd.velocity = transform.forward * speed;
        }
    }
}
