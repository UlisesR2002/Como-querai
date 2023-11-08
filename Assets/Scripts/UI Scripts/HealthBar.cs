using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealSlider;
    public PlayerController controller;
    public float health;
    private float lerpspeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health = controller.health;
        if (healthSlider.value != health) 
        {
            healthSlider.value = health;
        }

        if(Input.GetKeyDown(KeyCode.Q)) { takeDamage(10); }
        if (Input.GetKeyDown(KeyCode.Z)) { takeDamage(50); }

        if (healthSlider.value != easeHealSlider.value)
        {
            easeHealSlider.value = Mathf.Lerp(easeHealSlider.value, health, lerpspeed);
        }
    }

    void takeDamage(float damage)
    {
        health -= damage;
    }
}
