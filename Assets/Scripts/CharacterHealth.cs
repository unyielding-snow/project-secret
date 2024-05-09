using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// General Health System

public class CharacterHealth : HealthSystem
{
    public int maxHealth;
    public int health;
    public bool iframesEnabled = false;

    protected override void Initialize()
    {
        base.Initialize();  // Don't hide the functions
    }

}
