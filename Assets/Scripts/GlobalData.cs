using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


//[CreateAssetMenu(fileName = "GlobalData", menuName = "GlobalDataPersistance")]
public static class GlobalData
{
    // ---- GLOBAL DATA ----
    private static int defaultHeath = 100;
    public static int maxPlayerHealth = 100;

    public static int playerDeaths = 0;   // Death = Runs
    public static int totalEnemiesSlain = 0;
    public static int totalBossesSlain = 0;

    private static int numNPC = 4;       // Number of NPCs to befriend 
    private static int numBosses = 1;   // Number of Bosses (potentially befriend?)

    // Player NPC Relationships
    public static Dictionary<string, relationshipData> playerRelationships;

    // Potential Feature:
    // NPC to NPC Relationships: For dual dialogue interactions, we can use a 2D array between NPCs
    // public static int[,] NPCrelationships = new int[4, 4];


    // ---- RUN SPECIFIC DATA ----
    public static int runEnemiesSlain = 0;
    public static int runBossesSlain = 0;


    public static void CreateNewGlobaData()
    {
        maxPlayerHealth = defaultHeath;
        playerDeaths = 0;
        totalEnemiesSlain = 0;

        playerRelationships = new Dictionary<string, relationshipData>();
    }

    public static void NewRun()   // Reset Run Specific Data
    {
        runEnemiesSlain = 0;
        runBossesSlain = 0;
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

    public static void AddDeath(string enemyType)
    {
        if(enemyType == "mob")
        {
            runEnemiesSlain++;
            totalBossesSlain++;
        }
        else if (enemyType == "boss")
        {
            runBossesSlain++;
            totalBossesSlain++;
        }
    }

}
