using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows.Speech;

public static class DialogueManager 
{
    public static Dictionary<string, int> exhuastedDialogue;

    public static void ExhaustConversation(string convoName)
    {
        if (exhuastedDialogue.ContainsKey(convoName))
        {
            exhuastedDialogue[convoName] += 1; 
        }
        else
        {
            exhuastedDialogue[convoName] = 1; 
        }
    }

    public static void CreateNewDialogueManager()
    {
        exhuastedDialogue = new Dictionary<string, int>();
    }
}
