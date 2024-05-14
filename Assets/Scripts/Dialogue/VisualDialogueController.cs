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
using System;
using System.ComponentModel;


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
            if (collision.CompareTag("Player") && !inRange){  // !inRange prevents overlapping dialogue
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
            if (collision.CompareTag("Player"))  // TODO: Think of in range bug scenario
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
                // TODO: Fix Pause menuconnection. For now brute forced.
                pause.Invoke();
                Time.timeScale = 0f;

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

            // TODO: Make variables dynamic
            int relationshipLevel = 0;   
            string characterType = "Boss_";
            filePath = Application.dataPath + "/Scripts/Dialogue/DialogueObjects/" + characterType + CharacterName + "_" + relationshipLevel + ".json";
            jsonString = File.ReadAllText(filePath);
            Debug.Log(filePath);
            //ActivateRequirements dialogueRequirements = JsonConvert.DeserializeObject<ActivateRequirements>(jsonString); 
            // Or we can use: JObject jsonObject = JObject.Parse(jsonString);


            // TODO: Find correct dialogue from activation requirements (connected to game data)
            if (isGameStateEligble())
            {
                //Debug.Log(d.activateRequirement.test);
                //DialogueObject dialogueObject = JsonConvert.DeserializeObject<DialogueObject>(jsonString);
                //Debug.Log(dialogueObject.defaultPortrait);

                //ActivateRequirements conversations = dialogueObject.activateRequirement;
                //ActivateRequirements ei = JsonConvert.DeserializeObject<ActivateRequirements>(jsonString);
                //Debug.Log("Test shoud be 1, in actuality " + ei.test);

                JObject obj = JObject.Parse(jsonString);
                Debug.Log("Priority " + (int)obj["priority"]);
                JObject activatereq = (JObject)obj["ActivateRequirements"];

                Debug.Log("Test " + (int)activatereq["Test"]);

                JToken itemsToken = obj["ActivateRequirements"]["RequiredTextLines"];
                Debug.Log("JToken");
                Debug.Log(itemsToken.ToString());

                JArray reqLines = JArray.Parse(itemsToken.ToString());
                Debug.Log(reqLines.ToString());
                int i = 0;
                foreach (var element in reqLines)
                {
                    i++;
                    Debug.Log(element);
                }
                Debug.Log(i);

                JToken objFinal = obj["InteractTextLineSets"];
                Debug.Log(objFinal.ToString());
                JObject final = JObject.Parse(objFinal.ToString());

                Debug.Log("Text Line Sets");

                // Justin: I don't like the dictionary format. Let's try a list. 
                //         List will disregards the name of the conversation, which is good [names are only for internal cataloging]

                Dictionary<string, JObject> convoDict = new Dictionary<string, JObject>();
                List<JObject> convoList = new List<JObject>();

                foreach (var property in final.Properties())
                {
                    Debug.Log(property.Name);
                    Debug.Log((JObject)property.Value);
                    convoDict.Add(property.Name, (JObject)property.Value);
                    convoList.Add((JObject)property.Value);
                }

                Debug.Log("Conversation List Size: " + convoList.Count);

                Debug.Log("List of items in Conversation 0");
                foreach (var item in convoList[0].Properties())
                {
                    Debug.Log(item.Name);
                    Debug.Log(item.Value);
                }

                Debug.Log("List of text lines");
                JToken textLineListTok = convoList[0]["TextLines"];
                JArray textLineList = JArray.Parse(textLineListTok.ToString());
                List<TextLine> textLines = textLineList.ToObject<List<TextLine>>();

                Debug.Log(textLineListTok.ToString());
                foreach (var item in textLines)
                {
                    Debug.Log(item.text);
                }

                //Conversation conv = conversations.Conversations[0];
                //int convoSize = InterationSet.Conversations.Count;
                //int selectedConvo = Random.Range(0, 0);
                //Debug.Log("Conversation size: " + convoSize + ", Randomly Selected Conversation " + selectedConvo);
                //currentConvo = InterationSet.Conversations[selectedConvo];
            }

            // Change Dialogue UI to JSON text
            UpdateDialogueUI(currentConvo);

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
            //mainPortraitImage.Image;
            charNameDisplay.text = convo.textLines[0].speaker;
            charEpithetDisplay.text = "the protagonist";
            displayedConvoText.text = convo.textLines[0].text;  
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
            // Activation requirement should be a struct of game data
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