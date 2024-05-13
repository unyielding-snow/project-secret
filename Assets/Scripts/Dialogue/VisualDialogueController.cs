using Assets.Scripts.Dialogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class VisualDialogueController : MonoBehaviour
    {
        private string CharacterName;
        public struct DialogueEvent
        {
            public UnityEvent TriggerDialogue;
            public UnityEvent ContinueDialogue;
        }

        public UnityEvent pause;

        [SerializeField] private GameObject DialogueUI;
        private GameObject DialogueIndicator;
        public bool activateOnPlayerEnter = false;
        [SerializeField] private BoxCollider2D dialogueRange;

        public bool inRange = false;
        public bool inConversation = false;
        public bool inCombat = false;

        //JSON parsing
        private DialogueObject dialogueData;
        private string jsonString;
        private string filePath;

        void Awake()
        {
            DialogueUI.SetActive(false);

            DialogueIndicator = gameObject.transform.Find("DialogueIndicator").gameObject;
            DialogueIndicator.SetActive(false);

            CharacterName = gameObject.GetComponent<CharacterInfo>().getName();

            // Could generate dialogue box by code, or specification 
            dialogueRange = gameObject.AddComponent<BoxCollider2D>();
            dialogueRange.size = new Vector2(19f, 15f); 
            dialogueRange.isTrigger = true;

            pause.AddListener(GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseMenu>().Pause);
        }

        // TODO: What happens if there are mutliple charaters per scene, and OnTriggerEnter2D overlaps?
        // Do we design scenarios to never use this, and plan for later?
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")){
                if (activateOnPlayerEnter)
                {
                    PlayDialogueInteraction(collision);
                }
                else
                {
                    inRange = true;
                    DialogueIndicator.SetActive(true);  // TODO: Make this an fade in animation in the future
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                inRange = false;
                if (!activateOnPlayerEnter)
                {
                    DialogueIndicator.SetActive(false);
                }
            }
        }

        public void PlayDialogueInteraction(Collider2D player)
        {
            if(inRange && !inCombat && player.GetComponent<PlayerController>().grounded)
            {
                pause.Invoke();

                // CURRENT: Just test reading json file of data
                if (inConversation)  // Continue to play from current dialogue
                {
                    PlayNextDialogue();
                }
                else
                {
                    GetNewDialogue();

                    DialogueUI.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Cannot play dialogue from state");
            }
        }

        private void GetNewDialogue()
        {
            // Preface: Imported NewtonSoft and implemented partial deserialization for optimizing read operations
            //          Either use JSONObject or only deserialize certain objects

            // TODO: Implement priority system
            // Find right dialogue by naming convetion
            // Relationship level, and quest sequencing?
            // We can keep track of quest completed, and bosses killed in game data
            // If we keep NPC relationships seperate from main quest, then NPC's relationship level will become their quest sequencing. This is ideal for simplicity
            // Protagonist can just talk to themselves about the previous run at the begining of the respawn point, solving the mystery.

            int relationshipLevel = 0;   // Template variable
            filePath = Path.Combine(Application.dataPath, "Dialogue/DialogueObjects/" + CharacterName + "_" + relationshipLevel + ".json");
            jsonString = File.ReadAllText("filePath");
            List<ActivateRequirements> dialogueRequirements = JsonConvert.DeserializeObject<List<ActivateRequirements>>(jsonString); 
            Debug.Log(filePath);
            // Or we can use :  JObject jsonObject = JObject.Parse(jsonString);


            // TODO: Find correct dialogue from activation requirements (connected to game data)
            // Activation requirement should be a struct of game data
            bool gameDataRequirements = true;
            if (gameDataRequirements)
            {
                // Whole branch of conversations can be used, randomy generate one
                List<Conversation> convos = JsonConvert.DeserializeObject<List<Conversation>>(jsonString);
                int conversationSize = convos.Count;


            }

            // Change Dialogue UI to what 
            UpdateDialogueUI();
            DialogueUI.SetActive(true);
        }

        private void PlayNextDialogue()
        {
            // Get Next Line of Dialogue

            
        }

        private void UpdateDialogueUI()
        {
            
        }

        private bool isGameStateEligble()
        {
            return true;
        }

        void OnDrawGizmos()  // Debugging Purposes
        {
            if (dialogueRange != null)
            {
                Gizmos.color = Color.green; // Set the color of the Gizmos
                Gizmos.matrix = transform.localToWorldMatrix; // Use the object's local matrix

                // Draw the BoxCollider2D's outline using Gizmos
                Gizmos.DrawWireCube(dialogueRange.offset, dialogueRange.size);
            }
        }

    }
}