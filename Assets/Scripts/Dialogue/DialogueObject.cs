using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Group conversations by activation requirements. 


namespace Assets.Scripts.Dialogue
{
    public class DialogueObject   
    {
        public string defaultPortrait;
        public string subtitleColor;

        public int priorityLevel;

        // Looks weird but neccsiary for json parsing
        public ActivateRequirements activateRequirement;

        public InteractTextLineSets conversations;
    }

    public class ActivateRequirements
    {
        public int test;
        public List<string> requiredTextLines;
        public List<string> requiredFalseTextLinesThisRun;
        public List<string> requiredEncounters;
    }

    // Just a holder for a list of conversations, for JSON serialization
    public class InteractTextLineSets   
    {
        public List<Conversation> IextLineSets;
    }

    public class Conversation
    {
        public bool playOnce;
        public bool requireRunNotCleared;   // In vilage safe zone or not?
        public int charactersInBackground;  // Hades dim the third character in the bacgkround until called for duo boons 
        public string secondPortrait;

        public List<string> requiredFalseLines;
        public List<TextLine> textLines;
    }

    public class TextLine
    {
        public string audioCue;
        public string expression;
        public string portrait;
        public string speaker;

        public string text;
    }

}


// NPC controlls what dialogues you get, not the player 
// if you want to talk to NPC at a certain relationship level, 
// Think about a first unlock

