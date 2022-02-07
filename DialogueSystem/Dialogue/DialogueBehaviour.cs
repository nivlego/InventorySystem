using UnityEngine;

namespace DialogueSystem.Dialogue
{
    public class DialogueBehaviour: MonoBehaviour
    {
        public DialogueSystem.Dialogue.Dialogue d;
        public string dialoguePath;
        void Start()
        {
            d = DialogueSystem.Dialogue.Dialogue.Load(dialoguePath);
        }

        private void Update()
        {
            d.Update();
        }
    }
}