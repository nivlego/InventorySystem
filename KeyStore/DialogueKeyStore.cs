using System.Collections.Generic;
using DialogueSystem.Actor;
using DialogueSystem.Dialogue;
using UnityEngine;

namespace KeyStore
{
    public class DialogueKeyStore: IKeyStore
    {
        private Actor currentActor;
        private string currentLine;
        private List<DialoguePlayerChoice> currentChoices;
        private bool currentActive;
        private bool currentDefaultContinue;
        private bool currentSelectContinue;

        public DialogueKeyStore()
        {
            currentSelectContinue = false;
            currentActor = null;
            currentActive = false;
            currentChoices = null;
            currentLine = null;
            currentDefaultContinue = false;
        }
        public string GetKeyStoreName()
        {
            return "dialogue";
        }
        public dynamic GetKey(string key)
        {
            switch (key)
            {
                case "active":
                    return currentActive;
                case "actor":
                    return currentActor;
                case "line":
                    return currentLine;
                case "choices":
                    return currentChoices;
                case "defaultContinue":
                    return currentDefaultContinue;
                default:
                    Debug.LogWarning("Unknown key from dialogue: " + key);
                    return null;
            }
        }

        public void SetKey(string key, dynamic value)
        {
            switch (key)
            {
                case "active":
                    currentActive = value;
                    return;
                case "selectContinue":
                    currentSelectContinue = value;
                    return;
                case "actor":
                    currentActor = value;
                    return;
                case "line":
                    currentLine = value;
                    return;
                case "choices":
                    currentChoices = value;
                    return;
                case "defaultContinue":
                    currentDefaultContinue = value;
                    return;
                default:
                    Debug.LogWarning("Unknown key from dialogue: " + key);
                    return;
            }
        }
    }
}