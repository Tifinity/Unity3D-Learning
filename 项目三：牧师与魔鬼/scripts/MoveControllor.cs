using UnityEngine;

public class Move : MonoBehaviour {
    float move_speed = 120;
    int move_sign = 0;
    Vector3 end_pos;
    Vector3 middle_pos;
    void Update() {
        if (move_sign == 1) {
            transform.position = Vector3.MoveTowards(
                transform.position, middle_pos, move_speed * Time.deltaTime);
            if (transform.position == middle_pos)
                move_sign = 2;
        }
        else if (move_sign == 2) {
            transform.position = Vector3.MoveTowards(
                transform.position, end_pos, move_speed * Time.deltaTime);
            if (transform.position == end_pos)
                move_sign = 0;
        }
    }
    public void MovePosition(Vector3 position) {
        end_pos = position;
        if (position.y == transform.position.y) move_sign = 2;
        else if (position.y < transform.position.y) {
            middle_pos = new Vector3(position.x, transform.position.y, position.z);
        }
        else {
            middle_pos = new Vector3(transform.position.x, position.y, position.z);
        }
        move_sign = 1;
    }
}