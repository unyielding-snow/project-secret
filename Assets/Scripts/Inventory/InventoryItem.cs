using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data structure to manage a single item derived from ItemData.
[Serializable]
public class InventoryItem
{
    public ItemData data { get; private set; }
    public int stackSize { get; private set; }
    
    public InventoryItem(ItemData val)
    {
        if (val == null)
        {
            Debug.LogError("Attempt to create a null InventoryItem.", val);
            return;
        }
        data = val;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    // Add more functionality if needed or build a child class off this.
}
