using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// General Health System
// TODO: Get rid of unity events

public abstract class HealthSystem : MonoBehaviour
{
    [System.Serializable]
    public struct HealthEvents
    {
        public UnityEvent OnHeal;
        public UnityEvent OnDamage;
        public UnityEvent OnDeath;

        [Tooltip("Invoked when any health change occurs")]
        public UnityEvent OnHealthChange;
    }
    public HealthEvents events;

    public bool isInvincible = false;
    bool isDead = false;

    protected int currentHealth = -1;

    void Awake()
    {
        events.OnDamage.AddListener(events.OnHealthChange.Invoke);
        events.OnDeath.AddListener(events.OnHealthChange.Invoke);
        events.OnHeal.AddListener(events.OnHealthChange.Invoke);

        Initialize();
    }

    protected virtual void Initialize() { }  // For each character to modify

    public bool Heal(int amount)
    {
        bool healed = ApplyHeal(amount);

        if (healed)
        {
            events.OnHeal.Invoke();
        }

        return healed;
    }

    protected abstract bool ApplyHeal(int amount);

    public bool Damage(int amount, object obj = null)
    {
        if (isInvincible)
        {
            return false;
        }

        bool damaged = ApplyDamage(amount);

        if (damaged)
        {
            events.OnDamage.Invoke();
        }

        return damaged;
    }

    protected abstract bool ApplyDamage(int amount);

    protected abstract void Die();
 

    public bool IsDead()
    {
        return isDead;
    }

    public void SetDead(bool target)
    {
        isDead = target;
    }

}
