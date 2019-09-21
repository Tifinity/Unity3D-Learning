using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_move : MonoBehaviour {
    private Rigidbody rd;
    public int force = 10;

    // Start is called before the first frame update
    void Start() {
        rd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rd.AddForce(new Vector3(h,0,v) * force);

    }
    /*
    private void OnCollisionEnter(Collision collision) {
        string name = collision.collider.name;
        if(collision.collider.tag == "pickup") {
            Destroy(collision.collider.gameObject);
        }
    }
    */
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "pickup") {
            Destroy(other.gameObject);
        }
    }
}
