using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class EnemyHealth : HealthSystem
{
    public int maxHealth;

    protected override void Initialize()
    {
        base.Initialize();  // Don't hide the functions
        base.currentHealth = maxHealth;
    }

    protected override bool ApplyDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Current Health:" + currentHealth);

        if (currentHealth <= 0)
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
            Debug.LogError("Error: Healed when dead");
            Debug.LogError("Use Health Change to Revive A Player");
        }

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        return true;
    }

    protected override void Die()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            SetDead(true);
        }
        EnemyController enemy = this.gameObject.GetComponent<EnemyController>();
        enemy.InvokeDeath();
    }
}
