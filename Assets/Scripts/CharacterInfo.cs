using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterInfo : MonoBehaviour
    {
        // Make sure to use prefabs to create new NPCs, so we don't make error's on name 
        [SerializeField] private string CharacterName = "DefaultName";
        [SerializeField] private string Epithet = "DefaultEpithet";

        [SerializeField] private int relationshipMeter = 0;
        // Perhaps we want a grid / web of relationships, to be implemented in relationship mananger

        public string getName()
        {
            if(CharacterName == null)
            {
                Debug.LogError("Error: Null Name from CharacterInfo at " + gameObject.name);
            }    
            return CharacterName;
        }

        public string getEpithet()
        {
            if (Epithet == null)
            {
                Debug.LogError("Error: Null Epithet from CharacterInfo at " + gameObject.name);
            }
            return Epithet;
        }

    }
}