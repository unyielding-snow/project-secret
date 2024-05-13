using Assets.Scripts.Dialogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;


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
        public UnityEvent resume;

        [Header("Behaviour")]
        public bool activateOnPlayerEnter = false;

        [Header("Visual Dialogue UI")]
        [SerializeField] private GameObject VisualDialogueUI;
        [SerializeField] private BoxCollider2D dialogueRange;
        private GameObject DialogueIndicator;
        public Image mainPortraitImage;
        public TMP_Text charNameDisplay;
        public TMP_Text charEpithetDisplay;
        public TMP_Text displayedConvoText;


        [Header("States")]
        public bool inRange = false;
        public bool inConversation = false;
        public bool inCombat = false;

        //JSON parsing
        private DialogueObject dialogueData;
        private string jsonString;
        private string filePath;

        // Conversation Data
        private Conversation currentConvo;
        private int currentConvoLine;


        void Awake()
        {
            currentConvoLine = 0;
            CharacterName = gameObject.GetComponent<CharacterInfo>().getName();

            VisualDialogueUI = GameObject.FindGameObjectWithTag("DialogueUI");
            VisualDialogueUI.SetActive(false);
            DialogueIndicator = gameObject.transform.Find("DialogueIndicator").gameObject;
            DialogueIndicator.SetActive(false);

            // Get mutable objects
            //mainPortraitImage = GameObject.FindGameObjectWithTag("DialogueUIMainPortrait").GetComponent<Image>();
            // TODO: Spawn connections programatically to prevent null issues
            if (mainPortraitImage == null)
                Debug.LogError("Image component not found!");

            if(charNameDisplay == null)
                Debug.LogError("TextMeshPro - Text component not found!");

            // Currently programatically dialogue box by code, could also generate by specification
            dialogueRange = gameObject.AddComponent<BoxCollider2D>();
            dialogueRange.size = new Vector2(19f, 15f); 
            dialogueRange.isTrigger = true;

            // TODO: Sending a Pause event to Pause Controller when in dialogue
            // Error: Can't find an inactive object
            //pause.AddListener(GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseMenu>().Pause);
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
                // TODO: Fix connection. For now brute forced.
                pause.Invoke();
                Time.timeScale = 0f;

                // CURRENT: Just test reading json file of data
                if (inConversation)  // Continue to play from current dialogue
                {
                    // TODO: Get rid of placeholder
                    //PlayNextDialogue(currentConvo);
                    ExitVisualDialogue();
                }
                else
                {
                    PlayNewVisualDialogue();
                    inConversation = true;
                    VisualDialogueUI.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Cannot play dialogue from state");
            }
        }

        private void PlayNewVisualDialogue()
        {
            // Preface: Imported NewtonSoft and implemented partial deserialization for optimizing read operations
            //          Either use JSONObject or only deserialize certain objects

            // TODO: Implement priority system
            // Find right dialogue by naming convetion
            // Relationship level, and quest sequencing?
            // We can keep track of quest completed, and bosses killed in game data
            // If we keep NPC relationships seperate from main quest, then NPC's relationship level will become their quest sequencing. This is ideal for simplicity
            // Protagonist can just talk to themselves about the previous run at the begining of the respawn point, solving the mystery.

            // TODO:
            int relationshipLevel = 0;   // Template variable
            string characterType = "Boss_";
            filePath = Application.dataPath + "/Scripts/Dialogue/DialogueObjects/" + characterType + CharacterName + "_" + relationshipLevel + ".json";
            Debug.Log(Application.dataPath);
            Debug.Log(filePath);
            jsonString = File.ReadAllText(filePath);
            //ActivateRequirements dialogueRequirements = JsonConvert.DeserializeObject<ActivateRequirements>(jsonString); 
            // Or we can use: JObject jsonObject = JObject.Parse(jsonString);


            // TODO: Find correct dialogue from activation requirements (connected to game data)
            // Activation requirement should be a struct of game data
            if (!isGameStateEligble())
            {
                List<Conversation> conversations = JsonConvert.DeserializeObject<List<Conversation>>(jsonString);
                int convoSize = conversations.Count;
                int selectedConvo = Random.Range(0, convoSize);
                Debug.Log("Conversation size: " + convoSize + ", Randomly Selected Conversation " + selectedConvo);
                currentConvo = conversations[selectedConvo];
            }

            // Change Dialogue UI to JSON text

            //TODO: UpdateDialogueUI(currentConvo);
            VisualDialogueUI.SetActive(true);
        }

        private void PlayNextDialogue(Conversation convo)
        {
            // Get Next Line of Dialogue
            currentConvoLine++;
            if(currentConvoLine >= convo.textLines.Count)
            {
                ExitVisualDialogue();
            }
            
        }

        private void UpdateDialogueUI(Conversation convo)
        {
            // TODO: Use multi-threading / synchronization mechanisms to make sure dialogue data changes at the same time?
            
        }

        private void ExitVisualDialogue()
        {
            currentConvoLine = 0;
            currentConvo = null;

            // TODO: publish conversation has been fullfilled and get rid of it in pool

            // TODO: Fix pause menu event connection
            resume.Invoke();
            Time.timeScale = 1f;

            inConversation = false;
            VisualDialogueUI.SetActive(false);
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