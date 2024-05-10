using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// General Health System

public class CharacterHealth : HealthSystem
{
    public int maxHealth;
    public bool iframesEnabled = false;

    protected override void Initialize()
    {
        base.Initialize();  // Don't hide the functions
        base.currentHealth = maxHealth;
    }

    protected override bool ApplyDamage(int amount)
    {
        if (iframesEnabled)
        {
            // not sure how we want to do iframe time scheduling
            float iframeEnd = 0f;
            if(Time.time < iframeEnd)
            {
                return false;
            }
        }

        currentHealth -= amount;
        Debug.Log("Current Health:" + currentHealth);

        if(currentHealth <= 0 ) 
        {
            currentHealth = 0;
            Die();
        }

        return true;
    }

    protected override bool ApplyHeal(int amount)
    {
        if (IsDead())
        {
            Debug.Log("Error: Healed when dead");
            Debug.Log("Use Health Change to Revive A Player");
        }

        currentHealth += amount; 

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        return true;
    }

}
