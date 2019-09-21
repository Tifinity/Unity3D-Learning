using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class solar_beh : MonoBehaviour {
    public Transform sun;
    public Transform mercury;
    public Transform venus;
    public Transform earth;
    public Transform mars;
    public Transform jupiter;
    public Transform saturn;
    public Transform uranus;
    public Transform neptune;
    public Transform moonPoint;
    public Transform moon;

    void Start() {

    }

    void Update() {
        /*公转*/
        mercury.RotateAround(this.transform.position, new Vector3(4, 10, 0), 47 * Time.deltaTime);
        venus.RotateAround(this.transform.position, new Vector3(2,10, 0), 35 * Time.deltaTime);
        earth.RotateAround(this.transform.position, new Vector3(1, -10, 0), -30 * Time.deltaTime);
        moonPoint.RotateAround(this.transform.position, new Vector3(1, -10, 0), -30 * Time.deltaTime);
        moon.RotateAround(moonPoint.position, new Vector3(1, 10, 0), 30 * Time.deltaTime);
        mars.RotateAround(this.transform.position, new Vector3(2, 10, 0), 24 * Time.deltaTime);
        jupiter.RotateAround(this.transform.position, new Vector3(2, -10, 0), -13 * Time.deltaTime);
        saturn.RotateAround(this.transform.position, new Vector3(1, -10, 0), -9 * Time.deltaTime);
        uranus.RotateAround(this.transform.position, new Vector3(2, 10, 0), 6 * Time.deltaTime);
        neptune.RotateAround(this.transform.position, new Vector3(1, -10, 0), -5 * Time.deltaTime);
        /*自传*/
        earth.Rotate(Vector3.up * Time.deltaTime * 250);
        moon.Rotate(Vector3.up * Time.deltaTime * 250);
        mercury.Rotate(Vector3.up * Time.deltaTime * 300);
        venus.Rotate(Vector3.up * Time.deltaTime * 280);
        mars.Rotate(Vector3.up * Time.deltaTime * 220);
        jupiter.Rotate(Vector3.up * Time.deltaTime * 180);
        saturn.Rotate(Vector3.up * Time.deltaTime * 160);
        uranus.Rotate(Vector3.up * Time.deltaTime * 150);
        neptune.Rotate(Vector3.up * Time.deltaTime * 140);
    }
}
