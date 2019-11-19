using UnityEngine;

public class IMGUI_H : MonoBehaviour {
    public float HP = 1.0f;
    private float tmpHP;

    private Rect HealthBar;

    void Start() {
        HealthBar = new Rect(50, 50, 200, 20);
        tmpHP = HP;
    }

    void OnGUI() {
        HP = Mathf.Lerp(HP, tmpHP, 0.05f);
        GUI.HorizontalScrollbar(HealthBar, 0.0f, HP, 0.0f, 1.0f);
    }

    public void SetHP(int flag, float damage) {
        if (flag == 1) {
            tmpHP = tmpHP + 0.1f > 1.0f ? 1.0f : tmpHP + 0.1f;
        }
        else if (flag == 0) {
            tmpHP = tmpHP - 0.1f < 0.0f ? 0.0f : tmpHP - 0.1f;
        }
    }

    public float GetHP() {
        return HP;
    }
}