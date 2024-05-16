using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "GlobalData", menuName = "GlobalDataPersistance")]
    public class GlobalData : ScriptableObject
    {
        // ---- GLOBAL DATA ----
        public int maxPlayerHealth = 100;

        public int playerDeaths = 0;   // Death = Runs
        public int playerEnemiesSlain = 0;

        private int numNPC = 4;       // Number of NPCs to befriend 
        private int numBosses = 1;   // Number of Bosses (potentially befriend?)

        // Player NPC Relationships
        Dictionary<string, relationshipData> playerRelationships = new Dictionary<string, relationshipData>()
        {
            { "Friend", new relationshipData(false, 0, 0) }
        };

        // NPC to NPC Relationships
        // For dual dialogue interactions, we can use a 2D array between NPCs
        int[,] NPCrelationships = new int[4, 4];

        // Inventory System


        // Dialogue System


        // ---- RUN SPECIFIC DATA ----

    }

    public struct relationshipData
    {
        public bool isUnlocked;
        public int value;
        public int tier;

        public relationshipData(bool _isUnlocked, int _value, int _tier)
        {
            isUnlocked = _isUnlocked;
            value = _value;
            tier = _tier;
        }
    }
}