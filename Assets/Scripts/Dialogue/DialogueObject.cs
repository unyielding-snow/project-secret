using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Group conversations by activation requirements. This way we don't have to check each individual conversation's requirements


namespace Assets.Scripts.Dialogue
{
    public class DialogueObject   // TODO: Still need to fine tune this after playtest
    {
        public string defaultPortrait;
        public string subtitleColor;

        public int priorityLevel;

        public List<ActivateRequirements> activateRequirement;

        public List<InteractTextLineSets> interactTextLineSets;

        //TODO: maintain encapsulation by setting all varaibles to private, and use public getters & private setters
        //public string Name
        //{
        //    get { return _name; }
        //    private set { _name = value; }
        //}
    }

    public class ActivateRequirements
    {
        public List<string> requiredTextLines;
        public List<string> requiredFalseTextLinesThisRun;
        public List<string> requiredEncounters;
    }

    public class InteractTextLineSets   // Contains all the possible dialogue lines in 
    {
        public List<Conversation> Conversations;

    }

    public class Conversation
    {
        public bool playOnce;
        public bool requireRunNotCleared;  // In safe zone or not?
        public int charactersInBackground;
        public string secondPortrait;

        public List<string> requiredFalseLines;
        public List<TextLine> textLines;
    }

    public class TextLine
    {
        // How do we do conversations with three or more characters?
        // Hades dim the third character in the bacgkround until called
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

