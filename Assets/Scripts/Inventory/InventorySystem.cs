using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inventory Manager System
[Serializable]
public class InventorySystem
{
    private Dictionary<ItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    public InventorySystem()
    {
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<ItemData, InventoryItem>();
    }

    public void Add(ItemData reference)
    {
        if (itemDicionary.TryGetValue(reference, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(reference);
            inventory.Add(newItem);
            itemDictionary.Add(reference, newItem);
        }
    }

    public void Remove(ItemData reference)
    {
        if (itemDicionary.TryGetValue(reference, out InventoryItem value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                itemDictionary.Remove(reference);
            }
        }
    }

    public InventoryItem Get(ItemData reference)
    {
        if (itemDicionary.TryGetValue(reference, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public bool DoesItemExist(ItemData reference) 
    {
        return (Get(reference) != null);
    }
}
