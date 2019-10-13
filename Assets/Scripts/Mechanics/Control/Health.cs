using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int maxHealth = 1;

    public int currentHealth;

    public bool isDead { get; private set; }

    void Awake() {
        reset();
    }

    public void dealDamage(int damage) {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if(currentHealth == 0) {
            isDead = true;
        }
    }

    public void heal(int healAmount) {
        if(!isDead) {
            currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        }
    }

    public void reset() {
        currentHealth = maxHealth;
        isDead = false;
    }
}
