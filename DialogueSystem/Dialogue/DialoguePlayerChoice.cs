using System;
using System.Collections.Generic;

namespace DialogueSystem.Dialogue
{
    [Serializable]
    public class DialoguePlayerChoice
    {
        public string next;
        public string choiceText;
        public List<DialogueRequirements> requirements;
        private bool _selected;

        public Dialogue GetDialogue()
        {
            return Dialogue.Load(next);
        }
        public bool GetSelected()
        {
            return _selected;
        }

        public void SetSelected(bool s)
        {
            _selected = s;
        }
        
        public DialoguePlayerChoice(string next, string choiceText)
        {
            this.next = next;
            this.choiceText = choiceText;
            requirements = new List<DialogueRequirements>();
        }
        public DialoguePlayerChoice(string next)
        {
            // Use this for dialogue player "choices" (read: the default options.
            this.next = next;
            choiceText = next;
            requirements = new List<DialogueRequirements>();
        }

        public void AddRequirement(DialogueRequirements d)
        {
            requirements.Add(d);
        }

        public string GetChoiceText()
        {
            // TODO: reformat this based on the current status (IE: can show?)
            return choiceText;
        }
    
        public Tuple<bool,String> CanShow()
        {
            Tuple<bool, string> l = DialogueRequirements.EvaluateList(requirements);
            return l;
        }

        public void Update()
        {
            Dialogue d = Dialogue.Load(next);
            d.Update();
        }

        public bool exists()
        {
            if (next == null)
            {
                return false;
            }
            if (next == "")
            {
                return false;
            }
            // return false if we cannot load!
            // TODO: actually figure out... what happens if we can't load?
            // like, what exceptions are thrown?
            // fun 
            Dialogue.Load(next);
            return true;
        }
    }
}