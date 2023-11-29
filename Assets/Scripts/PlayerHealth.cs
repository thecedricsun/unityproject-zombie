using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] public float health, maxHealth;
    // we want to show the health bar of the player on the screen, with a canvas
    // reference to script PlayerHealthBar
    [SerializeField] PlayerHealthBar playerHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        playerHealthBar.UpdateHealthBar(health, maxHealth);
        
        if (health <= 0f)
        {
            // debug
            Debug.Log("Player died");
            Die();
            
        }
    }

    private void Die()
    {
        // list of things we could do on player death
        // death animation
        // death sound
        // death particle effect
        // death screen similar to pause
        // death screen with options to restart or quit, showing the score

        // for now, we stop the runtime
        Time.timeScale = 0f;
    }
}
