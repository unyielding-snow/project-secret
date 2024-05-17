using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inventory Manager System
[Serializable]
public static class InventorySystem
{
    public static Dictionary<ItemData, InventoryItem> itemDictionary;
    public static List<InventoryItem> inventory { get; private set; }

    public static void CreateNewInventorySystem()
    {
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<ItemData, InventoryItem>();
    }

    public static void Add(ItemData reference)
    {
        if (reference == null)
        {
            Debug.LogError("Attempt to add a null reference");
            return;
        }
        if (itemDictionary.TryGetValue(reference, out InventoryItem value))
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

    public static void Remove(ItemData reference)
    {
        if (reference == null)
        {
            Debug.LogError("Attempt to remove a null reference");
            return;
        }
        if (itemDictionary.TryGetValue(reference, out InventoryItem value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                itemDictionary.Remove(reference);
            } 
        }
    }

    public static InventoryItem Get(ItemData reference)
    {
        if (reference == null)
        {
            Debug.LogError("Attempt to get a null reference");
        }
        if (itemDictionary.TryGetValue(reference, out InventoryItem value))
        {
            return value; 
        }
        return null;
    }

    public static bool DoesItemExist(ItemData reference) 
    {
        return (Get(reference) != null);
    }

}
