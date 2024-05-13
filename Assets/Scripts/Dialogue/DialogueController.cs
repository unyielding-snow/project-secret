using Assets.Scripts.Dialogue;
using System.Collections;
using System.IO;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class DialogueController : MonoBehaviour
    {
        private string CharacterName;
        public struct DialogueEvent
        {
            public UnityEvent TriggerDialogue;
            public UnityEvent ContinueDialogue;
        }

        [SerializeField] private GameObject DialogueUI;
        private GameObject DialogueIndicator;
        public bool activateOnPlayerEnter = false;
        [SerializeField] private BoxCollider2D dialogueRange;


        public bool inRange = false;
        public bool inConversation = false;
        public bool inCombat = false;


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
                // TODO: Go through each layer of dialogue in priority, randomly pick one if able
                // CURRENTLY: Just test reading json file of data

                string filePath = Path.Combine(Application.dataPath, "Dialogue/" + CharacterName + ".json");
                string json = File.ReadAllText("filePath");
                Debug.Log(filePath);

                DialogueObject myData = JsonUtility.FromJson<DialogueObject>(json);
            }
            else
            {
                Debug.Log("Cannot play dialogue from state");
            }
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

        private bool isGameStateEligble()
        {
            return true;
        }
    }
}