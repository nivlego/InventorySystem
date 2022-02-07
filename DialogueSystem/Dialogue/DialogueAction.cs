using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Dialogue
{
    [Serializable]
    public class DialogueAction
    {
        public string actionType;
        public string key;
        public dynamic value;
        public List<DialogueRequirements> requirements;
        
        public DialogueAction(string actionType, string key, dynamic value)
        {
            this.actionType = actionType;
            this.key = key;
            this.value = value;
            requirements = new List<DialogueRequirements>();
        }
        public DialogueAction(string actionType, dynamic value)
        {
            this.actionType = actionType;
            key = null;
            this.value = value;
            requirements = new List<DialogueRequirements>();
        }

        public void AddRequirement(DialogueRequirements d)
        {
            requirements.Add(d);
        }
    
        public void Evaluate()
        {
            Tuple<bool, string> l = DialogueRequirements.EvaluateList(requirements);
            if (l.Item1)
            {
                Debug.LogWarning("unhandled action type: " + actionType);
            }
            else
            {
                Debug.LogWarning("no way to display errors from DialogueAction.Evaluate - " + l.Item2);
            }
        }
    }
}