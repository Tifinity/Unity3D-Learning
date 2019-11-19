using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UGUI_H: MonoBehaviour {
    public Slider HPStrip;
    public Image fill;
    public float HP = 10f;
    private float tmpHP;
    void Awake() {
        HPStrip.value = HPStrip.maxValue = HP;
        tmpHP = HP;
        fill.color = Color.green;
    }

    void Update() {
        HP = Mathf.Lerp(HP, tmpHP, 0.05f);
        HPStrip.value = HP;
        if (HP<=5) {
            fill.color = Color.red;
        }
    }
    public void SetHP(int flag, float damage) {
        if(flag == 1 && HP <= 10) {
            tmpHP += damage;
        } else if(flag == 0 && HP > 0) {
            tmpHP -= damage;
        }
        
    }

    public float GetHP() {
        return HP;
    }
}
