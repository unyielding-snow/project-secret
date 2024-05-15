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
        // TODO: Sync inConversation & inCombat with player controller
        // TODO: Maybe events?
        public bool inRange = false;
        public bool inConversation = false;
        public bool inCombat = false;

        //JSON parsing
        private DialogueObject dialogueData;
        private string jsonString;
        private string filePath;

        // Conversation Data
        private Conversation curConvo;
        private List<TextLine> textLinesSet;
        private TextLine curTextLine;
        private int curLineNum;
        private int maxLineNum;


        void Awake()
        {
            curLineNum = 0;
            CharacterName = gameObject.GetComponent<CharacterInfo>().getName();

            VisualDialogueUI = GameObject.FindGameObjectWithTag("DialogueUI");
            VisualDialogueUI.SetActive(false);
            DialogueIndicator = gameObject.transform.Find("DialogueIndicator").gameObject;
            DialogueIndicator.SetActive(false);

            // TODO: Spawn connections programatically to prevent null issues
            // i.e mainPortraitImage = GameObject.FindGameObjectWithTag("DialogueUIMainPortrait").GetComponent<Image
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

        // TODO: Change name to more fitting onInteractDialogue something
        public void PlayDialogueInteraction(Collider2D player)
        {
            if(inRange && !inCombat && player.GetComponent<PlayerController>().grounded)
            {
                // TODO: Fix Pause menuconnection. For now brute forced.
                pause.Invoke();
                Time.timeScale = 0f;

                if (inConversation)  // Continue to play from current dialogue
                {
                    PlayNextDialogue();
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

            // TODO: Find correct dialogue from activation requirements (connected to game data)
            if (isGameStateEligble())
            {
                //DialogueObject dialogueObject = JsonConvert.DeserializeObject<DialogueObject>(jsonString);
                //ActivateRequirements conversations = dialogueObject.activateRequirement;
                //ActivateRequirements ei = JsonConvert.DeserializeObject<ActivateRequirements>(jsonString);
                //Debug.Log("Test shoud be 1, in actuality " + ei.test);


                // TODO: dynamically build up a dialogue object variable, so we don't waste memory
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
                Debug.Log("final objc" + final.ToString());

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
                textLinesSet = textLineList.ToObject<List<TextLine>>();

                Debug.Log(textLineListTok.ToString());
                foreach (var item in textLinesSet)
                {
                    Debug.Log(item.text);
                }

                // Set variables
                curTextLine = textLinesSet[curLineNum];
                maxLineNum = textLinesSet.Count;
            }

            // Change Dialogue UI to JSON text
            UpdateDialogueUI();

            VisualDialogueUI.SetActive(true);
        }

        private void PlayNextDialogue()
        {
            Debug.Log("Playing next dialogue");
            if (curLineNum < maxLineNum)  // Play next line of dialogue
            {
                Debug.Log("cur = " + curLineNum + "max = " + maxLineNum);
                curTextLine = textLinesSet[curLineNum];
                UpdateDialogueUI();
            }
            else   
            {
                ExitVisualDialogue();
            }
        }

        private void UpdateDialogueUI()
        {
            // MAYBE: Use synchronization mechanisms / multi-threading to make sure dialogue data changes at the same time?
            Debug.Log(curTextLine.ToString());
            charNameDisplay.text = curTextLine.speaker;

            if(curTextLine.speaker == null)
            {
                charNameDisplay.text = "Set Default";
            }

            charEpithetDisplay.text = "temporary title";
            displayedConvoText.text = curTextLine.text;

            curLineNum++;

            // TODO: Use speaker to find epithet

        }

        private void ExitVisualDialogue()
        {
            curLineNum= 0;
            maxLineNum = 0;
            curConvo = null;
            curTextLine = null;

            // TODO: publish conversation has been fullfilled and get rid of it in pool


            // TODO: Fix pause menu event connection
            resume.Invoke();
            Time.timeScale = 1f;

            inConversation = false;
            VisualDialogueUI.SetActive(false);
        }

        private bool isGameStateEligble()  // PASS IN GAME STATE
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