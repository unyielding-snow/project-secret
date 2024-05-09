using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// General Health System

public abstract class HealthSystem : MonoBehaviour
{
    // TODO: Implement UnityEvents system
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

    protected void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        events.OnDeath.Invoke();
    }

    public bool IsDead()
    {
        return isDead;
    }

    
}
