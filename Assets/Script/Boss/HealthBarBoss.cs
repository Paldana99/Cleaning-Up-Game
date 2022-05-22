using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBoss : MonoBehaviour
{
    private Slider slider;
    [SerializeField]
    private EnemyDamage boss;

    void Awake() {
        slider = gameObject.GetComponent<Slider>();
    }


    public void SetMaxHealth(float maxhealth) {
        Debug.Log("max health called");
        slider.maxValue = maxhealth;
        slider.value = maxhealth;
    }

    public void SetHealth(float health) {
        slider.value = health;
    }

    void Update(){
        SetHealth(boss.life);
    }
}
