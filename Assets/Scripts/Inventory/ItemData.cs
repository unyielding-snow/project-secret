using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data structure for a single item.
[CreateAssetMenu(menuName = "Inventory Item Data")]
public class ItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
    public string description;
}
